using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.UnitOfWork;

namespace Ledger.Ledger.Web.Services
{
    
    public interface ITransactionService
    {
        Task<IEnumerable<Transaction>> GetAlTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(int id);
        Task<Transaction> AddTransactionAsync(Transaction transaction);
        Task<Transaction> UpdateTransactionAsync(int id, Transaction newtransaction);
        Task<Transaction> DeleteTransactionAsync(int id);
        
    }
    public class TransactionService :ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private IUnitOfWork _unitOfWork;

        public TransactionService(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Transaction>> GetAlTransactionsAsync()
        {
            return await _transactionRepository.GetAlTransactionsAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(int id)
        {
            return await _transactionRepository.GetTransactionByIdAsync(id);
        }
        
        public async Task<Transaction> AddTransactionAsync(Transaction transaction)
        {
            try
            {
                await _transactionRepository.AddTransactionAsync(transaction);
                await _unitOfWork.CommitAsync();
                return transaction;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBackAsync();
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Transaction> UpdateTransactionAsync(int id, Transaction newtransaction)
        {
            try
            {
                var transaction =  await _transactionRepository.UpdateTransactionAsync(id, newtransaction);
                await _unitOfWork.CommitAsync();
                return transaction;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBackAsync();
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Transaction> DeleteTransactionAsync(int id)
        {
            try
            {
                var transaction = await _transactionRepository.DeleteTransactionAsync(id);
                await _unitOfWork.CommitAsync();
                return transaction;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollBackAsync();
                Console.WriteLine(e);
                throw;
            }
        }
        
        
    }
}