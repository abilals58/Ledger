using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;

namespace Ledger.Ledger.Web.Services
{
    
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAlTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task AddTransactionAsync(Transaction transaction);
        Task<Transaction> UpdateTransactionAsync(int id, Transaction newtransaction);
        Task<Transaction> DeleteTransactionAsync(int id);
        
    }
    public class TransactionService :ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<IEnumerable<Transaction>> GetAlTransactionsAsync()
        {
            return await _transactionRepository.GetAlTransactionsAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _transactionRepository.GetTransactionByIdAsync(id);
        }
        
        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.AddTransactionAsync(transaction);
        }

        public async Task<Transaction> UpdateTransactionAsync(int id, Transaction newtransaction)
        {
            return await _transactionRepository.UpdateTransactionAsync(id, newtransaction);
        }

        public async Task<Transaction> DeleteTransactionAsync(int id)
        {
            return await _transactionRepository.DeleteTransactionAsync(id);
        }
        
        
    }
}