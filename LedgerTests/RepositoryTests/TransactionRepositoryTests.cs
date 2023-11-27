using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Transaction = Ledger.Ledger.Web.Models.Transaction;

namespace LedgerTests.RepositoryTests
{
    public class TransactionRepositoryTests
    {
        
        [Fact]
        public async Task GetAllTransactionsAsync_ShouldReturnAllTransactions()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var transactionRepository = new TransactionRepository(dbContext);
            
            // Act
            var result = await transactionRepository.GetAlTransactionsAsync();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Transaction>>(result); // what if it returns null, there is no users ???
        }
        
        [Fact]
        public async Task GetTransactionByIdAsync_ShouldReturnATransaction()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var transactionRepository = new TransactionRepository(dbContext);
            
            // Act
            var result = await transactionRepository.GetTransactionByIdAsync(1); // what if there is not a user with id 1 ???

            // Assert
            Assert.IsAssignableFrom<Transaction>(result);
        }
        
        [Fact]
        public async Task AddTransactionAsync_ShouldReturnAddedTransaction()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var transactionRepository = new TransactionRepository(dbContext);
            var transaction = new Transaction(default,1,2,1,10,1000,default);
            // Act
            var result = await transactionRepository.AddTransactionAsync(transaction);

            // Assert
            Assert.IsAssignableFrom<Transaction>(result);
        }

        [Fact]
        public async Task UpdateTransactionAsync_ShouldReturnUpdatedTransaction()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var transactionRepository = new TransactionRepository(dbContext);

            var id = 1;
            var transaction = new Transaction(id,1,2,1,9,900,default);
            // Act
            var result = await transactionRepository.UpdateTransactionAsync(id,transaction);

            // Assert
            Assert.IsAssignableFrom<Transaction>(result);
        }
        
        [Fact]
        public async Task DeleteTransactionAsync_ShouldReturnDeletedTransaction()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var transactionRepository = new TransactionRepository(dbContext);
            
            // add new transaction (test purpose)
            var id = 5;
            var transaction = new Transaction(id,1,2,1,9,900,default);
            await transactionRepository.AddTransactionAsync(transaction);
            // Act delete the user
            var result = await transactionRepository.DeleteTransactionAsync(id);

            // Assert
            Assert.IsAssignableFrom<Transaction>(result);
        }
        
    }
}