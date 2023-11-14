using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Services
{
    public class TransactionService // Transaction service corresponds to data tier and handles database operations
    {
        private ApiDbContext _dbContext;

        public TransactionService(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<Transaction>> GetAlTransactionsAsync() // returns all transactions
        {
            return await _dbContext.Transactions.ToListAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id) // returns a transaction by id
        {
            return await _dbContext.Transactions.FindAsync(id);
            
        }
        
        public async Task AddTransactionAsync(Transaction transaction) // adds a transaction to the database
        {
            await _dbContext.Transactions.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task<Transaction> UpdateTransactionAsync(int id, Transaction newtransaction) // updates a transaction and returns it, returns null if there is no match
        {
            var transaction = await _dbContext.Transactions.FindAsync(id);
            if (transaction == null) return null;
            transaction.SellerId = newtransaction.SellerId;
            transaction.BuyerId = newtransaction.BuyerId;
            transaction.StockId = newtransaction.StockId;
            transaction.StockNum = newtransaction.StockNum;
            transaction.Price = newtransaction.Price;
            transaction.Date = newtransaction.Date;
            await _dbContext.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction> DeleteTransactionAsync(int id)   // delete transaction and returns it, returns null if there is no match
        {
            var transaction = await _dbContext.Transactions.FindAsync(id);
            if (transaction == null) return null;
            _dbContext.Transactions.Remove(transaction);
            await _dbContext.SaveChangesAsync();
            return transaction;
        }
    }
}