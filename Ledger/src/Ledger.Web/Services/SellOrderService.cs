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
        //Task<List<SellOrder>> MatchSellOrdersAsync(int buyOrderId, int bidSize); //returns a list of matched sellOrders
        //Task<SellOrder> OperateSellOrder(int id);

    }
    public class SellOrderService :ISellOrderService
    {
        private readonly ISellOrderRepository _sellOrderRepository;
        private IUnitOfWork _unitOfWork;

        public SellOrderService(ISellOrderRepository sellOrderRepository, IUnitOfWork unitOfWork)
        {
            _sellOrderRepository = sellOrderRepository;
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
        }

        public async Task<SellOrder> OperateSellOrder(int id)
        {
            return await _sellOrderRepository.OperateSellOrderAsync(id);
        }*/
    }
}