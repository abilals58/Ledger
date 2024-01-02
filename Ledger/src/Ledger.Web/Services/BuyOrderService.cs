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
    }
    public class BuyOrderService :IBuyOrderService
    {
        private readonly IBuyOrderRepository _buyOrderRepository;
        private IUnitOfWork _unitOfWork;

        public BuyOrderService(IBuyOrderRepository buyOrderRepository, IUnitOfWork unitOfWork)
        {
            _buyOrderRepository = buyOrderRepository;
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
        
    }
}