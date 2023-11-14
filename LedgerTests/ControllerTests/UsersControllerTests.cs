using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Controllers;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LedgerTests.ControllerTests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task GetAllUsers_ShouldReturnListOfUsers()
        {
            // arrange
            var userServiceMock = new Mock<UserService>();
            var controller = new UsersController(userServiceMock.Object);
            
            //act
            var result = await controller.Get();
            
            //assert
            Assert.IsType<ActionResult<IEnumerable<User>>>(result);

        }
    }
}