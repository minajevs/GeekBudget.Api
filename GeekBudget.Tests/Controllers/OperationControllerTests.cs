using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Controllers;
using GeekBudget.Entities;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using GeekBudget.Services;
using GeekBudget.Services.Implementations;
using GeekBudget.Validators;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Operation = GeekBudget.Models.Operation;

namespace GeekBudget.Tests.Controllers
{
    public class OperationControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsAllOperations()
        {
            //Arrange
            var operations = new List<Operation>()
            {
                new Operation(){To = new Tab(){Id = 1}, From = new Tab(){Id = 2}},
                new Operation(){To = new Tab(){Id = 2}, From = new Tab(){Id = 1}},
            };
            var service = new Mock<IOperationService>();
            service.Setup(x => x.GetAll()).ReturnsAsyncServiceResult(operations);
            var validators = new Mock<IOperationValidators>();
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.GetAll();

            //Assert
            var data = result.Value;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
        }

        [Fact]
        public async Task Get_ReturnsFilteredOperations()
        {
            //Arrange
            var operations = new List<Operation>()
            {
                new Operation(){To = new Tab(){Id = 1}, From = new Tab(){Id = 2}},
                new Operation(){To = new Tab(){Id = 2}, From = new Tab(){Id = 1}},
            };
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Get(It.IsAny<OperationFilter>())).ReturnsAsyncServiceResult(operations);
            var validators = new Mock<IOperationValidators>();
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.Get(new OperationFilter());

            //Assert
            var data = result.Value;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
        }

        [Fact]
        public async Task Get_ReturnsNotFoundIfNoOperations()
        {
            //Arrange
            var operations = new List<Operation>();
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Get(It.IsAny<OperationFilter>())).ReturnsAsyncServiceResult(operations);
            var validators = new Mock<IOperationValidators>();
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.Get(new OperationFilter());

            //Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Add_OperationAdded()
        {
            //Arrange
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Add(It.IsAny<Operation>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsyncServiceResult(1);
            var validators = new Mock<IOperationValidators>();
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.Add(new OperationViewModel());

            //Assert
            Assert.Equal(1, result.Value);
        }

        [Fact]
        public async Task Add_DoesNotAddIncorrectOperation()
        {
            //Arrange
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Add(It.IsAny<Operation>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsyncServiceResult(1);
            var validators = new Mock<IOperationValidators>();
            validators.Setup(x => x.NotNull(It.IsAny<OperationViewModel>())).ReturnsAsync(new List<Error>()
            {
                new Error() {Id = 999, Description = "testing-error"}
            });
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.Add(new OperationViewModel());
            var innerResult = result.Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(innerResult);
            var errors = innerResult.Value as List<Error>;
            Assert.NotNull(errors);
            Assert.Single(errors);
            Assert.Equal(999, errors.SingleOrDefault()?.Id);
            Assert.Equal("testing-error", errors.SingleOrDefault()?.Description);
        }

        [Fact]
        public async Task Remove_RemovesOperation()
        {
            //Arrange
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Remove(It.IsAny<int>())).ReturnsAsyncServiceResult();
            var validators = new Mock<IOperationValidators>();
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.Remove(1) as OkResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Update_UdpdatesCorrectOperation()
        {
            //Arrange
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Operation>(), It.IsAny<OperationViewModel>())).ReturnsAsyncServiceResult();
            var validators = new Mock<IOperationValidators>();
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.Update(new OperationViewModel()) as OkResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Update_DoesNotUpdateIncorrectOperation()
        {
            //Arrange
            var service = new Mock<IOperationService>();
            var validators = new Mock<IOperationValidators>();
            validators.Setup(x => x.NotNull(It.IsAny<OperationViewModel>())).ReturnsAsync(new List<Error>()
            {
                new Error() {Id = 999, Description = "testing-error"}
            });
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.Update(new OperationViewModel()) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(result);
        }
    }
}