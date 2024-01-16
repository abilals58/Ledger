using System;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Services;
using Microsoft.CodeAnalysis.CSharp;
using Quartz;
using Quartz.Logging;

namespace Ledger.Ledger.Web.Jobs
{
    public class TradeJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                ISellOrderService _sellOrderService = (ISellOrderService)context.JobDetail.JobDataMap["sellOrderService"];
                await Console.Out.WriteLineAsync("TradeJob is executing.");
                int sellOrderId = await _sellOrderService.GetLatestSellOrderId();
                await _sellOrderService.MatchSellOrdersAsync(sellOrderId);
                await _sellOrderService.OperateTradeAsync(sellOrderId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}