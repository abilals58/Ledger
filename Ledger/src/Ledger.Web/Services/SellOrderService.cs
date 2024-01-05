using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.UnitOfWork;

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
        Task<SellOrder> OperateSellOrderAsync(int sellOrderId, int size);
        Task<BuyOrder> OperateBuyOrderAsync(int buyOrderId, int size);

    }
    public class SellOrderService :ISellOrderService
    {
        private readonly ISellOrderRepository _sellOrderRepository;
        private readonly IBuyOrderRepository _buyOrderRepository;
        private readonly IStocksOfUserRepository _stocksOfUserRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;

        public SellOrderService(ISellOrderRepository sellOrderRepository, IBuyOrderRepository buyOrderRepository,IStocksOfUserRepository stocksOfUserRepository, ITransactionRepository transactionRepository,IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _sellOrderRepository = sellOrderRepository;
            _buyOrderRepository = buyOrderRepository;
            _stocksOfUserRepository = stocksOfUserRepository;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
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
            try
            {
                await _sellOrderRepository.AddSellOrderAsync(sellOrder);
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
            try
            {
                var sellOrder = await _sellOrderRepository.UpdateSellOrderAsync(id,newSellOrder);
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

        public async Task<SellOrder> DeleteSellOrderAsync(int id)
        {
            try
            {
                var sellOrder =  await _sellOrderRepository.DeleteSellOrderAsync(id);
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

        public async Task<SellOrder> MatchSellOrdersAsync(int sellOrderId)
        {
            try
            {
                // get the related sellOrder
                var sellOrder = await _sellOrderRepository.GetSellOrderByIdAsync(sellOrderId);

                //get all buyOrders and search for a match
                var buyOrders = await _buyOrderRepository.GetAllBuyOrdersAsync();
                var totalSize = 0;

                foreach (var buyOrder in buyOrders)
                {
                    //check for the stockId
                    if (sellOrder.StockId == buyOrder.StockId && sellOrder.AskPrice == buyOrder.BidPrice)
                    {
                        sellOrder.MatchList.Add(buyOrder);
                        totalSize = totalSize + buyOrder.BidSize;

                        if (totalSize >= sellOrder.AskSize)
                        {
                            break;
                        }
                    }
                }

                if (totalSize != 0)
                {
                    sellOrder.IsMatched = true; //partial match kısmını düşün !!!
                }
                await _unitOfWork.CommitAsync(); // save changes 
                return sellOrder;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBackAsync();
                Console.WriteLine(e);
                throw;
            }
           
        }

        public async Task<SellOrder> OperateTradeAsync(int sellOrderId) //operate trade, form transactions
        {
            try
            {
                //get related sellOrder
                var sellOrder = await _sellOrderRepository.GetSellOrderByIdAsync(sellOrderId);
                foreach (var buyOrder in sellOrder.MatchList)
                {
                    var size = new int();
                    // determine the size
                    if (sellOrder.AskSize < buyOrder.BidSize) //lowest one determines 
                    {
                        size = sellOrder.AskSize;
                    }
                    else
                    {
                        size = buyOrder.BidSize;
                    }
                    //operate the trade between matched sellOrder and buyOrder
                    sellOrder.AskSize -= size;
                
                    //update sellOrder related information
                    await this.OperateSellOrderAsync(sellOrderId, size);
                    await this.OperateBuyOrderAsync(buyOrder.BuyOrderId, size);
                
                    //create new transaction 
                    await _transactionRepository.AddTransactionAsync(new Transaction(default, sellOrder.UserId,
                        buyOrder.UserId, sellOrder.StockId, size, sellOrder.AskPrice, DateTime.Now));
                
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
            sellOrder.AskSize -= size;
        
            //if the AskSize is 0 logically delete
            if (sellOrder.AskSize == 0)
            {
                await _sellOrderRepository.LogicalDelete(sellOrderId);
            }
            return sellOrder;
           
        }

        public async Task<BuyOrder> OperateBuyOrderAsync(int buyOrderId, int size)
        {
            var buyOrder = await _buyOrderRepository.GetBuyOrderByIdAsync(buyOrderId);
            //update stocks of user
            await _stocksOfUserRepository.UpdateStockInfo(buyOrder.UserId, buyOrder.StockId, '+', size);
        
            //update budget 
            var amount = buyOrder.BidPrice * size;
            await _userRepository.UpdateBudget(buyOrder.UserId, '-', amount);
            
            //update bidSize in buyOrder
            buyOrder.BidSize -= size;
            await _buyOrderRepository.UpdateBidSize(buyOrderId, size);
            
            //if the BidSize is 0 logically delete
            if (buyOrder.BidSize == 0)
            {
                await _buyOrderRepository.LogicalDelete(buyOrderId);
            }
            return buyOrder;
        }


        //bussiness logic operations
        /*public async Task<List<SellOrder>> MatchSellOrdersAsync(int buyOrderId, int bidSize)
        {
            
            // return all matched sellOrders according to bidId and price
            var sellOrders = await _sellOrderRepository.MatchSellOrdersAsync(buyOrderId);

            var matchList = new List<SellOrder>();
            var totalSize = 0;
            foreach (var sellorder in sellOrders)
            {
                matchList.Add(sellorder);
                totalSize += sellorder.AskSize;

                if (totalSize <= bidSize)
                {
                    break;
                }
            }
            return matchList;
        }*/
    }
}