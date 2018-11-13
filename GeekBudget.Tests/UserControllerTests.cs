using System;
using GeekBudget.Controllers;
using GeekBudget.DataAccess.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GeekBudget.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public void Add_NewUser_ReturnsOk()
        {
            //Arrange
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.Add(It.IsAny<string>())).Returns("123");

            //Act
            var controller = new UserController(userRepo.Object);

            var result = controller.Add("test-user");

            //Assert
            Assert.IsType<OkObjectResult>(result);
            var data = ((OkObjectResult)result).Value as string;
            Assert.NotNull(data);
        }

        [Fact]
        public void CantAddSameUserTwice()
        {
            //Arrange
            var userRepo = new Mock<IUserRepository>();
            userRepo.Setup(x => x.Add(It.IsAny<string>())).Throws(new InvalidOperationException());

            //Act
            var controller = new UserController(userRepo.Object);

            var result = controller.Add("test-user");

            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
