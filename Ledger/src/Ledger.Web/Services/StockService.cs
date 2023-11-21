using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;

namespace Ledger.Ledger.Web.Services
{
    
    public interface IStockService
    {
        Task<IEnumerable<Stock>> GetAllStocksAsync();
        Task<Stock> GetStockByIdAsync(int id);
        Task AddStockAsync(Stock stock);
        Task<Stock> UpdateStockAsync(int id, Stock newStock);
        Task<Stock> DeleteStockAsync(int id);
    }
    public class StockService :IStockService
    {
        private readonly IStockRepository _stockRepository;

        public StockService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }
        public async Task<IEnumerable<Stock>> GetAllStocksAsync()
        {
            return await _stockRepository.GetAllStocksAsync();
        }

        public async Task<Stock> GetStockByIdAsync(int id)
        {
            return await _stockRepository.GetStockByIdAsync(id);
        }

        public async Task AddStockAsync(Stock stock)
        {
            await _stockRepository.AddStockAsync(stock);        
        }

        public async Task<Stock> UpdateStockAsync(int id, Stock newStock)
        {
            return await _stockRepository.UpdateStockAsync(id, newStock);        
        }

        public async Task<Stock> DeleteStockAsync(int id)
        {
            return await _stockRepository.DeleteStockAsync(id);        
        }
    }
}