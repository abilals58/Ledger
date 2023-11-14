using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Services
{
    public class SellOrderService // SellOrder service corresponds to data tier and it handles database operations
    {
        private ApiDbContext _dbContext;

        public SellOrderService(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<SellOrder>> GetAllSellOrdersAsync() // returns all sellorders
        {
            return await _dbContext.SellOrders.ToListAsync();
        }
        
        public async Task<SellOrder> GetSellOrderByIdAsync(int id) // returns a sellorder by id
        {
            return await _dbContext.SellOrders.FindAsync(id);
            
        }
        
        public async Task AddSellOrderAsync(SellOrder sellOrder) // adds a sellorder to the database
        {
            await _dbContext.AddAsync(sellOrder);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task<SellOrder> UpdateSellOrderAsync(int id, SellOrder newSellOrder) // updates a sellorder and returns it, returns null if there is no match
        {
            var sellOrder = await _dbContext.SellOrders.FindAsync(id);
            if (sellOrder == null) return null;
            sellOrder.UserId = newSellOrder.UserId;
            sellOrder.StockId = newSellOrder.StockId;
            sellOrder.AskPrice = newSellOrder.AskPrice;
            sellOrder.AskSize = newSellOrder.AskSize;
            sellOrder.DateCreated = newSellOrder.DateCreated;
            await _dbContext.SaveChangesAsync();
            return sellOrder;
        }
        
        public async Task<SellOrder> DeleteSellOrderAsync(int id) // deletes a sellorder and returns it, returns null if there is no match
        {
            var sellOrder = await _dbContext.SellOrders.FindAsync(id);
            if (sellOrder == null) return null;
            _dbContext.SellOrders.Remove(sellOrder);
            await _dbContext.SaveChangesAsync();
            return sellOrder;
        }
        
    }
}