using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeekBudget.Controllers;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using GeekBudget.Test;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace GeekBudget.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public void CanAddNewUser()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = new UserController(context);

                var result = controller.Add("test-user");

                //Assert
                Assert.IsType<OkObjectResult>(result);
                var data = ((OkObjectResult)result).Value as string;
                Assert.NotNull(data);
            }
        }

        [Fact]
        public void CantAddSameUserTwice()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                context.Users.Add(new User() {Id = 1, Key = "test-key", Username = "test-user"});
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = new UserController(context);

                var result = controller.Add("test-user");

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }
    }
}
