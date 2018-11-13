using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Application.Operations;
using GeekBudget.Application.Operations.Requests;
using GeekBudget.Application.Tests;
using GeekBudget.Controllers;
using GeekBudget.Core;
using GeekBudget.Domain.Operations;
using GeekBudget.Domain.Tabs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GeekBudget.Tests
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
            var controller = new OperationController(service.Object);

            //Act
            var result = await controller.GetAll();

            //Assert
            var data = result.Value;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
        }

        [Fact]
        public async Task Get_OperationsExists_ReturnsOperations()
        {
            //Arrange
            var operations = new List<Operation>()
            {
                new Operation(){To = new Tab(){Id = 1}, From = new Tab(){Id = 2}},
                new Operation(){To = new Tab(){Id = 2}, From = new Tab(){Id = 1}},
            };
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Get(It.IsAny<OperationFilter>())).ReturnsAsyncServiceResult(operations);
            var controller = new OperationController(service.Object);

            //Act
            var result = await controller.Get(new OperationFilter());

            //Assert
            var data = result.Value;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
        }

        [Fact]
        public async Task Get_OperationsDoesNotExists_ReturnsNotFound()
        {
            //Arrange
            var operations = new List<Operation>();
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Get(It.IsAny<OperationFilter>())).ReturnsAsyncServiceResult(operations);
            var controller = new OperationController(service.Object);

            //Act
            var result = await controller.Get(new OperationFilter());

            //Assert
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Add_CorrectOperation_ReturnsOk()
        {
            //Arrange
            var operation = new OperationVm()
            {
                From = 1,
                To = 2,
                Amount = 10
            };
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Add(It.IsAny<AddOperationRequest>())).ReturnsAsyncServiceResult(1);
            var controller = new OperationController(service.Object);

            //Act
            var result = await controller.Add(operation);

            //Assert
            Assert.Equal(1, result.Value);
        }

        [Fact]
        public async Task Add_IncorrectOperation_ReturnsNok()
        {
            //Arrange
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Add(It.IsAny<AddOperationRequest>())).ReturnsAsyncServiceResult(1);
            var controller = new OperationController(service.Object);

            //Act
            var result = await controller.Add(new OperationVm());
            var innerResult = result.Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(innerResult);
            var errors = innerResult.Value as List<Error>;
            Assert.NotNull(errors);
            Assert.NotEmpty(errors);
        }

        [Fact]
        public async Task Remove_RemovesOperation()
        {
            //Arrange
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Remove(It.IsAny<int>())).ReturnsAsyncServiceResult();
            var controller = new OperationController(service.Object);

            //Act
            var result = await controller.Remove(1) as OkResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Update_CorrectOperation_ReturnsOk()
        {
            //Arrange
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Update(It.IsAny<UpdateOperationRequest>())).ReturnsAsyncServiceResult();
            var controller = new OperationController(service.Object);

            //Act
            var result = await controller.Update(1, new OperationVm()) as OkResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Update_IncorrectOperation_ReturnsNok()
        {
            //Arrange
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Update(It.IsAny<UpdateOperationRequest>()))
                .ReturnsAsyncServiceResult(ServiceResultStatus.Failure);
            var controller = new OperationController(service.Object);

            //Act
            var result = await controller.Update(1, new OperationVm()) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(result);
        }
    }
}