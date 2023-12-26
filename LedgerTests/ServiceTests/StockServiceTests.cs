using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.Services;
using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace LedgerTests.ServiceTests
{
    public class StockServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public StockServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task GetAllStocksAsync_ShouldReturnAllStocks()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseInMemoryDatabase("Ledger").Options;
            var dbContext = new ApiDbContext(options);
            var stockRepository = new StockRepository(dbContext);
            var stockService = new StockService(stockRepository);
            
            // Act
            var result = await stockService.GetAllStocksAsync();
            //print result
            foreach (var stock in result)
            {
                _testOutputHelper.WriteLine(stock.ToString());
            }

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Stock>>(result); // what if it returns null, there is no users ???
        }
        
        [Fact]
        public async Task GetStockByIdAsync_ShouldReturnAStock()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseInMemoryDatabase("Ledger").Options;
            var dbContext = new ApiDbContext(options);
            var stockRepository = new StockRepository(dbContext);
            var stockService = new StockService(stockRepository);
            
            // Act
            var stock = new Stock(1,"THY",100,10,8,20,30,10,true);
            await stockService.AddStockAsync(stock);
            var result = await stockService.GetStockByIdAsync(1); // what if there is not a user with id 1 ???

            // Assert
            Assert.IsAssignableFrom<Stock>(result);
            //print the result
            _testOutputHelper.WriteLine(result.ToString());
        }
        
        [Fact]
        public async Task AddStockAsync_ShouldReturnAddedStock()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseInMemoryDatabase("Ledger").Options;
            var dbContext = new ApiDbContext(options);
            var stockRepository = new StockRepository(dbContext);
            var stockService = new StockService(stockRepository);
            var stock = new Stock(3,"THY",100,10,8,20,30,10,true);
            
            // Act
            var result = await stockService.AddStockAsync(stock);

            // Assert
            Assert.IsAssignableFrom<Stock>(result);
            //print the result
            _testOutputHelper.WriteLine(result.ToString());
        }

        [Fact]
        public async Task UpdateStocksAsync_ShouldReturnUpdatedStock()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseInMemoryDatabase("Ledger").Options;
            var dbContext = new ApiDbContext(options);
            var stockRepository = new StockRepository(dbContext);
            var stockService = new StockService(stockRepository);
            var id = 2;
            var newstock = new Stock(id,"THY",100,10,0,50,60,30, true);

            // Act
            var stock = new Stock(id,"THY",100,10,8,20,30,10,true);
            await stockService.AddStockAsync(stock);
            var result = await stockService.UpdateStockAsync(id,newstock);

            // Assert
            Assert.IsAssignableFrom<Stock>(result);
            //print the result
            _testOutputHelper.WriteLine(result.ToString());
        }
        
        [Fact]
        public async Task DeleteStockAsync_ShouldReturnDeletedStock()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseInMemoryDatabase("Ledger").Options;
            var dbContext = new ApiDbContext(options);
            var stockRepository = new StockRepository(dbContext);
            var stockService = new StockService(stockRepository);
            
            // add new user (test purpose)
            var id = 6;
            var stock = new Stock(id,"ASELSAN",100,10,40,50,55,40, true);
            await stockService.AddStockAsync(stock);
            // Act delete the user
            var result = await stockService.DeleteStockAsync(id);

            // Assert
            Assert.IsAssignableFrom<Stock>(result);
            //print the result
            _testOutputHelper.WriteLine(result.ToString());
        }

        [Fact]
        public async Task RetrieveAllStockInfo_ShouldReturnListOfObjectLists()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseInMemoryDatabase("Ledger").Options;
            var dbContext = new ApiDbContext(options);
            var stockRepository = new StockRepository(dbContext);
            var stockService = new StockService(stockRepository);
            //add several stocks (test purpose)
            var stock1 = new Stock(5,"ASELSAN",100,10,40,50,55,40, true);
            var stock2 = new Stock(6,"TURKCELL",1000,15,20,100,100,10, true);
            await stockService.AddStockAsync(stock1);
            await stockService.AddStockAsync(stock2);
            
            //act retrieve all info
            var result = await stockService.RetrieveAllStockInfo();

            //Assert
            Assert.IsAssignableFrom<List<List<Object>>>(result);
        }

        [Fact]
        public async Task UpdateAStockPrice_ShouldReturnUpdatedStock()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseInMemoryDatabase("Ledger").Options;
            var dbContext = new ApiDbContext(options);
            var stockRepository = new StockRepository(dbContext);
            var stockService = new StockService(stockRepository);
            // add a stock (test purpose)
            var stock = new Stock(7,"HEPS",500,100,0,1000,1500,800, true);
            await stockService.AddStockAsync(stock);
            //act simulate a new trade happen with given price
            var result = await stockService.UpdateAStockPrice(7, 700);
            
            //assert
            Assert.IsAssignableFrom<Stock>(result);
            //print the result
            _testOutputHelper.WriteLine(result.ToString());
            
        }
    }
}