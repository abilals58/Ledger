using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;

namespace Ledger.Ledger.Web.Services
{
    public interface IDailyStockService
    {
        Task<IEnumerable<DailyStock>> GetAllDailyStocksAsync();
        Task<DailyStock> GetDailyStockByDateAsync(int id, DateTime date);
        Task<DailyStock> AddDailyStockAsync(DailyStock dailystock);
        Task<DailyStock> UpdateDailyStockAsync(int id,DateTime date, DailyStock newdailystock);
        Task<DailyStock> DeleteDailyStockAsync(int id,DateTime date);

        Task<IEnumerable<DailyStock>> GetDailyStocksOfAStock(int id);
    }
    
    public class DailyStockService : IDailyStockService
    {
        private readonly IDailyStockRepository _dailyStockRepository;

        public DailyStockService(IDailyStockRepository dailyStockRepository)
        {
            _dailyStockRepository = dailyStockRepository;
        }
        
        public async Task<IEnumerable<DailyStock>> GetAllDailyStocksAsync()
        {
            return await _dailyStockRepository.GetAllDailyStocksAsync();
        }

        public async Task<DailyStock> GetDailyStockByDateAsync(int id,DateTime date)
        {
            return await _dailyStockRepository.GetDailyStockByDateAsync(id,date);
        }

        public async Task<DailyStock> AddDailyStockAsync(DailyStock dailystock)
        {
            return await _dailyStockRepository.AddDailyStockAsync(dailystock);
        }
        
        public async Task<DailyStock> UpdateDailyStockAsync(int id, DateTime date, DailyStock newdailystock)
        {
            return await _dailyStockRepository.UpdateDailyStockAsync(id, date, newdailystock);
        }

        public async Task<DailyStock> DeleteDailyStockAsync(int id, DateTime date)
        {
            return await _dailyStockRepository.DeleteDailyStockAsync(id, date);
        }

        public async Task<IEnumerable<DailyStock>> GetDailyStocksOfAStock(int id)
        {
            return await _dailyStockRepository.GetDailyStocksOfAStock(id);
        }
    }
}