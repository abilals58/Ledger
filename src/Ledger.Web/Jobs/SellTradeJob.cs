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
    public class SellTradeJob : IJob // SellTradeJob follows the SellOrders, find matched SellOrders and implements operations
    {
        private ISellOrderService _sellOrderService;
        private ISellOrderProcessService _sellOrderProcessService;
        public SellTradeJob(ISellOrderService sellOrderService, ISellOrderProcessService sellOrderProcessService)
        {
            _sellOrderService = sellOrderService;
            _sellOrderProcessService = sellOrderProcessService;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("SellTradeJob is executing.");
            //get sellOrderProcess according to FIFO order of ordernum
            var sellOrderProcess = await _sellOrderProcessService.GetFifoSellOrderProcess();
            if (sellOrderProcess != null) // if null, do nothing and try after 5 seconds 
            {
                var sellOrderId = sellOrderProcess.SellOrderId;
                var sellOrder = await _sellOrderService.GetSellOrderByIdAsync(sellOrderId);
            
                // match and operate logic 
                var endLoop = false;
                while (! endLoop)
                {
                    try
                    {
                        // get the first match  //TODO Fix MatchSellOrder and implement row level lock
                        var result = await _sellOrderService.MatchSellOrderProcessAsync(sellOrderProcess.SellOrderProcessId);
                        if (result == null)
                        {
                            endLoop = true;
                            //1 dk sonra tekrar dene
                            // increment the order num of sellOrderProcess
                            await _sellOrderProcessService.UpdateOrderNum(sellOrderProcess.SellOrderProcessId);
                        }
                        // status ==> isMatched handled in matchSellOrderProcessAsync
                        else
                        {
                            await _sellOrderService.OperateTradeAsync(sellOrderId);

                            if(sellOrder.CurrentAskSize == 0)
                            {
                                //status ==> completed and deleted handled in operateTradeAsync
                                endLoop = true;
                            }
                            //status ==> partially completed and active handled in operateTradeAsync
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
    }
}