using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.UnitOfWork;
using Microsoft.VisualBasic;

namespace Ledger.Ledger.Web.Services
{
    public interface IBuyOrderService
    {
        Task<IEnumerable<BuyOrder>> GetAllBuyOrdersAsync();
        Task<BuyOrder> GetBuyOrderByIdAsync(int id);
        Task<BuyOrder> AddBuyOrderAsync(BuyOrder buyOrder);
        Task<BuyOrder> UpdateByOrderAsync(int id, BuyOrder newbuyOrder);
        Task<BuyOrder> DeleteBuyOrderAsync(int id);
        //Task<BuyOrder> MatchBuyOrdersAsync(int buyOrderId);
        Task<BuyOrder> OperateTradeAsync(int buyOrderId);
        Task<Stock> UpdateStockPriceAsync(int stockId, double newPrice, int tradeSize);
        Task<SellOrder> OperateSellOrderAsync(int sellOrderId, int size);
        Task<BuyOrder> OperateBuyOrderAsync(int buyOrderId, int size);
        Task<IEnumerable<int>> GetLatestBuyOrderIds();

        Task<IEnumerable<Transaction>> GetTransactionsOfABuyOrder(int buyOrderId);
    }
    public class BuyOrderService :IBuyOrderService
    {
        private readonly IBuyOrderRepository _buyOrderRepository;
        private readonly ISellOrderRepository _sellOrderRepository;
        private readonly IStocksOfUserRepository _stocksOfUserRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBuyOrderMatchRepository _buyOrderMatchRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IBuyOrderProcessRepository _buyOrderProcessRepository;
        private IUnitOfWork _unitOfWork;

