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
            sellOrder.DateCreated = newSellOrder.DateCreated;
            sellOrder.StartDate = newSellOrder.StartDate;
            sellOrder.EndDate = newSellOrder.EndDate;
            return sellOrder;
        }
        
        public async Task<SellOrder> DeleteSellOrderAsync(int id) // deletes a sellorder and returns it, returns null if there is no match
        {
            var sellOrder = await _dbSellOrder.FindAsync(id);
            if (sellOrder == null) return null;
            _dbSellOrder.Remove(sellOrder);
            return sellOrder;
        }

        /*public async Task<IEnumerable<SellOrder>> MatchSellOrdersAsync(int buyorderId)
        {
            //find buyorder by given id
            var buyorder = await _dbContext.BuyOrders.FindAsync(buyorderId);
            if (buyorder == null)
            {
                return null;
            }
            var stockid = buyorder.StockId;
            var price = buyorder.BidPrice;
            return await _dbContext.SellOrders.Where(s => s.StockId == stockid && s.AskPrice == price).ToListAsync();
        }

        public async Task<SellOrder> OperateSellOrderAsync(int id)
        {
            //get related sellOrder object
            var sellOrder = await _dbContext.SellOrders.FindAsync(id);
            // change the stocksOfUser information accordingly
            var stocksOfUser = await _dbContext.StocksOfUser.FindAsync(sellOrder.UserId, sellOrder.StockId);
            stocksOfUser.NumOfStocks -= sellOrder.AskSize;
            
            //change the user budget accordingly
            var user = await _dbContext.Users.FindAsync(sellOrder.UserId);
            user.Budget += sellOrder.AskSize * sellOrder.AskPrice;
            
            //update the sellOrder status
            sellOrder.Status = false; //operation is done, logicaly deleted
            
            //saving the changes to database
            await _dbContext.SaveChangesAsync();

            return sellOrder;
        }*/
    }
}