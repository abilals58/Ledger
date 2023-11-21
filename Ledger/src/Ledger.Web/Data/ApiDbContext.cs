using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Data
{
    public interface IUserContext //implements user table only
    {
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync();
    }
    
    public interface IStockContext //implements user table only
    {
        DbSet<Stock> Stocks { get; set; }
        Task<int> SaveChangesAsync();
    }
    
    public interface IStocksOfUserContext //implements user table only
    {
        DbSet<StocksOfUser> StocksOfUser{ get; set; }
        Task<int> SaveChangesAsync();
    }

    public interface IBuyOrderContext //implements user table only
    {
        DbSet<BuyOrder> BuyOrders { get; set; }
        Task<int> SaveChangesAsync();
    }

    public interface ISellOrderContext //implements user table only
    {
        DbSet<SellOrder> SellOrders { get; set; }
        Task<int> SaveChangesAsync();
    }

    public interface ITransactionContext //implements user table only
    {
        DbSet<Transaction> Transactions { get; set; }
        Task<int> SaveChangesAsync();
    }
    public class ApiDbContext :DbContext, IUserContext, IStockContext, IStocksOfUserContext, IBuyOrderContext, ISellOrderContext, ITransactionContext
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