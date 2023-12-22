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

        //Task<Stock> UpdateStockPriceAsync(int id, double currentPrice, double highestPrice, double lowestPrice);
        Task<Stock> UpdateStockPriceAsync(int id, double newPrice);

    }
    public class StockRepository : IStockRepository
    {
        private readonly IDbContext _dbContext;

        public StockRepository(IDbContext dbContext) // Stock service corresponds to data access tier and handles database operations
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
            stock.HighestPrice = newStock.HighestPrice;
            stock.LowestPrice = newStock.LowestPrice;
            stock.Status = newStock.Status;
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
        /*public async Task<Stock> UpdateStockPriceAsync(int id, double currentPrice, double highestPrice, double lowestPrice) // updates the stock and returns that stock, returns null if there is no match 
        {
            var stock = await _dbContext.Stocks.FindAsync(id);
            if (stock == null) return null;
            
            stock.CurrentPrice = currentPrice;
            stock.HighestPrice = highestPrice;
            stock.LowestPrice = lowestPrice;
            await _dbContext.SaveChangesAsync();
            return stock;
        }*/
        
        public async Task<Stock> UpdateStockPriceAsync(int id, double newPrice) // updates the stock and returns that stock, returns null if there is no match 
        {
            var stock = await _dbContext.Stocks.FindAsync(id);
            if (stock == null) return null;

            if (newPrice < stock.LowestPrice) //update current and lowest price as newPrice
            {
                stock.CurrentPrice = newPrice;
                stock.HighestPrice = stock.HighestPrice;
                stock.LowestPrice = newPrice;
                await _dbContext.SaveChangesAsync();
                return stock;
                
            }
            if (newPrice > stock.HighestPrice) //update current and high price as newPrice
            {
                stock.CurrentPrice = newPrice;
                stock.HighestPrice = newPrice;
                stock.LowestPrice = stock.LowestPrice;
                await _dbContext.SaveChangesAsync();
                return stock;
                
            }
            //update only current price as newPrice
            stock.CurrentPrice = newPrice;
            stock.HighestPrice = stock.HighestPrice;
            stock.LowestPrice = stock.LowestPrice;
            await _dbContext.SaveChangesAsync();
            return stock;
        }
        
        
        
        
    }
}
