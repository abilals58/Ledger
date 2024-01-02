using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.Services;
using Ledger.Ledger.Web.UnitOfWork;
using Quartz;

namespace Ledger.Ledger.Web.Jobs
{
    public class CloseSystemJob : IJob
    {
        private readonly IDailyStockService _dailyStockService;

        public CloseSystemJob(IDailyStockService dailyStockService)
        {
            _dailyStockService = dailyStockService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            //record the ending values of all stocks
            await _dailyStockService.RecordAllDailyStocksAsync();
        }
    }
}