/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Controllers;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace LedgerTests.ControllerTests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task GetAllUsers_ShouldReturnListOfUsers() // return type check unit result
        {
            // arrange
            var users = new List<User>
            {
                new User {UserId = 1, Name = "Ahmet",Surname = "Yildiz", UserName = "abilal", Email = "abilal@gmail.com", Password = "123458", Phone = "5343519032"},
                new User {UserId = 2, Name = "Yusuf",Surname = "Colak", UserName = "ycolak", Email = "ycolak@gmail.com", Password = "123424", Phone = "5353519982"}
            };
            
            var userServiceMock = new Mock<IUserRepository>(); // this is the mock version of userService with the test purpose
            userServiceMock.Setup(service => service.GetAllUsersAsync()).ReturnsAsync(users);
            
            var controller = new UsersController(userServiceMock.Object);
            
            //act
            var result = await controller.Get();
            //assert
            //var actionResult = Assert.IsType<ActionResult<IEnumerable<User>>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(okObjectResult.Value);
            Assert.Equal(users.Count, model.Count());
            
        }

        [Fact]
        public async Task Get_ReturnsOkResultWithUser_WhenUserExists()
        {
            //arrange
            var id = 1;
            var user = new User
            {
                UserId = id, Name = "Ahmet", Surname = "Yildiz", UserName = "abilal", Email = "abilal@gmail.com",
                Password = "123458", Phone = "5343519032"
            };
            
            var userServiceMock = new Mock<IUserRepository>(); // this is the mock version of userService with the test purpose
            userServiceMock.Setup(service => service.GetUserByIdAsync(id)).ReturnsAsync(user);
            var controller = new UsersController(userServiceMock.Object);
            
            //act
            var result = await controller.Get(id);
            
            //assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result); //check the return type
            var model = Assert.IsAssignableFrom<User>(okObjectResult.Value); // get the returned object
            Assert.Equal(user.UserId, model.UserId); // check then returned object
            Assert.Equal(user.UserName, model.UserName); 
            
        }
        
        [Fact]
        public async Task Get_ReturnsNotFound_WhenUserDoesNotExist()
        {
            //arrange
            var id = 1;
            
            var userServiceMock = new Mock<IUserRepository>(); // this is the mock version of userService with the test purpose
            userServiceMock.Setup(service => service.GetUserByIdAsync(id)).ReturnsAsync(null as User);
            var controller = new UsersController(userServiceMock.Object);
            
            //act
            var result = await controller.Get(id);
            
            //assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result); // check the return type
            Assert.Equal("User with given id does not exist in the database!", notFoundResult.Value); // check the returned object
        }
        [Fact]
        public async Task Post_ReturnsCreatedResultWithUser_WhenUserIsAddedSuccessfully()
        {
            //arrange
            var userToAdd = new User
                { Name = "Ahmet", Surname = "Yildiz", UserName = "abilal", Email = "abilal@gmail.com",Password = "123458", Phone = "5343519032" };
            var createdUser = new User 
                { UserId = 1, Name = "Ahmet", Surname = "Yildiz", UserName = "abilal", Email = "abilal@gmail.com", Password = "123458", Phone = "5343519032" };
            var userServiceMock = new Mock<IUserRepository>(); // this is the mock version of userService with the test purpose
            userServiceMock.Setup(x => x.AddUserAsync(userToAdd)).ReturnsAsync(createdUser);
            var controller = new UsersController(userServiceMock.Object);
            
            // act 
            var result = await controller.Post(userToAdd);
            
            // assert

            var createdResult = Assert.IsType<ObjectResult>(result);
            //var model = Assert.IsType<User>(createdResult.Value);
            //Assert.Equal(createdUser.UserId, model.UserId);
            //Assert.Equal(createdUser.UserName, model.UserName);
            Assert.Equal(201, createdResult.StatusCode);
            
        }
        [Fact]
        public async Task Put_ReturnsUpdatedUser_WhenUserUpdated()
        {
            //arrange
            int id = 1;
            var userToUpdate = new User
                { Name = "Ahmet", Surname = "Yildiz", UserName = "abilals", Email = "abilal@gmail.com",Password = "123458", Phone = "5343519032" };
            var updatedUser = new User 
                { UserId = id, Name = "Ahmet", Surname = "Yildiz", UserName = "abilals", Email = "abilal@gmail.com", Password = "123458", Phone = "5343519032" };
            var userServiceMock = new Mock<IUserRepository>(); // this is the mock version of userService with the test purpose
            userServiceMock.Setup(x => x.UpdateUserAsync(id, userToUpdate)).ReturnsAsync(userToUpdate);
            var controller = new UsersController(userServiceMock.Object);
            
            //act
            var result = await controller.Put(id, userToUpdate);
            
            //assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result); //check the return type
            //var model = Assert.IsAssignableFrom<User>(okObjectResult.Value); // get the returned object
            //Assert.Equal(updatedUser.UserId, model.UserId); // check then returned object
            //Assert.Equal(updatedUser.UserName, model.UserName); 

        }
        [Fact]
        public async Task Delete_ReturnsDeletedUser_WhenUserIsDeleted()
        {
            //arrange
            var id = 1;
            var user = new User
            {
                UserId = id, Name = "Ahmet", Surname = "Yildiz", UserName = "abilal", Email = "abilal@gmail.com",
                Password = "123458", Phone = "5343519032"
            };
            
            var userServiceMock = new Mock<IUserRepository>(); // this is the mock version of userService with the test purpose
            userServiceMock.Setup(service => service.DeleteUserAsync(id)).ReturnsAsync(user);
            var controller = new UsersController(userServiceMock.Object);
            
            //act
            var result = await controller.Delete(id);
            
            //assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result); //check the return type
            //var model = Assert.IsAssignableFrom<User>(okObjectResult.Value); // get the returned object
            //Assert.Equal(user.UserId, model.UserId); // check then returned object
            //Assert.Equal(user.UserName, model.UserName); 
            
        }
        
    }
}*/