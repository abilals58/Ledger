using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories
{

    public interface IDailyStockRepository
    {
        Task<IEnumerable<DailyStock>> GetAllDailyStocksAsync();
        Task<DailyStock> GetDailyStockByDateAsync(int id, DateTime date);
        Task<DailyStock> AddDailyStockAsync(DailyStock dailystock);
        Task<DailyStock> UpdateDailyStockAsync(int id, DateTime date, DailyStock newdailystock);
        Task<DailyStock> DeleteDailyStockAsync(int id, DateTime date);
        Task<IEnumerable<DailyStock>> GetDailyStocksOfAStock(int id);

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

        public async Task<DailyStock> GetDailyStockByDateAsync(int id, DateTime date)
        {
            return await _dbContext.DailyStocks.FindAsync(id, date);
        }

        public async Task<DailyStock> AddDailyStockAsync(DailyStock dailystock)
        {
            await _dbContext.DailyStocks.AddAsync(dailystock);
            await _dbContext.SaveChangesAsync();
            return dailystock;
        }
        
        public async Task<DailyStock> UpdateDailyStockAsync(int id, DateTime date, DailyStock newdailystock)
        {
            var dailystock = await  _dbContext.DailyStocks.FindAsync(id, date);
            if (dailystock == null) return null;
            
            dailystock.StockValue = newdailystock.StockValue;
            await _dbContext.SaveChangesAsync();
            return dailystock;
        }

        public async Task<DailyStock> DeleteDailyStockAsync(int id, DateTime date)
        {
            var dailystock = await  _dbContext.DailyStocks.FindAsync(id, date );
            if (dailystock == null) return null;

            _dbContext.DailyStocks.Remove(dailystock);
            await _dbContext.SaveChangesAsync();
            return dailystock;
        }

        public async Task<IEnumerable<DailyStock>> GetDailyStocksOfAStock(int id)
        {
            return await _dbContext.DailyStocks.Where(ds => ds.StockId == id).ToListAsync();
        }
    }
}