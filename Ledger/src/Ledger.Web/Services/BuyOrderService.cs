using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;

namespace Ledger.Ledger.Web.Services
{
    
    public interface IBuyOrderService
    {
        Task<IEnumerable<BuyOrder>> GetAllBuyOrdersAsync();
        Task<BuyOrder> GetBuyOrderByIdAsync(int id);
        Task AddBuyOrderAsync(BuyOrder buyOrder);
        Task<BuyOrder> UpdateByOrderAsync(int id, BuyOrder newbuyOrder);
        Task<BuyOrder> DeleteBuyOrderAsync(int id);
        
    }
    public class BuyOrderService :IBuyOrderService
    {
        private readonly IBuyOrderRepository _buyOrderRepository;

        public BuyOrderService(IBuyOrderRepository buyOrderRepository)
        {
            _buyOrderRepository = buyOrderRepository;
        }
        public async Task<IEnumerable<BuyOrder>> GetAllBuyOrdersAsync()
        {
            return await _buyOrderRepository.GetAllBuyOrdersAsync();
        }

        public async Task<BuyOrder> GetBuyOrderByIdAsync(int id)
        {
            return await _buyOrderRepository.GetBuyOrderByIdAsync(id);        }

        public async Task AddBuyOrderAsync(BuyOrder buyOrder)
        {
            await _buyOrderRepository.AddBuyOrderAsync(buyOrder);        }

        public async Task<BuyOrder> UpdateByOrderAsync(int id, BuyOrder newbuyOrder)
        {
            return await _buyOrderRepository.UpdateByOrderAsync(id, newbuyOrder);        }

        public async Task<BuyOrder> DeleteBuyOrderAsync(int id)
        {
            return await _buyOrderRepository.DeleteBuyOrderAsync(id);        }
    }
}