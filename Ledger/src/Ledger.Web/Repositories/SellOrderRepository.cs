using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories
{
    public interface ISellOrderRepository
    {
        Task<IEnumerable<SellOrder>> GetAllSellOrdersAsync();
        Task<SellOrder> GetSellOrderByIdAsync(int id);
        Task<SellOrder> AddSellOrderAsync(SellOrder sellOrder);
        Task<SellOrder> UpdateSellOrderAsync(int id, SellOrder newSellOrder);
        Task<SellOrder> DeleteSellOrderAsync(int id);
        Task UpdateAskSizeAsync(int id, int size);

        Task LogicalDelete(int id);
        //Task<IEnumerable<SellOrder>> MatchSellOrdersAsync(int buyorderId);
        //Task<SellOrder> OperateSellOrderAsync(int id);
    }
    public class SellOrderRepository : ISellOrderRepository // SellOrder service corresponds to data tier and it handles database operations
    {
        private readonly DbSet<SellOrder> _dbSellOrder;

        public SellOrderRepository(IDbContext dbContext)
        {
            _dbSellOrder = dbContext.SellOrders;
        }
        
        public async Task<IEnumerable<SellOrder>> GetAllSellOrdersAsync() // returns all sellorders
        {
            return await _dbSellOrder.ToListAsync();
        }
        
        public async Task<SellOrder> GetSellOrderByIdAsync(int id) // returns a sellorder by id
        {
            return await _dbSellOrder.FindAsync(id);
            
        }
        
        public async Task<SellOrder> AddSellOrderAsync(SellOrder sellOrder) // adds a sellorder to the database
        {
            await _dbSellOrder.AddAsync(sellOrder);
            return sellOrder;
        }
        
        public async Task<SellOrder> UpdateSellOrderAsync(int id, SellOrder newSellOrder) // updates a sellorder and returns it, returns null if there is no match
        {
            var sellOrder = await _dbSellOrder.FindAsync(id);
            if (sellOrder == null) return null;
            sellOrder.UserId = newSellOrder.UserId;
            sellOrder.StockId = newSellOrder.StockId;
            sellOrder.AskPrice = newSellOrder.AskPrice;
            sellOrder.AskSize = newSellOrder.AskSize;
            sellOrder.StartDate = newSellOrder.StartDate;
            return sellOrder;
        }
        
        public async Task<SellOrder> DeleteSellOrderAsync(int id) // deletes a sellorder and returns it, returns null if there is no match
        {
            var sellOrder = await _dbSellOrder.FindAsync(id);
            if (sellOrder == null) return null;
            _dbSellOrder.Remove(sellOrder);
            return sellOrder;
        }

        public async Task UpdateAskSizeAsync(int id, int size) //decrements the askSize by given size
        {
            var sellOrder = await _dbSellOrder.FindAsync(id);
            sellOrder.CurrentAskSize = sellOrder.CurrentAskSize - size;
        }

        public async Task LogicalDelete(int id) //changes the status to deleted (no)
        {
            var sellOrder = await _dbSellOrder.FindAsync(id);
            sellOrder.Status = OrderStatus.CompletedAndDeleted;
        }
    }
}