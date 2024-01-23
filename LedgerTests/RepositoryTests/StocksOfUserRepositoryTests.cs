/*using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LedgerTests.RepositoryTests
{
    public class StocksOfUserRepositoryTests
    {
         [Fact]
        public async Task GetAllStocksOfUserAsync_ShouldReturnAllStocksOfUsers()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var stocksOfUserRepository = new StocksOfUserRepository(dbContext);
            
            // Act
            var result = await stocksOfUserRepository.GetAllStocksOfUserAsync();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<StocksOfUser>>(result); // what if it returns null, there is no users ???
        }
        
        [Fact]
        public async Task GetStocksOfUserByIdAsync_ShouldReturnAStocksOfUser()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var stocksOfUserRepository = new StocksOfUserRepository(dbContext);            
            // Act
            var result = await stocksOfUserRepository.GetStocksOfUserByIdAsync(0); // what if there is not a user with id 1 ???

            // Assert
            Assert.IsAssignableFrom<StocksOfUser>(result);
        }
        
        [Fact]
        public async Task AddStocksOfUserAsync_ShouldReturnAddedStocksOfUser()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var stocksOfUserRepository = new StocksOfUserRepository(dbContext);
            var stocksOfUser = new StocksOfUser(1,1,1,10);
            
            // Act
            var result = await stocksOfUserRepository.AddStocksOfUserAsync(stocksOfUser);

            // Assert
            Assert.IsAssignableFrom<StocksOfUser>(result);
        }

        [Fact]
        public async Task UpdateStocksOfUserAsync_ShouldReturnUpdatedStocksOfUser()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;")
                .Options;
            var dbContext = new ApiDbContext(options);
            var stocksOfUserRepository = new StocksOfUserRepository(dbContext);
            var id = 4;
            var stocksOfUser = new StocksOfUser(id, 1, 1, 5);
            // Act
            var result = await stocksOfUserRepository.UpdateStocksOfUserAsync(id,stocksOfUser);

            // Assert
            Assert.IsAssignableFrom<StocksOfUser>(result);
        }
        
        [Fact]
        public async Task DeleteStocksOfUserAsync_ShouldReturnDeletedStocksOfUser()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var stocksOfUserRepository = new StocksOfUserRepository(dbContext);
            // add new user (test purpose)
            var id = 5;
            var stocksOfUser = new StocksOfUser(id, 1, 1, 10);
            await stocksOfUserRepository.AddStocksOfUserAsync(stocksOfUser);
            // Act delete the user
            var result = await stocksOfUserRepository.DeleteStocksOfUserAsync(id);

            // Assert
            Assert.IsAssignableFrom<StocksOfUser>(result);
        }
    }
}*/