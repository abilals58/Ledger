using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

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
        
        Task<int> SaveChangesAsync();
    }


    public class ApiDbContext :DbContext, IDbContext
    {
        // form dbcontext object 
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
            
        }
        
        // form DbSets (tables)
        public DbSet<User> Users { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StocksOfUser> StocksOfUser{ get; set; }
        public DbSet<BuyOrder> BuyOrders { get; set; }
        public DbSet<SellOrder> SellOrders { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
        
    }
}