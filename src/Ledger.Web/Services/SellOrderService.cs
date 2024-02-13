using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Ledger.Ledger.Web.Jobs;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Transaction = Ledger.Ledger.Web.Models.Transaction;

namespace Ledger.Ledger.Web.Services
{
    
    public interface ISellOrderService
    {
        Task<IEnumerable<SellOrder>> GetAllSellOrdersAsync();
        Task<SellOrder> GetSellOrderByIdAsync(int id);
        Task<SellOrder> AddSellOrderAsync(SellOrder sellOrder);
        Task<SellOrder> UpdateSellOrderAsync(int id, SellOrder newSellOrder);
        Task<SellOrder> DeleteSellOrderAsync(int id);
        Task<SellOrderProcess> MatchSellOrderProcessAsync(int sellOrderId); //returns the ending sellOrder with Matched List of BuyOrders
        Task<SellOrder> OperateTradeAsync(int sellOrderId); //do necessary changes in stocksOfUser, sellOrder, transaction, stock tables.
        Task<Stock> UpdateStockPriceAsync(int stockId, double newPrice, int tradeSize);
        Task<SellOrder> OperateSellOrderAsync(int sellOrderId, int size);
        Task<BuyOrder> OperateBuyOrderAsync(int buyOrderId, int size);
            
        //Task<IEnumerable<int>> GetLatestSellOrderIds();
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
        private readonly ISellOrderProcessRepository _sellOrderProcessRepository;
        private readonly IBuyOrderProcessRepository _buyOrderProcessRepository;
        private IUnitOfWork _unitOfWork;

        public SellOrderService(ISellOrderRepository sellOrderRepository, IBuyOrderRepository buyOrderRepository,IStocksOfUserRepository stocksOfUserRepository, ITransactionRepository transactionRepository,IUserRepository userRepository, ISellOrderMatchRepository sellOrderMatchRepository, IStockRepository stockRepository, ISellOrderProcessRepository sellOrderProcessRepository, IBuyOrderProcessRepository buyOrderProcessRepository, IUnitOfWork unitOfWork)
        {
            _sellOrderRepository = sellOrderRepository;
            _buyOrderRepository = buyOrderRepository;
            _stocksOfUserRepository = stocksOfUserRepository;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _sellOrderMatchRepository = sellOrderMatchRepository;
            _stockRepository = stockRepository;
            _sellOrderProcessRepository = sellOrderProcessRepository;
            _buyOrderProcessRepository = buyOrderProcessRepository;
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
            _unitOfWork.BeginTransaction();
            try
            {
                //add new sellOrder
                await _sellOrderRepository.AddSellOrderAsync(sellOrder);
                await _unitOfWork.SaveChangesAsync();
                // add sellOrderProcess
                await _sellOrderProcessRepository.AddSellOrderProcess(new SellOrderProcess(default, sellOrder.SellOrderId,
                    sellOrder.Status, sellOrder.StockId, sellOrder.AskPrice));
                await _unitOfWork.SaveChangesAsync();
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
        /*public async Task<SellOrder>  MatchSellOrderAsync(int sellOrderId) // for a sell order, it matches first buyOrder with same Price and id // then add matched buyOrder to the Matched List
        {
            // get the related sellOrderProcess
            var sellOrder = await _sellOrderRepository.GetSellOrderByIdAsync(sellOrderId);

            //get the first matched buyOrder
            var buyOrder = await _buyOrderRepository.GetMatchedBuyOrder(sellOrder);
            if (buyOrder == null)
            {
                return null;
            }
            Console.WriteLine(buyOrder.Status);
            //change status of sellOrder 
            sellOrder.Status = OrderStatus.IsMatched;
    
            // add new match record to the database
            await _sellOrderMatchRepository.AddSellOrderMatchAsync(sellOrderId, buyOrder.BuyOrderId);
            
            //persist all changes 
            await _unitOfWork.SaveChangesAsync();
            
            await _unitOfWork.CommitAsync();
            return sellOrder;
        }*/
        
        public async Task<SellOrderProcess>  MatchSellOrderProcessAsync(int sellOrderProcessId) // for a sell order process, it matches first buyOrder process with same Price and id // then add matched buyOrder to the Matched List
        {
            _unitOfWork.BeginTransaction();
            // get the related sellOrderProcess
            var sellOrderProcess = await _sellOrderProcessRepository.GetSellOrderProcessById(sellOrderProcessId);

            //get the first matched buyOrderProcess
            var buyOrderProcess = await _buyOrderProcessRepository.GetMatchedBuyOrderProcess(sellOrderProcess);
            if (buyOrderProcess == null)
            {
                return null;
            }
            Console.WriteLine(buyOrderProcess.Status);
            //change status of matched buyOrder -- buyOrderProcess is already changed
            await _buyOrderRepository.FindAndUpdateStatus(buyOrderProcess.BuyOrderId, OrderStatus.IsMatched);
            //change status of sellOrder process and sellOrder
            sellOrderProcess.Status = OrderStatus.IsMatched;
            await _sellOrderRepository.FindAndUpdateStatus(sellOrderProcess.SellOrderId, OrderStatus.IsMatched);

            // add new match record to the database
            await _sellOrderMatchRepository.AddSellOrderMatchAsync(sellOrderProcess.SellOrderId, buyOrderProcess.BuyOrderId);

            //persist all changes
            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitAsync();
            return sellOrderProcess;
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
                    if (buyOrder.Status == OrderStatus.IsMatched ) //if buyOrder is not deleted, do the operation
                    {
                        var size = new int();
                        // determine the size
                        if (sellOrder.CurrentAskSize < buyOrder.CurrentBidSize) //lowest one determines 
                        {
                            size = sellOrder.CurrentAskSize;
                            buyOrder.Status = OrderStatus.PartiallyCompletedAndActive;
                            await _buyOrderProcessRepository.FindAndUpdateStatus(buyOrderId,OrderStatus.PartiallyCompletedAndActive);
                        }
                        else
                        {
                            size = buyOrder.CurrentBidSize;
                            sellOrder.Status = OrderStatus.PartiallyCompletedAndActive; //partially completed
                            await _sellOrderProcessRepository.FindAndUpdateStatus(sellOrderId,OrderStatus.PartiallyCompletedAndActive);
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
                //delete from sellOrder Process table
                await _sellOrderProcessRepository.DeleteSellOrderProcessBySellOrderId(sellOrderId);
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
                //delete from buyOrderProcess table
                await _buyOrderProcessRepository.DeleteByOrderProcessByBuyOrderId(buyOrderId);
            }

            await _unitOfWork.SaveChangesAsync();
            return buyOrder;
        }
        
        /*public async Task<IEnumerable<int>> GetLatestSellOrderIds() // return the latest sellOrderIds (isMatched)
        {
            return await _sellOrderRepository.GetLatestSellOderIds();
        }*/
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