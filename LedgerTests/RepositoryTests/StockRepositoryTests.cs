using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LedgerTests.RepositoryTests
{
    public class StockRepositoryTests
    {
        [Fact]
        public async Task GetAllStocksAsync_ShouldReturnAllStocks()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var stockRepository = new StockRepository(dbContext);
            
            // Act
            var result = await stockRepository.GetAllStocksAsync();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Stock>>(result); // what if it returns null, there is no users ???
        }
        
        [Fact]
        public async Task GetStockByIdAsync_ShouldReturnAStock()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var stockRepository = new StockRepository(dbContext);            
            // Act
            var result = await stockRepository.GetStockByIdAsync(1); // what if there is not a user with id 1 ???

            // Assert
            Assert.IsAssignableFrom<Stock>(result);
        }
        
        [Fact]
        public async Task AddStockAsync_ShouldReturnAddedStock()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var stockRepository = new StockRepository(dbContext);
            var stock = new Stock(3,"THY",default,100,10,50,8);
            
            // Act
            var result = await stockRepository.AddStockAsync(stock);

            // Assert
            Assert.IsAssignableFrom<Stock>(result);
        }

        [Fact]
        public async Task UpdateStocksAsync_ShouldReturnUpdatedStock()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;")
                .Options;
            var dbContext = new ApiDbContext(options);
            var stockRepository = new StockRepository(dbContext);
            var id = 1;
            var stock = new Stock(id,"THY",default,100,10,50,9);

            // Act
            var result = await stockRepository.UpdateStockAsync(id,stock);

            // Assert
            Assert.IsAssignableFrom<Stock>(result);
        }
        
        [Fact]
        public async Task DeleteStockAsync_ShouldReturnDeletedStock()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var stockRepository = new StockRepository(dbContext);
            // add new user (test purpose)
            var id = 6;
            var stock = new Stock(id,"ASELSAN",default,100,10,50,9);
            await stockRepository.AddStockAsync(stock);
            // Act delete the user
            var result = await stockRepository.DeleteStockAsync(id);

            // Assert
            Assert.IsAssignableFrom<Stock>(result);
        }
    }
}