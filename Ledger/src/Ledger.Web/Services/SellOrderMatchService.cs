using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;

namespace Ledger.Ledger.Web.Services
{
    public interface ISellOrderMatchService
    {
        Task<IEnumerable<SellOrderMatch>> GetAllSellOrdersAsync();
        Task<SellOrderMatch> GetASellOrderMatchAsync(int sellOrderId, int buyOrderId);
        Task<SellOrderMatch> AddSellOrderMatchAsync(int sellOrderId, int buyOrderId);
        Task<IEnumerable<int>> GetMatchedBuyOrders(int sellOrderId);
    }
    
    
    public class SellOrderMatchService :ISellOrderMatchService
    {
        private readonly ISellOrderMatchRepository _sellOrderMatchRepository;

        public SellOrderMatchService(ISellOrderMatchRepository sellOrderMatchRepository)
        {
            _sellOrderMatchRepository = sellOrderMatchRepository;
        }
        
        public async Task<IEnumerable<SellOrderMatch>> GetAllSellOrdersAsync()
        {
            return await _sellOrderMatchRepository.GetAllSellOrdersAsync();
        }

        public async Task<SellOrderMatch> GetASellOrderMatchAsync(int sellOrderId, int buyOrderId)
        {
            return await _sellOrderMatchRepository.GetASellOrderMatchAsync(sellOrderId, buyOrderId);
        }

        public async Task<SellOrderMatch> AddSellOrderMatchAsync(int sellOrderId, int buyOrderId)
        {
            return await _sellOrderMatchRepository.AddSellOrderMatchAsync(sellOrderId, buyOrderId);
        }

        public async Task<IEnumerable<int>> GetMatchedBuyOrders(int sellOrderId)
        {
            return await _sellOrderMatchRepository.GetMatchedBuyOrders(sellOrderId);
        }
    }
}