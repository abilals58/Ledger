using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ledger.Ledger.Web
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<ApiDbContext>(option =>
                option.UseNpgsql(
                    "Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;"));
            services.AddSwaggerGen();
            services.AddScoped<UserService>();
            services.AddScoped<StockService>();
            services.AddScoped<StocksOfUserService>();
            services.AddScoped<BuyOrderService>();
            services.AddScoped<SellOrderService>();
            services.AddScoped<TransactionService>();
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
        }
    }
}