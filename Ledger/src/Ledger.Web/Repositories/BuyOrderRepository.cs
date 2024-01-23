using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories
{
    public interface IBuyOrderRepository
    {
        Task<IEnumerable<BuyOrder>> GetAllBuyOrdersAsync();
        Task<BuyOrder> GetBuyOrderByIdAsync(int id);
        Task<BuyOrder> AddBuyOrderAsync(BuyOrder buyOrder);
        Task<BuyOrder> UpdateByOrderAsync(int id, BuyOrder newbuyOrder);
        Task<BuyOrder> DeleteBuyOrderAsync(int id);
        //Task<BuyOrder> OperateBuyOrderAsync(int id);
        Task UpdateBidSize(int id, int size); //decrement the bidSize by given size
        Task LogicalDelete(int id); // change the status of buyOrder false (deleted)

    }
    
    public class BuyOrderRepository : IBuyOrderRepository // BuyOrder service coresponds to data tier and handes database operations 
    {
        private readonly DbSet<BuyOrder> _dbBuyOrder;

        public BuyOrderRepository(IDbContext dbContext)
        {
            _dbBuyOrder = dbContext.BuyOrders;
        }

        public async Task<IEnumerable<BuyOrder>> GetAllBuyOrdersAsync() // returns al buyorders
        {
            return await _dbBuyOrder.ToListAsync();
        }

        public async Task<BuyOrder> GetBuyOrderByIdAsync(int id) // returns a buyorder by id
        {
            return await _dbBuyOrder.FindAsync(id);

        }

        public async Task<BuyOrder> AddBuyOrderAsync(BuyOrder buyOrder) // adds a buyorder to the database
        {
            await _dbBuyOrder.AddAsync(buyOrder);
            return buyOrder;
        }

        public async Task<BuyOrder> UpdateByOrderAsync(int id, BuyOrder newbuyOrder) // updates a buyorder an returns it, returns null if there is no match
        {
            var buyOrder = await _dbBuyOrder.FindAsync(id);
            if (buyOrder == null) return null;

            buyOrder.UserId = newbuyOrder.UserId;
            buyOrder.StockId = newbuyOrder.StockId;
            buyOrder.BidPrice = newbuyOrder.BidSize;
            buyOrder.BidSize = newbuyOrder.BidSize;
            buyOrder.StartDate = newbuyOrder.StartDate;
            return buyOrder;
        }

        public async Task<BuyOrder> DeleteBuyOrderAsync(int id) // deletes a buyorder and returns it, returns null if there is no match
        {
            var buyOrder = await _dbBuyOrder.FindAsync(id);
            if (buyOrder == null) return null;
            _dbBuyOrder.Remove(buyOrder);
            return buyOrder;
        }

        public async Task UpdateBidSize(int id, int size)
        {
            var buyOrder = await _dbBuyOrder.FindAsync(id);
            buyOrder.CurrentBidSize = buyOrder.CurrentBidSize - size;
        }
        
        public async Task LogicalDelete(int id) //changes the status to deleted (no)
        {
            var buyOrder = await _dbBuyOrder.FindAsync(id);
            buyOrder.Status = OrderStatus.CompletedAndDeleted;
        }
    }
}