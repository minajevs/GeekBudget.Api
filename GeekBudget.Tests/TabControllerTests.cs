using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Application.Tabs;
using GeekBudget.Application.Tabs.Requests;
using GeekBudget.Application.Tests;
using GeekBudget.Controllers;
using GeekBudget.Core;
using GeekBudget.Domain.Tabs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace GeekBudget.Tests
{
    public class TabControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnAllTabs()
        {
            // Arrange
            var tabs = new List<Tab>()
            {
                new Tab(),
                new Tab()
            };
            var service = new Mock<ITabService>();
            service.Setup(x => x.GetAll()).ReturnsAsyncServiceResult(tabs);
            var controller = new TabController(service.Object);
            
            // Act
            var result = await controller.GetAll();

            // Assert
            var data = result.Value;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
        }

        [Fact]
        public async Task Get_TabExist_ReturnsTab()
        {
            // Arrange
            var tab = new Tab(){Name = "testing-tab"};
            var service = new Mock<ITabService>();
            service.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsyncServiceResult(tab);
            var controller = new TabController(service.Object);

            // Act
            var result = await controller.Get(1);

            // Assert
            var data = result.Value;
            Assert.NotNull(data);
            Assert.Equal("testing-tab", data.Name);
        }

        [Fact]
        public async Task Get_TabDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var tab = new TabVm(){Name = "testing-tab"};
            var service = new Mock<ITabService>();
            service.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsyncServiceResult(null);
            var controller = new TabController(service.Object);

            // Act
            var result = await controller.Get(1);
            var innerResult = result.Result as NotFoundResult;

            // Assert
            Assert.Null(result.Value);
            Assert.NotNull(innerResult);
        }

        [Fact]
        public async Task Add_CorrectTab_ReturnsOk()
        {
            // Arrange
            var tab = new TabVm(){Name = "testing-tab", Type = 1};
            var service = new Mock<ITabService>();
            service.Setup(x => x.Add(It.IsAny<AddTabRequest>())).ReturnsAsyncServiceResult(1);
            var controller = new TabController(service.Object);

            // Act
            var result = await controller.Add(tab);

            // Assert
            Assert.Equal(1, result.Value);
        }

        [Fact]
        public async Task Add_IncorrectTab_ReturnsNok()
        {
            // Arrange
            var tab = new TabVm();
            var service = new Mock<ITabService>();
            service.Setup(x => x.Add(It.IsAny<AddTabRequest>())).ReturnsAsyncServiceResult(1);
            var controller = new TabController(service.Object);

            // Act
            var result = await controller.Add(tab);
            var innerResult = result.Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(innerResult);
            var errors = innerResult.Value as List<Error>;
            Assert.NotNull(errors);
            Assert.NotEmpty(errors);
        }

        [Fact]
        public async Task Remove_RemovesTab()
        {
            // Arrange
            var service = new Mock<ITabService>();
            service.Setup(x => x.Remove(It.IsAny<int>())).ReturnsAsyncServiceResult();
            var controller = new TabController(service.Object);

            // Act
            var result = await controller.Remove(1) as OkResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Update_CorrectTab_ReturnsOk()
        {
            // Arrange
            var tab = new TabVm(){Id = 1};
            var service = new Mock<ITabService>();
            service.Setup(x => x.Update(It.IsAny<UpdateTabRequest>())).ReturnsAsyncServiceResult();
            var controller = new TabController(service.Object);

            // Act
            var result = await controller.Update(1, tab) as OkResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Update_IncorrectTab_ReturnsNok()
        {
            // Arrange
            var tab = new TabVm();
            var service = new Mock<ITabService>();
            service.Setup(x => x.Update(It.IsAny<UpdateTabRequest>())).ReturnsAsyncServiceResult(ServiceResultStatus.Failure);
            var controller = new TabController(service.Object);

            // Act
            var result = await controller.Update(1, tab) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
        }
    }
}