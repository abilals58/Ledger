using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories
{

    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAlTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task<Transaction> AddTransactionAsync(Transaction transaction);
        Task<Transaction> UpdateTransactionAsync(int id, Transaction newtransaction);
        Task<Transaction> DeleteTransactionAsync(int id);
        
    }

    public class TransactionRepository : ITransactionRepository // Transaction service corresponds to data tier and handles database operations

    {
    private readonly DbSet<Transaction> _dbTransaction;

    public TransactionRepository(IDbContext dbContext)
    {
        _dbTransaction = dbContext.Transactions;
    }

    public async Task<IEnumerable<Transaction>> GetAlTransactionsAsync() // returns all transactions
    {
        return await _dbTransaction.ToListAsync();
    }

    public async Task<Transaction> GetTransactionByIdAsync(int id) // returns a transaction by id
    {
        return await _dbTransaction.FindAsync(id);
    }

    public async Task<Transaction> AddTransactionAsync(Transaction transaction) // adds a transaction to the database and returns the added transaction
    {
        await _dbTransaction.AddAsync(transaction);
        //await _dbContext.SaveChangesAsync();
        return transaction;
    }

    public async Task<Transaction> UpdateTransactionAsync(int id, Transaction newtransaction) // updates a transaction and returns it, returns null if there is no match
    {
        var transaction = await _dbTransaction.FindAsync(id);
        if (transaction == null) return null;
        transaction.SellerId = newtransaction.SellerId;
        transaction.BuyerId = newtransaction.BuyerId;
        transaction.StockId = newtransaction.StockId;
        transaction.StockNum = newtransaction.StockNum;
        transaction.Price = newtransaction.Price;
        transaction.Date = newtransaction.Date;
        //await _dbContext.SaveChangesAsync();
        return transaction;
    }

    public async Task<Transaction>
        DeleteTransactionAsync(int id) // delete transaction and returns it, returns null if there is no match
    {
        var transaction = await _dbTransaction.FindAsync(id);
        if (transaction == null) return null;
        _dbTransaction.Remove(transaction);
       // await _dbContext.SaveChangesAsync();
        return transaction;
    }
    }
}