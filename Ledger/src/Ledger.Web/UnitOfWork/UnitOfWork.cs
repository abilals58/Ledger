using System;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ledger.Ledger.Web.UnitOfWork
{

    public interface IUnitOfWork :IDisposable
    {
        Task CommitAsync();
        Task RollBackAsync();

    }
    
    public class UnitOfWork :IUnitOfWork
    {
        private readonly IDbContext _dbContext;
        private IDbContextTransaction _transaction;

        public UnitOfWork(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _transaction = _dbContext.BeginTransaction();
        }
        
        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        public async Task RollBackAsync()
        {
            await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}