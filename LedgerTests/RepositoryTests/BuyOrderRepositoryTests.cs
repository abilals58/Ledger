/*using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LedgerTests.RepositoryTests
{
    public class BuyOrderRepositoryTests
    {
        [Fact]
        public async Task GetAllBuyOrdersAsync_ShouldReturnAllBuyOrders()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseInMemoryDatabase("Ledger").Options;
            var dbContext = new ApiDbContext(options);
            var buyOrderRepository = new BuyOrderRepository(dbContext);
            
            // Act
            var result = await buyOrderRepository.GetAllBuyOrdersAsync();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<BuyOrder>>(result); // what if it returns null, there is no users ???
        }
        
        [Fact]
        public async Task GetBuyOrderByIdAsync_ShouldReturnABuyOrder()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseInMemoryDatabase("Ledger").Options;
            var dbContext = new ApiDbContext(options);
            var buyOrderRepository = new BuyOrderRepository(dbContext);
            // Act
            var result = await buyOrderRepository.GetBuyOrderByIdAsync(3); // what if there is not a user with id 1 ???

            // Assert
            Assert.IsAssignableFrom<BuyOrder>(result);
        }
        
        [Fact]
        public async Task AddBuyOrderAsync_ShouldReturnAddedBuyOrder()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var buyOrderRepository = new BuyOrderRepository(dbContext);
            var buyOrder = new BuyOrder(1,1,1,10,20,default);
            
            // Act
            var result = await buyOrderRepository.AddBuyOrderAsync(buyOrder);

            // Assert
            Assert.IsAssignableFrom<BuyOrder>(result);
        }

        [Fact]
        public async Task UpdateBuyOrderAsync_ShouldReturnUpdatedBuyOrder()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;")
                .Options;
            var dbContext = new ApiDbContext(options);
            var buyOrderRepository = new BuyOrderRepository(dbContext);
            var id = 3;
            var buyOrder = new BuyOrder(id,1,1,10,20,default);
            // Act
            var result = await buyOrderRepository.UpdateByOrderAsync(id,buyOrder);

            // Assert
            Assert.IsAssignableFrom<BuyOrder>(result);
        }
        
        [Fact]
        public async Task DeleteBuyOrderAsync_ShouldReturnDeletedBuyOrder()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var buyOrderRepository = new BuyOrderRepository(dbContext);
            // add new user (test purpose)
            var id = 6;
            var buyOrder = new BuyOrder(id,1,1,10,20,default);
            await buyOrderRepository.AddBuyOrderAsync(buyOrder);
            // Act delete the user
            var result = await buyOrderRepository.DeleteBuyOrderAsync(id);

            // Assert
            Assert.IsAssignableFrom<BuyOrder>(result);
        }
    }
}*/