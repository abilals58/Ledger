using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace LedgerTests.RepositoryTests
{
    /*public class UserRepositoryTests
    {
        
        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnAllUsers()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var userRepository = new UserRepository(dbContext);
            
            // Act
            var result = await userRepository.GetAllUsersAsync();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<User>>(result); // what if it returns null, there is no users ???
        }
        
        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnAUser()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var userRepository = new UserRepository(dbContext);
            
            // Act
            var result = await userRepository.GetUserByIdAsync(1); // what if there is not a user with id 1 ???

            // Assert
            Assert.IsAssignableFrom<User>(result);
        }
        
        [Fact]
        public async Task AddUserAsync_ShouldReturnAddedUser()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var userRepository = new UserRepository(dbContext);
            var user = new User { Name = "Mehmet", Surname = "Yildiz", UserName = "myildiz",Email = "abilal@gmail.com",Password = "123458", Phone = "5343519032" };
            
            // Act
            var result = await userRepository.AddUserAsync(user);

            // Assert
            Assert.IsAssignableFrom<User>(result);
        }

        [Fact]
        public async Task UpdateUserAsync_ShouldReturnUpdatedUser()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;")
                .Options;
            var dbContext = new ApiDbContext(options);
            var userRepository = new UserRepository(dbContext);

            var id = 1;
            var user = new User { UserId = id, Name = "Ahmet", Surname = "Yildiz", UserName = "abilal",Email = "abilal@gmail.com",Password = "123458", Phone = "5343519032" };

            // Act
            var result = await userRepository.UpdateUserAsync(id,user);

            // Assert
            Assert.IsAssignableFrom<User>(result);
        }
        
        [Fact]
        public async Task DeleteUserAsync_ShouldReturnDeletedUser()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApiDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=Ledger;Username=postgres;Password=mysecretpassword;").Options;
            var dbContext = new ApiDbContext(options);
            var userRepository = new UserRepository(dbContext);
            // add new user (test purpose)
            var id = 6;
            var user = new User { Name = "  Emre", Surname = "Yildiz", UserName = "eyildiz",Email = "eyildiz@gmail.com",Password = "123458", Phone = "5343519032" };
            await userRepository.AddUserAsync(user);
            // Act delete the user
            var result = await userRepository.DeleteUserAsync(id);

            // Assert
            Assert.IsAssignableFrom<User>(result);
        }
        
    }*/
}