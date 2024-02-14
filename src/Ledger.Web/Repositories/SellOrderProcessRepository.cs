using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Jobs;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories;

public interface ISellOrderProcessRepository
{
    Task AddSellOrderProcess(SellOrderProcess sellOrderProcess);
    Task<SellOrderProcess> GetSellOrderProcessById(int sellOrderProcessId);
    Task<SellOrderProcess> GetFifoSellOrder();
    Task<SellOrderProcess> FindAndUpdateStatus(int sellOrderProcessId, OrderStatus newStatus);

    Task<SellOrderProcess> UpdateOrderNum(int sellOrderProcessId);

    Task DeleteSellOrderProcessBySellOrderId(int sellOrderId);
}

public class SellOrderProcessRepository : ISellOrderProcessRepository
{
    private readonly DbSet<SellOrderProcess> _dbSellOrderProcess;

    public SellOrderProcessRepository(IDbContext dbContext)
    {
        _dbSellOrderProcess = dbContext.SellOrderJobs;
    }

    public async Task AddSellOrderProcess(SellOrderProcess sellOrderProcess)
    {
        await _dbSellOrderProcess.AddAsync(sellOrderProcess);
    }

    public async Task<SellOrderProcess> GetSellOrderProcessById(int sellOrderProcessId)
    {
        return await _dbSellOrderProcess.Where(s => s.SellOrderProcessId == sellOrderProcessId).FirstOrDefaultAsync();
    }

    public async Task<SellOrderProcess> GetFifoSellOrder()
    {
        //return the ordernum of the sellOrderProcess according to FIFO order (get the sellOrderProcess which has the lowest orderNum)
        return await _dbSellOrderProcess
            .Where(s => s.Status == OrderStatus.Active || s.Status == OrderStatus.PartiallyCompletedAndActive)
            .OrderBy(s=>s.OrderNum).FirstOrDefaultAsync();
    }

    public async Task<SellOrderProcess> FindAndUpdateStatus(int sellOrderProcessId, OrderStatus newStatus)
    {
        var sellOrderProcess = await _dbSellOrderProcess.FindAsync(sellOrderProcessId);
        if (sellOrderProcess == null)
        {
            return null;
        }

        if (sellOrderProcess.Status != newStatus)
        {
            sellOrderProcess.Status = newStatus;
        }

        return sellOrderProcess;
    }

    public async Task<SellOrderProcess> UpdateOrderNum(int sellOrderProcessId)
    {
        var sellOrderProcess = await _dbSellOrderProcess.FindAsync(sellOrderProcessId);
        if (sellOrderProcess == null)
        {
            return null;
        }

        sellOrderProcess.OrderNum += 10;
        return sellOrderProcess;
    }

    public async Task DeleteSellOrderProcessBySellOrderId(int sellOrderId)
    {
        var sellOrderProcess = await _dbSellOrderProcess.Where(s => s.SellOrderId == sellOrderId).FirstOrDefaultAsync();
        _dbSellOrderProcess.Remove(sellOrderProcess);
    }
}