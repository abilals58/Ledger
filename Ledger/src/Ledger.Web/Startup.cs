using System;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Jobs;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.Services;
using Ledger.Ledger.Web.UnitOfWork;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Ledger.Ledger.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        [Obsolete("Obsolete")]
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddQuartz(q =>
            {
                // base Quartz scheduler, job and trigger configuration
                string sellJobKey = "sellTradeJob";
                string buyJobKey = "buyTradeJob";
                q.AddJob<SellTradeJob>(opts => opts.WithIdentity(sellJobKey));
                q.AddTrigger(opts => opts
                    .ForJob(sellJobKey) // link to the tradeJob
                    .WithIdentity("sellTradeJob-trigger") // give the trigger a unique name
                    .WithCronSchedule("0/10 * * * * ?")); // run every 10 seconds
                //.WithSimpleSchedule(x => x.WithRepeatCount(0)));
                q.AddJob<BuyTradeJob>(opts => opts.WithIdentity(buyJobKey));
                q.AddTrigger(opts => opts
                    .ForJob(buyJobKey)
                    .WithIdentity("buyTradeJob-trigger")
                    .WithCronSchedule("0/10 * * * * ?"));
            });

            // ASP.NET Core hosting
            services.AddQuartzHostedService(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });
            
            var pgString = "Host=localhost;Port=5432;Database=Ledger2;Username=postgres;Password=mysecretpassword;";
            services.AddControllers(); 
            // add dbcontext
            services.AddDbContext<ApiDbContext>(option =>
            {
                option.UseNpgsql(pgString);
            });
            //services.AddDbContext<ApiDbContext>(option => option.UseInMemoryDatabase("Ledger"));
            services.AddSwaggerGen(); // add swagger
            // add interfaces for dbcontext (connection to database, database layer)
            services.AddScoped<IDbContext,ApiDbContext>();
            // add interfaces and repositories for data repository layer (data access)
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IStockRepository,StockRepository>();
            services.AddScoped<IStocksOfUserRepository,StocksOfUserRepository>();
            services.AddScoped<IBuyOrderRepository,BuyOrderRepository>();
            services.AddScoped<ISellOrderRepository, SellOrderRepository>();
            services.AddScoped<ITransactionRepository,TransactionRepository>();
            services.AddScoped<IDailyStockRepository, DailyStockRepository>();
            services.AddScoped<ISellOrderMatchRepository, SellOrderMatchRepository>();
            services.AddScoped<IBuyOrderMatchRepository, BuyOrderMatchRepository>();
            
            // add interfaces and services for bussiness layer
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IStocksOfUserService, StocksOfUserService>();
            services.AddScoped<IBuyOrderService, BuyOrderService>();
            services.AddScoped<ISellOrderService, SellOrderService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IDailyStockService, DailyStockService>();
            services.AddScoped<ISellOrderMatchService, SellOrderMatchService>();
            //services.AddScoped<IScheduler, SchedulerService>();
            
            //add interface for unitofwork
            services.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddTransient<SellTradeJob>();
            
            /*//inject scheduler 
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<IScheduler>(provider =>
            {
                ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
                return schedulerFactory.GetScheduler().Result;
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApiDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            dbContext.Database.EnsureCreated();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ledger API V1");
                c.RoutePrefix = string.Empty;
            });
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
    }
}