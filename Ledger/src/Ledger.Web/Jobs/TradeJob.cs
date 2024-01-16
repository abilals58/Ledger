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
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                /*var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger2;Username=postgres;Password=mysecretpassword;").Options;
                var dbContext = new ApiDbContext(options);
                var sellOrderRepository = new SellOrderRepository(dbContext);
                var buyOrderRepository = new BuyOrderRepository(dbContext);
                var stocksOfUserRepository = new StocksOfUserRepository(dbContext);
                var transactionRepository = new TransactionRepository(dbContext);
                var userRepository = new UserRepository(dbContext);
                var sellOrderMatchRepository = new SellOrderMatchRepository(dbContext);
                var stockRepository = new StockRepository(dbContext);
                var unitOfWorkRepository = new UnitOfWork.UnitOfWork(dbContext);
                
                var _sellOrderService = new SellOrderService(sellOrderRepository,buyOrderRepository,stocksOfUserRepository,transactionRepository,userRepository,sellOrderMatchRepository,stockRepository,unitOfWorkRepository);*/
                
                //ISellOrderService _sellOrderService = (ISellOrderService)context.JobDetail.JobDataMap["sellOrderService"];
                Console.WriteLine("TradeJob is executing.");
                //int sellOrderId = await _sellOrderService.GetLatestSellOrderId();
                //await _sellOrderService.MatchSellOrdersAsync(sellOrderId);
                //await _sellOrderService.OperateTradeAsync(sellOrderId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}