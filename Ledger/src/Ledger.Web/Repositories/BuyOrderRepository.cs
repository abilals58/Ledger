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
            buyOrder.DateCreated = newbuyOrder.DateCreated;
            buyOrder.StartDate = newbuyOrder.StartDate;
            buyOrder.EndDate = newbuyOrder.EndDate;
            return buyOrder;
        }

        public async Task<BuyOrder> DeleteBuyOrderAsync(int id) // deletes a buyorder and returns it, returns null if there is no match
        {
            var buyOrder = await _dbBuyOrder.FindAsync(id);
            if (buyOrder == null) return null;
            _dbBuyOrder.Remove(buyOrder);
            return buyOrder;
        }
        
        /*public async Task<BuyOrder> OperateBuyOrderAsync(int id)  bussiness layera taşınacak
        {
            //get related buyOrder object
            var buyOrder = await _dbBuyOrder.FindAsync(id);
            // change the stocksOfUser information accordingly
            var stocksOfUser = await _dbContext.StocksOfUser.FindAsync(buyOrder.UserId, buyOrder.StockId);
            stocksOfUser.NumOfStocks += buyOrder.BidSize;
            
            //change the user budget accordingly
            var user = await _dbContext.Users.FindAsync(buyOrder.UserId);
            user.Budget -= buyOrder.BidSize * buyOrder.BidPrice;
            
            //update the buyOrder status
            buyOrder.Status = false; //operation is done, logicaly deleted
            
            //saving the changes to database
            await _dbContext.SaveChangesAsync();

            return buyOrder;
        }*/
    }
}