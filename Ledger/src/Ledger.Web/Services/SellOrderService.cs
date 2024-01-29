using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Jobs;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Ledger.Ledger.Web.Services
{
    
    public interface ISellOrderService
    {
        Task<IEnumerable<SellOrder>> GetAllSellOrdersAsync();
        Task<SellOrder> GetSellOrderByIdAsync(int id);
        Task<SellOrder> AddSellOrderAsync(SellOrder sellOrder);
        Task<SellOrder> UpdateSellOrderAsync(int id, SellOrder newSellOrder);
        Task<SellOrder> DeleteSellOrderAsync(int id);
        Task<SellOrder> MatchSellOrdersAsync(int sellOrderId); //returns the ending sellOrder with Matched List of BuyOrders
        Task<SellOrder> OperateTradeAsync(int sellOrderId); //do necessary changes in stocksOfUser, sellOrder, transaction, stock tables.
        Task<Stock> UpdateStockPriceAsync(int stockId, double newPrice, int tradeSize);
        Task<SellOrder> OperateSellOrderAsync(int sellOrderId, int size);
        Task<BuyOrder> OperateBuyOrderAsync(int buyOrderId, int size);

        Task<int> GetLatestSellOrderId();
        Task<IEnumerable<Transaction>> GetTransactionsOfASellOrder(int sellOrderId); // returns transactions related to a sellOrder

    }
    public class SellOrderService :ISellOrderService
    {
        private readonly ISellOrderRepository _sellOrderRepository;
        private readonly IBuyOrderRepository _buyOrderRepository;
        private readonly IStocksOfUserRepository _stocksOfUserRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISellOrderMatchRepository _sellOrderMatchRepository;
        private readonly IStockRepository _stockRepository;
        private IUnitOfWork _unitOfWork;

        public SellOrderService(ISellOrderRepository sellOrderRepository, IBuyOrderRepository buyOrderRepository,IStocksOfUserRepository stocksOfUserRepository, ITransactionRepository transactionRepository,IUserRepository userRepository, ISellOrderMatchRepository sellOrderMatchRepository, IStockRepository stockRepository, IUnitOfWork unitOfWork)
        {
            _sellOrderRepository = sellOrderRepository;
            _buyOrderRepository = buyOrderRepository;
            _stocksOfUserRepository = stocksOfUserRepository;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _sellOrderMatchRepository = sellOrderMatchRepository;
            _stockRepository = stockRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<SellOrder>> GetAllSellOrdersAsync()
        {
            return await _sellOrderRepository.GetAllSellOrdersAsync();
        }

        public async Task<SellOrder> GetSellOrderByIdAsync(int id)
        {
            return await _sellOrderRepository.GetSellOrderByIdAsync(id);
        }

        public async Task<SellOrder> AddSellOrderAsync(SellOrder sellOrder)
        {
            await _sellOrderRepository.AddSellOrderAsync(sellOrder);
            await _unitOfWork.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<SellOrder> UpdateSellOrderAsync(int id, SellOrder newSellOrder)
        {
            var sellOrder = await _sellOrderRepository.UpdateSellOrderAsync(id,newSellOrder);
            await _unitOfWork.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<SellOrder> DeleteSellOrderAsync(int id)
        {
            var sellOrder =  await _sellOrderRepository.DeleteSellOrderAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<SellOrder>
            MatchSellOrdersAsync(int sellOrderId) // for a sell order, it matches buyOrder with same Price and id
            // then add matched buyOrders to the Matched List up to the size  limit
        {
            // get the related sellOrder
            var sellOrder = await _sellOrderRepository.GetSellOrderByIdAsync(sellOrderId);

            //get all buyOrders and search for a match
            var buyOrders = await _buyOrderRepository.GetAllBuyOrdersAsync();
            var totalSize = 0;

            foreach (var buyOrder in buyOrders)
            {
                //check for the stockId and price and deleted --status:false/not deleted --status:true
                if ((buyOrder.Status == OrderStatus.Active || buyOrder.Status == OrderStatus.PartiallyCompletedAndActive) && sellOrder.StockId == buyOrder.StockId &&
                    sellOrder.AskPrice == buyOrder.BidPrice)
                {
                    // add  new match record to the sellOrderMatchs
                    await _sellOrderMatchRepository.AddSellOrderMatchAsync(sellOrderId, buyOrder.BuyOrderId);
                    totalSize = totalSize + buyOrder.CurrentBidSize;

                    if (totalSize >= sellOrder.CurrentAskSize)
                    {
                        break;
                    }
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return sellOrder;
        }


        public async Task<SellOrder> OperateTradeAsync(int sellOrderId) //operate trade, form transactions
        {
            _unitOfWork.BeginTransaction();
            try
            {
                //get related sellOrder
                var sellOrder = await _sellOrderRepository.GetSellOrderByIdAsync(sellOrderId);
                
                //get sellOrderMatch list --> list of ids of matched buyOrders
                var buyOrders = await _sellOrderMatchRepository.GetMatchedBuyOrders(sellOrderId);
                
                foreach (var buyOrderId in buyOrders)
                {
                    var buyOrder = await _buyOrderRepository.GetBuyOrderByIdAsync(buyOrderId);
                    if (buyOrder.Status == OrderStatus.Active || buyOrder.Status == OrderStatus.PartiallyCompletedAndActive) //if buyOrder is not deleted, do the operation
                    {
                        var size = new int();
                        // determine the size
                        if (sellOrder.CurrentAskSize < buyOrder.CurrentBidSize) //lowest one determines 
                        {
                            size = sellOrder.CurrentAskSize;
                            buyOrder.Status = OrderStatus.PartiallyCompletedAndActive;

                        }
                        else
                        {
                            size = buyOrder.CurrentBidSize;
                            sellOrder.Status = OrderStatus.PartiallyCompletedAndActive; //partially completed
                        }
                        //save changes 
                        await _unitOfWork.SaveChangesAsync();
                        //operate the trade between matched sellOrder and buyOrder
                        //update sellOrder and buyOrder related information
                        await this.OperateSellOrderAsync(sellOrderId, size);
                        await this.OperateBuyOrderAsync(buyOrderId, size);
                
                        //create new transactions
                        await _transactionRepository.AddTransactionAsync(new Transaction(default,sellOrderId, buyOrderId, sellOrder.UserId,
                            buyOrder.UserId, sellOrder.StockId, size, sellOrder.AskPrice));
                        await _unitOfWork.SaveChangesAsync();
                        //update price info according to this trade
                        await this.UpdateStockPriceAsync(sellOrder.StockId, sellOrder.AskPrice, size);
                    }
                }
                
                //save all changes at once
                await _unitOfWork.CommitAsync();
                return sellOrder;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBackAsync();
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<SellOrder> OperateSellOrderAsync(int sellOrderId, int size) //changes will be saved at the end of trade operation
        {
            var sellOrder = await _sellOrderRepository.GetSellOrderByIdAsync(sellOrderId);
            //update stocks of user
            await _stocksOfUserRepository.UpdateStockInfo(sellOrder.UserId, sellOrder.StockId, '-', size);
        
            //update budget 
            var amount = sellOrder.AskPrice * size;
            await _userRepository.UpdateBudget(sellOrder.UserId, '+', amount);
        
            // update AskSize in sellOrder
            await _sellOrderRepository.UpdateAskSizeAsync(sellOrderId, size); 
        
            //if the AskSize is 0 logically delete
            if (sellOrder.CurrentAskSize == 0)
            {
                await _sellOrderRepository.LogicalDelete(sellOrderId);
            }

            await _unitOfWork.SaveChangesAsync();
            return sellOrder;
        }

        public async Task<BuyOrder> OperateBuyOrderAsync(int buyOrderId, int size)
        {
            var buyOrder = await _buyOrderRepository.GetBuyOrderByIdAsync(buyOrderId);
            //update stocks of user if this stock exists else add new record for new stock
            var stocksOfUser = await _stocksOfUserRepository.GetAStocksOfUserAsync(buyOrder.UserId, buyOrder.StockId);
            if (stocksOfUser == null)
            {
                // add new stocksofuser record
                await _stocksOfUserRepository.AddStocksOfUserAsync(new StocksOfUser(buyOrder.UserId, buyOrder.StockId, size));
            }
            else
            {
                await _stocksOfUserRepository.UpdateStockInfo(buyOrder.UserId, buyOrder.StockId, '+', size);
            }
            
            //update budget 
            var amount = buyOrder.BidPrice * size;
            await _userRepository.UpdateBudget(buyOrder.UserId, '-', amount);
            
            //update bidSize in buyOrder
            await _buyOrderRepository.UpdateBidSize(buyOrderId, size);
            
            //if the BidSize is 0 logically delete
            if (buyOrder.CurrentBidSize == 0)
            {
                await _buyOrderRepository.LogicalDelete(buyOrderId);
            }

            await _unitOfWork.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<int> GetLatestSellOrderId() // return the latest sellOrderId and active olmalı !!!!!
        {
            var sellOrders = await _sellOrderRepository.GetAllSellOrdersAsync();
            var max = 0;
            foreach (var sellOrder in sellOrders)
            {
                if (sellOrder.SellOrderId > max)
                {
                    max = sellOrder.SellOrderId;
                }
            }
            return max;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsOfASellOrder(int sellOrderId)
        {
            return await _transactionRepository.GetTransactionsOfASellOrder(sellOrderId);
        }

        public async Task<Stock> UpdateStockPriceAsync(int id,double newPrice, int tradeSize)
        {
            
            var stock = await _stockRepository.GetStockByIdAsync(id);
            var updatedStock = new Stock();
            if (stock == null) return null;

            var currentPrice = stock.CurrentPrice + ((newPrice - stock.CurrentPrice) * tradeSize) / stock.InitialStock;
        
            if (currentPrice < stock.LowestPrice) //update lowest price and current price as newPrice
            {
                updatedStock =await _stockRepository.UpdateStockPriceAsync(id, currentPrice, stock.HighestPrice, currentPrice);
            }

            else if (newPrice > stock.HighestPrice) //update highest price and current price as newPrice
            {
                updatedStock = await _stockRepository.UpdateStockPriceAsync(id, currentPrice, currentPrice, stock.LowestPrice);
            }
            else{
                // update only current price
                updatedStock = await _stockRepository.UpdateStockPriceAsync(id, currentPrice, stock.HighestPrice, stock.LowestPrice);
            }

            await _unitOfWork.SaveChangesAsync();
            return updatedStock;
        }
        
    }
}