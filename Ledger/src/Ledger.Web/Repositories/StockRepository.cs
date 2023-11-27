using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories
{
    public interface IStockRepository
    {
        Task<IEnumerable<Stock>> GetAllStocksAsync();
        Task<Stock> GetStockByIdAsync(int id);
        Task<Stock> AddStockAsync(Stock stock);
        Task<Stock> UpdateStockAsync(int id, Stock newStock);
        Task<Stock> DeleteStockAsync(int id);
    }
    public class StockRepository : IStockRepository
    {
        private readonly IStockContext _dbContext;

        public StockRepository(IStockContext dbContext) // Stock service corresponds to data access tier and handles database operations
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<Stock>> GetAllStocksAsync() // returns all stocks
        {
            return await _dbContext.Stocks.ToListAsync();
        }
        
        public async Task<Stock> GetStockByIdAsync(int id) // returns a stock by id
        {
            return await _dbContext.Stocks.FindAsync(id);
            
        }
        
        public async Task<Stock> AddStockAsync(Stock stock)  // adds a stock to the database
        {
            await _dbContext.Stocks.AddAsync(stock);
            await _dbContext.SaveChangesAsync();
            return stock;
        }
        
        public async Task<Stock> UpdateStockAsync(int id, Stock newStock) // updates the stock and returns that stock, returns null if there is no match 
        {
            var stock = await _dbContext.Stocks.FindAsync(id);
            if (stock == null) return null;
            stock.StockName = newStock.StockName;
            stock.OpenDate = newStock.OpenDate;
            stock.InitialStock = newStock.InitialStock;
            stock.InitialPrice = newStock.InitialPrice;
            stock.CurrentStock = newStock.CurrentStock;
            stock.CurrentPrice = newStock.CurrentPrice;
            await _dbContext.SaveChangesAsync();
            return stock;
        }
        
        public async Task<Stock> DeleteStockAsync(int id) // deletes a stock and returns that stock, returns null if there is no match
        {
            var stock = await _dbContext.Stocks.FindAsync(id);
            if (stock == null) return null;
            _dbContext.Stocks.Remove(stock);
            await _dbContext.SaveChangesAsync();
            return stock;
        }
        
    }
}