        public BuyOrderService(IBuyOrderRepository buyOrderRepository, ISellOrderRepository sellOrderRepository,IStocksOfUserRepository stocksOfUserRepository, ITransactionRepository transactionRepository,IUserRepository userRepository, IBuyOrderMatchRepository buyOrderMatchRepository, IStockRepository stockRepository,IBuyOrderProcessRepository buyOrderProcessRepository, IUnitOfWork unitOfWork)
        {
            _buyOrderRepository = buyOrderRepository;
            _sellOrderRepository = sellOrderRepository;
            _stocksOfUserRepository = stocksOfUserRepository;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _buyOrderMatchRepository = buyOrderMatchRepository;
            _stockRepository = stockRepository;
            _buyOrderProcessRepository = buyOrderProcessRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<BuyOrder>> GetAllBuyOrdersAsync()
        {
            return await _buyOrderRepository.GetAllBuyOrdersAsync();
        }

        public async Task<BuyOrder> GetBuyOrderByIdAsync(int id)
        {
            return await _buyOrderRepository.GetBuyOrderByIdAsync(id);        }

        public async Task<BuyOrder> AddBuyOrderAsync(BuyOrder buyOrder)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                await _buyOrderRepository.AddBuyOrderAsync(buyOrder);
                await _unitOfWork.SaveChangesAsync();
                // add buy order process
                await this.AddBuyOrderProcessByBuyOrder(buyOrder);
                await _unitOfWork.CommitAsync();
                return buyOrder;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBackAsync();
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<BuyOrder> UpdateByOrderAsync(int id, BuyOrder newbuyOrder)
        {
            var buyOrder = await _buyOrderRepository.UpdateByOrderAsync(id, newbuyOrder);
            await _unitOfWork.SaveChangesAsync();
            return buyOrder;
        }

        public async Task<BuyOrder> DeleteBuyOrderAsync(int id)
        {
            var buyOrder = await _buyOrderRepository.DeleteBuyOrderAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return buyOrder;
        }
        
        /*public async Task<BuyOrder> MatchBuyOrdersAsync(int buyOrderId)
        {
            //get related buyOrder
            var buyOrder = await _buyOrderRepository.GetBuyOrderByIdAsync(buyOrderId);
            
            //get ids of matched sellOrders
            var sellOrders = await _sellOrderRepository.GetMatchedSellOrderIds(buyOrder);
            if (sellOrders == null)
            {
                return null;
            }
            //change status of buyOrder
            buyOrder.Status = OrderStatus.IsMatched;
            await _unitOfWork.SaveChangesAsync();
            
            foreach (var sellOrderId in sellOrders)
            {
                //add new match record to the buyOrder matches
                await _buyOrderMatchRepository.AddBuyOrderMatchAsync(buyOrderId, sellOrderId);
                
            }
            await _unitOfWork.SaveChangesAsync();
            return buyOrder;

        }*/
        public async Task<BuyOrder> OperateTradeAsync(int buyOrderId)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                // get related buyOrder
                var buyOrder = await _buyOrderRepository.GetBuyOrderByIdAsync(buyOrderId);
                
                //get BuyOrderMatch list --> list of ids of matched sellOrders
                var sellOrders = await _buyOrderMatchRepository.GetMatchedSellOrders(buyOrderId);

                foreach (var sellOrderId in sellOrders)
                {
                    var sellOrder = await _sellOrderRepository.GetSellOrderByIdAsync(sellOrderId);
                    if (sellOrder.Status == OrderStatus.IsMatched ) //if sellOrder is not deleted, do the operation
                    {
                        //determine the size
                        var size = new int();
                        if (buyOrder.CurrentBidSize < sellOrder.CurrentAskSize) //the lowest determines the size
                        {
                            size = buyOrder.CurrentBidSize;
                            sellOrder.Status = OrderStatus.PartiallyCompletedAndActive;
                        }
                        else
                        {
                            size = sellOrder.CurrentAskSize;
                            buyOrder.Status = OrderStatus.PartiallyCompletedAndActive;
                        }
                        //save changes
                        await _unitOfWork.SaveChangesAsync();
                        //operate the trade between matched buyOrder and sellOrder
                        //update sellOrder and buyOrder related information 
                        await this.OperateBuyOrderAsync(buyOrderId, size);
                        await this.OperateSellOrderAsync(sellOrderId, size);
                        
                        //create new transaction
                        await _transactionRepository.AddTransactionAsync(new Transaction(default, sellOrderId,
                            buyOrderId, sellOrder.UserId, buyOrder.UserId, buyOrder.StockId, size, buyOrder.BidPrice));
                        //save changes
                        await _unitOfWork.SaveChangesAsync();
                        //update price info according to this trade
                        await this.UpdateStockPriceAsync(buyOrder.StockId, buyOrder.BidPrice, size);
                    }
                }
                
                //save all changes at once
                await _unitOfWork.CommitAsync();
                return buyOrder;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBackAsync();
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Stock> UpdateStockPriceAsync(int stockId, double newPrice, int tradeSize)
        {
            var stock = await _stockRepository.GetStockByIdAsync(stockId);
            var updatedStock = new Stock();
            if (stock == null) return null;

            var currentPrice = stock.CurrentPrice + ((newPrice - stock.CurrentPrice) * tradeSize) / stock.InitialStock;
        
            if (currentPrice < stock.LowestPrice) //update lowest price and current price as newPrice
            {
                updatedStock =await _stockRepository.UpdateStockPriceAsync(stockId, currentPrice, stock.HighestPrice, currentPrice);
            }

            else if (newPrice > stock.HighestPrice) //update highest price and current price as newPrice
            {
                updatedStock = await _stockRepository.UpdateStockPriceAsync(stockId, currentPrice, currentPrice, stock.LowestPrice);
            }
            else{
                // update only current price
                updatedStock = await _stockRepository.UpdateStockPriceAsync(stockId, currentPrice, stock.HighestPrice, stock.LowestPrice);
            }

            await _unitOfWork.SaveChangesAsync();
            return updatedStock;
        }

        public async Task<SellOrder> OperateSellOrderAsync(int sellOrderId, int size)
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

        public async Task<IEnumerable<int>> GetLatestBuyOrderIds()
        {
            return await _buyOrderRepository.GetLatestBuyOrderIds();
        }
        public async Task<IEnumerable<Transaction>> GetTransactionsOfABuyOrder(int buyOrderId)
        {
            return await _transactionRepository.GetTransactionsOfABuyOrder(buyOrderId);
        }

        private async Task AddBuyOrderProcessByBuyOrder(BuyOrder buyOrder)
        {
            // if we are in working hours add buyOrderProcess
            if (buyOrder.Status != OrderStatus.NotYetActive)
            {
                await _buyOrderProcessRepository.AddBuyOrderProcess(new BuyOrderProcess(default, buyOrder.BuyOrderId,
                    buyOrder.Status, buyOrder.StockId, buyOrder.BidPrice));
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}