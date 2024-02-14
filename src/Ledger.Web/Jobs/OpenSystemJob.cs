using System.Threading.Tasks;
using Quartz;

namespace Ledger.Ledger.Web.Jobs
{
    public class OpenSystemJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            //add sellOrders and buyOrders created in out of working hours to sellOrderJobs and BuyOrderJobs tables 
            // handle these operations
            throw new System.NotImplementedException();
        }
    }
}