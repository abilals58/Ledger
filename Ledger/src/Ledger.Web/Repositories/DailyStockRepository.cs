using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories
{

    public interface IDailyStockRepository
    {
        Task<IEnumerable<DailyStock>> GetAllDailyStocksAsync();
        Task<DailyStock> GetDailyStockByDateAsync(DateTime date);
        Task<DailyStock> AddDailyStockAsync(DailyStock dailystock);
        Task<DailyStock> AddDailyStockAsync(DateTime date, Double price);
        Task<DailyStock> UpdateDailyStockAsync(DateTime date, DailyStock newdailystock);
        Task<DailyStock> DeleteDailyStockAsync(DateTime date);
        
    }
    
    public class DailyStockRepository : IDailyStockRepository
    {

        private readonly IDbContext _dbContext;
        public DailyStockRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<DailyStock>> GetAllDailyStocksAsync()
        {
            return await _dbContext.DailyStocks.ToListAsync();
        }

        public async Task<DailyStock> GetDailyStockByDateAsync(DateTime date)
        {
            return await _dbContext.DailyStocks.FindAsync(date);
        }

        public async Task<DailyStock> AddDailyStockAsync(DailyStock dailystock)
        {
            await _dbContext.DailyStocks.AddAsync(dailystock);
            await _dbContext.SaveChangesAsync();
            return dailystock;
        }

        public async Task<DailyStock> AddDailyStockAsync(DateTime date, double price)
        {
            var dailystock = new DailyStock(date, price);
            await _dbContext.DailyStocks.AddAsync(dailystock);
            await _dbContext.SaveChangesAsync();
            return dailystock;
        }

        public async Task<DailyStock> UpdateDailyStockAsync(DateTime date, DailyStock newdailystock)
        {
            var dailystock = await  _dbContext.DailyStocks.FindAsync(date);
            if (dailystock == null) return null;
            
            dailystock.StockValue = newdailystock.StockValue;
            await _dbContext.SaveChangesAsync();
            return dailystock;
        }

        public async Task<DailyStock> DeleteDailyStockAsync(DateTime date)
        {
            var dailystock = await  _dbContext.DailyStocks.FindAsync(date);
            if (dailystock == null) return null;

            _dbContext.DailyStocks.Remove(dailystock);
            await _dbContext.SaveChangesAsync();
            return dailystock;
        }
    }
}