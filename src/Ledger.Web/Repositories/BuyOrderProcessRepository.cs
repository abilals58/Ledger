using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories;

public interface IBuyOrderProcessRepository
{
    Task AddBuyOrderProcess(BuyOrderProcess buyOrderProcess);
    Task<BuyOrderProcess> GetMatchedBuyOrderProcess(SellOrderProcess sellOrderProcess);
    Task<BuyOrderProcess> FindAndUpdateStatus(int buyOrderProcessId, OrderStatus newStatus);
    Task DeleteByOrderProcessByBuyOrderId(int buyOrderId);
}


public class BuyOrderProcessRepository :IBuyOrderProcessRepository
{
    private readonly DbSet<BuyOrderProcess> _dbBuyOrderProcess;

    public BuyOrderProcessRepository(IDbContext dbContext)
    {
        _dbBuyOrderProcess = dbContext.BuyOrderJobs;
    }

    public async Task AddBuyOrderProcess(BuyOrderProcess buyOrderProcess)
    {
        await _dbBuyOrderProcess.AddAsync(buyOrderProcess);
    }

    public async Task<BuyOrderProcess> GetMatchedBuyOrderProcess(SellOrderProcess sellOrderProcess)
    {
        var buyOrderProcess = await _dbBuyOrderProcess
            .Where(b =>
                (b.Status == OrderStatus.Active || b.Status == OrderStatus.PartiallyCompletedAndActive) &&
                b.StockId == sellOrderProcess.StockId && b.BidPrice == sellOrderProcess.AskPrice).OrderBy(b => b.BuyOrderId)
            .FirstOrDefaultAsync();
        if (buyOrderProcess == null)
        {
            return null;
        }
        //update the status of buyOrderProcess
        buyOrderProcess.Status = OrderStatus.IsMatched;
        return buyOrderProcess;
    }

    public async Task<BuyOrderProcess> FindAndUpdateStatus(int buyOrderProcessId, OrderStatus newStatus)
    {
        var buyOrderProcess = await _dbBuyOrderProcess.FindAsync(buyOrderProcessId);
        if (buyOrderProcess == null)
        {
            return null;
        }

        if (buyOrderProcess.Status != newStatus)
        {
            buyOrderProcess.Status = newStatus;
        }

        return buyOrderProcess;
    }

    public async Task DeleteByOrderProcessByBuyOrderId(int buyOrderId)
    {
        var buyOrderProcess = await _dbBuyOrderProcess.Where(b => b.BuyOrderId == buyOrderId).FirstOrDefaultAsync();
        _dbBuyOrderProcess.Remove(buyOrderProcess);
    }
}