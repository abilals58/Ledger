using System;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace LedgerTests.ServiceTests
{
    
    //şimdilik repo testi, servise çevrilecek
    public class BuyOrderServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public BuyOrderServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task OperateBuyOrder_ShouldReturnOperatedBuyOrder()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseInMemoryDatabase("Ledger").Options;
            var dbContext = new ApiDbContext(options);
            var buyOrderRepository = new BuyOrderRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var stockRepository = new StockRepository(dbContext);
            var buyOrderService = new BuyOrderService(buyOrderRepository);
            var stocksOfUserRepository = new StocksOfUserRepository(dbContext);
            
            //initiate a user,stock,stocksOfUser,sellOrder
            var user = new User(1, "Ahmet", "Yildiz", "ayildiz", "ahmet.yildiz@hepsiburada.com", "123456", "5343519032",
                1000);
            await userRepository.AddUserAsync(user);
            
            var stock = new Stock(1, "THY", 100, 10, 0, 100, 150, 50, true);
            await stockRepository.AddStockAsync(stock);
            
            var stocksOfUser = new StocksOfUser(1, 1, 10,user,stock);
            await stocksOfUserRepository.AddStocksOfUserAsync(stocksOfUser);

            var buyOrder = new BuyOrder(1, 1, 1, 100, 10, true, user, stock);
            await buyOrderRepository.AddBuyOrderAsync(buyOrder);
            
            //act
            var result = await buyOrderRepository.OperateBuyOrderAsync(1);
            
            //assert
            Assert.IsAssignableFrom<BuyOrder>(result);
            _testOutputHelper.WriteLine(result.ToString());

        }
    }
}