using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Data
{
    public class ApiDbContext :DbContext
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
        
        
    }
}