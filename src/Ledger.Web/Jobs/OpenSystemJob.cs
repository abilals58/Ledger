using System.Threading.Tasks;
using Ledger.Ledger.Web.Services;
using Quartz;

namespace Ledger.Ledger.Web.Jobs
{
    public class OpenSystemJob : IJob
    {
        private readonly ISellOrderService _sellOrderService;
        private readonly ISellOrderProcessService _sellOrderProcessService;

        public OpenSystemJob(ISellOrderService sellOrderService, ISellOrderProcessService sellOrderProcessService)
        {
            _sellOrderService = sellOrderService;
            _sellOrderProcessService = sellOrderProcessService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //add sellOrders and buyOrders created in out of working hours to sellOrderJobs and BuyOrderJobs tables 
            
            //change status of all SellOrders from notyetactive to active
            var sellOrders = await _sellOrderService.ChangeStatusActiveOnTheBeginningOfDay();
            //add all sellOrderProcesses
            foreach (var sellOrder in sellOrders)
            {
                // add new sellOrderProcess
                _sellOrderProcessService.AddSellOrderProcessBySellOrder(sellOrder); //TODO burayı incele

            }
        }
    }
}