/*using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LedgerTests.RepositoryTests
{
    public class SellOrderRepositoryTests
    {
        [Fact]
        public async Task GetAllSellOrdersAsync_ShouldReturnAllSellOrders()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var sellOrderRepository = new SellOrderRepository(dbContext);
            
            // Act
            var result = await sellOrderRepository.GetAllSellOrdersAsync();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<SellOrder>>(result); // what if it returns null, there is no users ???
        }
        
        [Fact]
        public async Task GetUSellOrderByIdAsync_ShouldReturnASellOrder()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var sellOrderRepository = new SellOrderRepository(dbContext);            
            // Act
            var result = await sellOrderRepository.GetSellOrderByIdAsync(1); // what if there is not a user with id 1 ???

            // Assert
            Assert.IsAssignableFrom<SellOrder>(result);
        }
        
        [Fact]
        public async Task AddSellOrderAsync_ShouldReturnAddedSellOrder()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var sellOrderRepository = new SellOrderRepository(dbContext);
            var sellOrder = new SellOrder(1,1,1,10,20,default);
            
            // Act
            var result = await sellOrderRepository.AddSellOrderAsync(sellOrder);

            // Assert
            Assert.IsAssignableFrom<SellOrder>(result);
        }

        [Fact]
        public async Task UpdateSellOrderAsync_ShouldReturnUpdatedSellOrder()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;")
                .Options;
            var dbContext = new ApiDbContext(options);
            var sellOrderRepository = new SellOrderRepository(dbContext);
            var id = 1;
            var sellOrder = new SellOrder(id,1,1,12,15,default);
            // Act
            var result = await sellOrderRepository.UpdateSellOrderAsync(id,sellOrder);

            // Assert
            Assert.IsAssignableFrom<SellOrder>(result);
        }
        
        [Fact]
        public async Task DeleteSellOrderAsync_ShouldReturnDeletedSellOrder()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var sellOrderRepository = new SellOrderRepository(dbContext);
            // add new user (test purpose)
            var id = 6;
            var sellOrder = new SellOrder(id,1,1,12,15,default);
            await sellOrderRepository.AddSellOrderAsync(sellOrder);
            // Act delete the user
            var result = await sellOrderRepository.DeleteSellOrderAsync(id);

            // Assert
            Assert.IsAssignableFrom<SellOrder>(result);
        }
    }
}*/