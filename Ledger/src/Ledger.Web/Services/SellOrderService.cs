using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;

namespace Ledger.Ledger.Web.Services
{
    
    public interface ISellOrderService
    {
        Task<IEnumerable<SellOrder>> GetAllSellOrdersAsync();
        Task<SellOrder> GetSellOrderByIdAsync(int id);
        Task AddSellOrderAsync(SellOrder sellOrder);
        Task<SellOrder> UpdateSellOrderAsync(int id, SellOrder newSellOrder);
        Task<SellOrder> DeleteSellOrderAsync(int id);
    }
    public class SellOrderService :ISellOrderService
    {
        private readonly ISellOrderRepository _sellOrderRepository;

        public SellOrderService(ISellOrderRepository sellOrderRepository)
        {
            _sellOrderRepository = sellOrderRepository;
        }
        public async Task<IEnumerable<SellOrder>> GetAllSellOrdersAsync()
        {
            return await _sellOrderRepository.GetAllSellOrdersAsync();
        }

        public async Task<SellOrder> GetSellOrderByIdAsync(int id)
        {
            return await _sellOrderRepository.GetSellOrderByIdAsync(id);
        }

        public async Task AddSellOrderAsync(SellOrder sellOrder)
        {
            await _sellOrderRepository.AddSellOrderAsync(sellOrder);
        }

        public async Task<SellOrder> UpdateSellOrderAsync(int id, SellOrder newSellOrder)
        {
            return await _sellOrderRepository.UpdateSellOrderAsync(id,newSellOrder);
        }

        public async Task<SellOrder> DeleteSellOrderAsync(int id)
        {
            return await _sellOrderRepository.DeleteSellOrderAsync(id);
        }
    }
}