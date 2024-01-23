using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ledger.Ledger.Web.Data
{
    public interface IDbContext 
    {
        DbSet<User> Users { get; set; }
        DbSet<Stock> Stocks { get; set; }
        DbSet<StocksOfUser> StocksOfUser{ get; set; }
        DbSet<BuyOrder> BuyOrders { get; set; }
        DbSet<SellOrder> SellOrders { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        DbSet<DailyStock> DailyStocks { get; set; }
        
        DbSet<SellOrderMatch> SellOrderMatches { get; set; }
        DbSet<BuyOrderMatch> BuyOrderMatches { get; set; }

        IDbContextTransaction BeginTransaction();
        Task<int> SaveChangesAsync();
    }
    public class ApiDbContext : DbContext, IDbContext
    {
        // form dbcontext object 
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {

        }

        // form DbSets (tables)
        public DbSet<User> Users { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StocksOfUser> StocksOfUser { get; set; }
        public DbSet<BuyOrder> BuyOrders { get; set; }
        public DbSet<SellOrder> SellOrders { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<DailyStock> DailyStocks { get; set; }
        public DbSet<SellOrderMatch> SellOrderMatches { get; set; }
        public DbSet<BuyOrderMatch> BuyOrderMatches { get; set; }

        public IDbContextTransaction BeginTransaction()
        {
            return  base.Database.BeginTransaction();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure composite primary key for Dailystocks entity
            modelBuilder.Entity<DailyStock>()
                .HasKey(ds => new { ds.StockId, ds.Date });
            modelBuilder.Entity<StocksOfUser>()
                .HasKey(sou => new { sou.UserId, sou.StockId });
            modelBuilder.Entity<SellOrderMatch>()
                .HasKey(som => new { som.SellOrderId, som.BuyOrderId });
            modelBuilder.Entity<BuyOrderMatch>()
                .HasKey(bom => new { bom.BuyOrderId, bom.SellOrderId });
            // Additional configurations, if needed
            base.OnModelCreating(modelBuilder);
        }

    }

}