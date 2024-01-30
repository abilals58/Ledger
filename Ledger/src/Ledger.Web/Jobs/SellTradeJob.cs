using System;
using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.Services;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Logging;

namespace Ledger.Ledger.Web.Jobs
{
    public class SellTradeJob : IJob
    {
        //private IBuyOrderService _buyOrderService;
        private ISellOrderService _sellOrderService;
        public SellTradeJob(ISellOrderService sellOrderService)
        {
            //_buyOrderService = buyOrderService;
            _sellOrderService = sellOrderService;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("SellTradeJob is executing.");
                var sellOrders = await _sellOrderService.GetLatestSellOrderIds();
                if (sellOrders.Count()!= 0)
                {
                    foreach (var sellOrder in sellOrders)
                    {
                        await _sellOrderService.OperateTradeAsync(sellOrder);
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
}