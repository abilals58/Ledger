using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.UnitOfWork;

namespace Ledger.Ledger.Web.Services
{
    public interface IBuyOrderService
    {
        Task<IEnumerable<BuyOrder>> GetAllBuyOrdersAsync();
        Task<BuyOrder> GetBuyOrderByIdAsync(int id);
        Task<BuyOrder> AddBuyOrderAsync(BuyOrder buyOrder);
        Task<BuyOrder> UpdateByOrderAsync(int id, BuyOrder newbuyOrder);
        Task<BuyOrder> DeleteBuyOrderAsync(int id);
        Task<BuyOrder> MatchBuyOrdersAsync(int buyOrderId);
        Task<BuyOrder> OperateTradeAsync(int buyOrderId);
        Task<Stock> UpdateStockPriceAsync(int stockId, double newPrice, int tradeSize);
        Task<SellOrder> OperateSellOrderAsync(int sellOrderId, int size);
        Task<BuyOrder> OperateBuyOrderAsync(int buyOrderId, int size);

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
        private IUnitOfWork _unitOfWork;

        public BuyOrderService(IBuyOrderRepository buyOrderRepository, ISellOrderRepository sellOrderRepository,IStocksOfUserRepository stocksOfUserRepository, ITransactionRepository transactionRepository,IUserRepository userRepository, IBuyOrderMatchRepository buyOrderMatchRepository, IStockRepository stockRepository, IUnitOfWork unitOfWork)
        {
            _buyOrderRepository = buyOrderRepository;
            _sellOrderRepository = sellOrderRepository;
            _stocksOfUserRepository = stocksOfUserRepository;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _buyOrderMatchRepository = buyOrderMatchRepository;
            _stockRepository = stockRepository;
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
            try
            {
                await _buyOrderRepository.AddBuyOrderAsync(buyOrder);
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
            try
            {
                var buyOrder = await _buyOrderRepository.UpdateByOrderAsync(id, newbuyOrder);
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

        public async Task<BuyOrder> DeleteBuyOrderAsync(int id)
        {
            try
            {
                var buyOrder = await _buyOrderRepository.DeleteBuyOrderAsync(id);
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

        public async Task<BuyOrder> MatchBuyOrdersAsync(int buyOrderId)
        {
            try
            {
                //get related buyOrder
                var buyOrder = await _buyOrderRepository.GetBuyOrderByIdAsync(buyOrderId);
                
                //get all sellOrders and search for a match
                var sellOrders = await _sellOrderRepository.GetAllSellOrdersAsync();
                var totalSize = 0;
                foreach (var sellOrder in sellOrders)
                {
                    if ((sellOrder.Status == OrderStatus.Active || sellOrder.Status == OrderStatus.PartiallyCompletedAndActive) && buyOrder.StockId == sellOrder.StockId && buyOrder.BidPrice == sellOrder.AskPrice)
                    {
                        //add new match record to the buyOrder matches
                        await _buyOrderMatchRepository.AddBuyOrderMatchAsync(buyOrderId, sellOrder.SellOrderId);
                        totalSize = totalSize + sellOrder.CurrentAskSize;
                        if (totalSize >= buyOrder.CurrentBidSize)
                        {
                            break;
                        }
                    }
                }
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

        public async Task<BuyOrder> OperateTradeAsync(int buyOrderId)
        {
            try
            {
                // get related buyOrder
                var buyOrder = await _buyOrderRepository.GetBuyOrderByIdAsync(buyOrderId);
                
                //get BuyOrderMatch list --> list of ids of matched sellOrders
                var sellOrders = await _buyOrderMatchRepository.GetMatchedSellOrders(buyOrderId);

                foreach (var sellOrderId in sellOrders)
                {
                    var sellOrder = await _sellOrderRepository.GetSellOrderByIdAsync(sellOrderId);
                    if (sellOrder.Status == OrderStatus.Active || sellOrder.Status == OrderStatus.PartiallyCompletedAndActive) //if sellOrder is not deleted, do the operation
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
                        
                        //operate the trade between matched buyOrder and sellOrder
                        //update sellOrder and buyOrder related information 
                        await this.OperateBuyOrderAsync(buyOrderId, size);
                        await this.OperateSellOrderAsync(sellOrderId, size);
                        
                        //create new transaction
                        await _transactionRepository.AddTransactionAsync(new Transaction(default, sellOrderId,
                            buyOrderId, sellOrder.UserId, buyOrder.UserId, buyOrder.StockId, size, buyOrder.BidPrice));
                        
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
            return buyOrder;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionsOfABuyOrder(int buyOrderId)
        {
            return await _transactionRepository.GetTransactionsOfABuyOrder(buyOrderId);
        }
    }
}