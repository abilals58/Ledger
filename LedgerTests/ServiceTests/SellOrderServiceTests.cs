/*using System;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.Services;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace LedgerTests.ServiceTests
{
    public class SellOrderServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public SellOrderServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        //şimdilik repo testi, servise çevrilecek!!!
        [Fact]
        public async Task OperateSellOrder_ShouldReturnOperatedSellOrder()
        {
            //arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseInMemoryDatabase("Ledger").Options;
            var dbContext = new ApiDbContext(options);
            var sellOrderRepository = new SellOrderRepository(dbContext);
            var userRepository = new UserRepository(dbContext);
            var stockRepository = new StockRepository(dbContext);
            var sellOrderService = new SellOrderService(sellOrderRepository);
            var stocksOfUserRepository = new StocksOfUserRepository(dbContext);
            
            //initiate a user,stock,stocksOfUser,sellOrder
            var user = new User(1, "Ahmet", "Yildiz", "ayildiz", "ahmet.yildiz@hepsiburada.com", "123456", "5343519032",
                1000);
            await userRepository.AddUserAsync(user);
            
            var stock = new Stock(1, "THY", 100, 10, 0, 100, 150, 50, true);
            await stockRepository.AddStockAsync(stock);
            
            var stocksOfUser = new StocksOfUser(1, 1, 10,user,stock);
            await stocksOfUserRepository.AddStocksOfUserAsync(stocksOfUser);

            var sellOrder = new SellOrder(1, 1, 1, 100, 10, true, user, stock);
            await sellOrderRepository.AddSellOrderAsync(sellOrder);
            
            //act
            var result = await sellOrderRepository.OperateSellOrderAsync(1);
            
            //assert
            Assert.IsAssignableFrom<SellOrder>(result);
            _testOutputHelper.WriteLine(result.ToString());
            
        }
    }
}*/