using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories
{

    public interface IStocksOfUserRepository
    {
        Task<IEnumerable<StocksOfUser>> GetAllStocksOfUserAsync();
        Task<StocksOfUser> GetStocksOfUserByIdAsync(int id);
        Task AddStocksOfUserAsync(StocksOfUser stocksOfUser);
        Task<StocksOfUser> UpdateStocksOfUserAsync(int id, StocksOfUser newStocksOfUser);
        Task<StocksOfUser> DeleteStocksOfUserAsync(int id);
    }
    public class StocksOfUserRepository : IStocksOfUserRepository// StocksOfUser service corresponds to data access tier and handles database operations
    {
        private readonly IDbContext _dbContext;

        public StocksOfUserRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<StocksOfUser>> GetAllStocksOfUserAsync() // returns all stocksofusers
        {
            return await _dbContext.StocksOfUser.ToListAsync();
        }

        
        public async Task<StocksOfUser> GetStocksOfUserByIdAsync(int id) // returns a stocksofuser by id
        {
            return await _dbContext.StocksOfUser.FindAsync(id);
           
        }

        public async Task AddStocksOfUserAsync( StocksOfUser stocksOfUser) // add a stocksofuser
        {
            await _dbContext.StocksOfUser.AddAsync(stocksOfUser);
            await _dbContext.SaveChangesAsync();
        }

        
        public async Task<StocksOfUser> UpdateStocksOfUserAsync(int id, StocksOfUser newStocksOfUser) // update a stocksofuser and return it, returns null if there is no match
        {
            var stocksOfUser = await _dbContext.StocksOfUser.FindAsync(id);
            if (stocksOfUser == null) return null;
            stocksOfUser.UserId = newStocksOfUser.UserId;
            stocksOfUser.StockId = newStocksOfUser.StockId;
            stocksOfUser.NumOfStocks = newStocksOfUser.NumOfStocks;
            await _dbContext.SaveChangesAsync();
            return stocksOfUser;

        }
        
        public async Task<StocksOfUser> DeleteStocksOfUserAsync(int id) // delete a stocksofuser and return it, return null if there is no match
        {
            var stocksOfUser = await _dbContext.StocksOfUser.FindAsync(id);
            if (stocksOfUser == null) return null;
            _dbContext.StocksOfUser.Remove(stocksOfUser);
            await _dbContext.SaveChangesAsync();
            return stocksOfUser;
        }
    }
}