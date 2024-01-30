using System;
using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Services;
using Quartz;

namespace Ledger.Ledger.Web.Jobs;

public class BuyTradeJob : IJob
{
    
    private IBuyOrderService _buyOrderService;
    public BuyTradeJob(IBuyOrderService buyOrderService)
    {
        _buyOrderService = buyOrderService;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            Console.WriteLine("BuyTradeJob is executing.");
            var buyOrders = await _buyOrderService.GetLatestBuyOrderIds();
            if (buyOrders.Count()!= 0)
            {
                foreach (var buyOrder in buyOrders)
                {
                    await _buyOrderService.OperateTradeAsync(buyOrder);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}