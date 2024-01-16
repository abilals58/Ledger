using System.Threading.Tasks;
using Ledger.Ledger.Web.Jobs;
using Ledger.Ledger.Web.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;

namespace Ledger.Ledger.Web
{
    public class Program
    {
        private static ISellOrderService _sellOrderService;

        public Program(ISellOrderService sellOrderService)
        {
            _sellOrderService = sellOrderService;
        }
        public static async Task Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices((cxt, services) =>
                {
                    services.AddQuartz(q =>
                    {
                        q.UseMicrosoftDependencyInjectionJobFactory();
                    });
                    services.AddQuartzHostedService(opt =>
                    {
                        opt.WaitForJobsToComplete = true;
                    });
                    
                    /*var schedulerFactory = services.BuildServiceProvider().GetRequiredService<ISchedulerFactory>();
                    var scheduler = schedulerFactory.GetScheduler().Result;

                    IJobDetail tradeJob = JobBuilder.Create<TradeJob>()
                        .WithIdentity("tradeJob", "group1")
                        .Build();
                    tradeJob.JobDataMap["sellOrderService"] = _sellOrderService;

                    var tradeTrigger = TriggerBuilder.Create()
                        .WithIdentity("tradeTrigger", "group1")
                        .StartNow()
                        .WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever())
                        .Build();
                    scheduler.ScheduleJob(tradeJob, tradeTrigger);*/
                }).Build();
            //await builder.RunAsync();

            var schedulerFactory = builder.Services.GetRequiredService<ISchedulerFactory>();
            var scheduler = await schedulerFactory.GetScheduler();

            IJobDetail tradeJob = JobBuilder.Create<TradeJob>()
                .WithIdentity("tradeJob", "group1")
                .Build();
            tradeJob.JobDataMap["sellOrderService"] = _sellOrderService;

            var tradeTrigger = TriggerBuilder.Create()
                .WithIdentity("tradeTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever())
                .Build();

            await scheduler.ScheduleJob(tradeJob, tradeTrigger);
            await builder.RunAsync();
        }
        
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        
    }
}