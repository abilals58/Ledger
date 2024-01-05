using System.Threading.Tasks;
using Ledger.Ledger.Web.Services;
using Quartz;

namespace Ledger.Ledger.Web.Jobs
{
    public class MatchSellOrder : IJob
    {
        private readonly ISellOrderService _sellOrderService;

        public MatchSellOrder(ISellOrderService sellOrderService)
        {
            _sellOrderService = sellOrderService;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            //await _sellOrderService.MatchSellOrdersAsync();
            //await _sellOrderService.OperateTradeAsync(sellOrderId);
        }
    }
}