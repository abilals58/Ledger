using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Services
{
    public class BuyOrderService  // BuyOrder service coresponds to data tier and handes database operations 

    {
    private ApiDbContext _dbContext;

    public BuyOrderService(ApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<BuyOrder>> GetAllBuyOrdersAsync() // returns al buyorders
    {
        return await _dbContext.BuyOrders.ToListAsync();
    }

    public async Task<BuyOrder> GetBuyOrderByIdAsync(int id) // returns a buyorder by id
    {
        return await _dbContext.BuyOrders.FindAsync(id);

    }

    public async Task AddBuyOrderAsync(BuyOrder buyOrder) // adds a buyorder to the database
    {
        await _dbContext.BuyOrders.AddAsync(buyOrder);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<BuyOrder>
        UpdateByOrderAsync(int id,
            BuyOrder newbuyOrder) // updates a buyorder an returns it, returns null if there is no match
    {
        var buyOrder = await _dbContext.BuyOrders.FindAsync(id);
        if (buyOrder == null) return null;

        buyOrder.UserId = newbuyOrder.UserId;
        buyOrder.StockId = newbuyOrder.StockId;
        buyOrder.BidPrice = newbuyOrder.BidSize;
        buyOrder.BidSize = newbuyOrder.BidSize;
        buyOrder.DateCreated = newbuyOrder.DateCreated;
        await _dbContext.SaveChangesAsync();
        return buyOrder;
    }

    public async Task<BuyOrder>
        DeleteBuyOrderAsync(int id) // deletes a buyorder and returns it, returns null if there is no match
    {
        var buyOrder = await _dbContext.BuyOrders.FindAsync(id);
        if (buyOrder == null) return null;
        _dbContext.BuyOrders.Remove(buyOrder);
        await _dbContext.SaveChangesAsync();
        return buyOrder;
    }
    }
}