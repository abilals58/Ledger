using System;
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
    public class TradeJob : IJob
    {
        //private IBuyOrderService _buyOrderService;
        private ISellOrderService _sellOrderService;
        public TradeJob(ISellOrderService sellOrderService)
        {
            //_buyOrderService = buyOrderService;
            _sellOrderService = sellOrderService;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                Console.WriteLine("TradeJob is executing.");
                //int sellOrderId = await _sellOrderService.GetLatestSellOrderId();
                //await _sellOrderService.MatchSellOrdersAsync(sellOrderId);
                //await _sellOrderService.OperateTradeAsync(sellOrderId);
                //await _buyOrderService.MatchBuyOrdersAsync(1);
                //await _buyOrderService.OperateTradeAsync(1);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}