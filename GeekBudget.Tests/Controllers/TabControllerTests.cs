using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Controllers;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using GeekBudget.Services;
using GeekBudget.Entities;
using static GeekBudget.Entities.Enums;
using GeekBudget.Services.Implementations;
using GeekBudget.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using Xunit;

namespace GeekBudget.Tests.Controllers
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
            var validators = new Mock<ITabValidators>();
            var mapping = new MappingService();
            var controller = new TabController(service.Object, validators.Object, mapping);
            
            // Act
            var result = await controller.GetAll() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var data = result.Value as IEnumerable<TabViewModel>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
        }

        [Fact]
        public async Task Get_ReturnsCorrectFoundTab()
        {
            // Arrange
            var tab = new Tab(){Name = "testing-tab"};
            var service = new Mock<ITabService>();
            service.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsyncServiceResult(tab);
            var validators = new Mock<ITabValidators>();
            var mapping = new MappingService();
            var controller = new TabController(service.Object, validators.Object, mapping);

            // Act
            var result = await controller.Get(1) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var data = result.Value as TabViewModel;
            Assert.NotNull(data);
            Assert.Equal("testing-tab", data.Name);
        }

        [Fact]
        public async Task Get_ReturnsNotFoundForNotFoundTab()
        {
            // Arrange
            var tab = new TabViewModel(){Name = "testing-tab"};
            var service = new Mock<ITabService>();
            service.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsyncServiceResult(null);
            var validators = new Mock<ITabValidators>();
            var mapping = new MappingService();
            var controller = new TabController(service.Object, validators.Object, mapping);

            // Act
            var result = await controller.Get(1) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Add_AddsCorrectTab()
        {
            // Arrange
            var tab = new TabViewModel(){Name = "testing-tab"};
            var service = new Mock<ITabService>();
            service.Setup(x => x.Add(It.IsAny<Tab>())).ReturnsAsyncServiceResult(1);
            var validators = new Mock<ITabValidators>();
            validators.Setup(x => x.TabTypeRequired(It.IsAny<TabViewModel>())).ReturnsAsync(new Error[0]);
            var mapping = new MappingService();
            var controller = new TabController(service.Object, validators.Object, mapping);

            // Act
            var result = await controller.Add(tab) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var data = (int) result.Value;
            Assert.Equal(1, data);
        }

        [Fact]
        public async Task Add_DoesNotAddsIncorrectTab()
        {
            // Arrange
            var tab = new TabViewModel(){Name = "testing-tab"};
            var service = new Mock<ITabService>();
            service.Setup(x => x.Add(It.IsAny<Tab>())).ReturnsAsyncServiceResult(1);
            var validators = new Mock<ITabValidators>();
            validators.Setup(x => x.TabTypeRequired(It.IsAny<TabViewModel>())).ReturnsAsync(new List<Error>()
            {
                new Error(){Id = 999, Description = "testing-error"}
            });
            var mapping = new MappingService();
            var controller = new TabController(service.Object, validators.Object, mapping);

            // Act
            var result = await controller.Add(tab) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            var data = (List<Error>) result.Value;
            Assert.Single(data);
            Assert.Equal(999, data.SingleOrDefault()?.Id);
            Assert.Equal("testing-error", data.SingleOrDefault()?.Description);
        }

        [Fact]
        public async Task Remove_RemovesTab()
        {
            // Arrange
            var tab = new TabViewModel(){Id = 1};
            var service = new Mock<ITabService>();
            service.Setup(x => x.Remove(It.IsAny<int>())).ReturnsAsyncServiceResult();
            var validators = new Mock<ITabValidators>();
            validators.Setup(x => x.IdExists(It.IsAny<TabViewModel>())).ReturnsAsync(new Error[0]);
            var mapping = new MappingService();
            var controller = new TabController(service.Object, validators.Object, mapping);

            // Act
            var result = await controller.Remove(tab) as OkResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Remove_DoNotRemoveNonExistingTab()
        {
            // Arrange
            var tab = new TabViewModel(){Name = "testing-tab"};
            var service = new Mock<ITabService>();
            service.Setup(x => x.Remove(It.IsAny<int>())).ReturnsAsyncServiceResult();
            var validators = new Mock<ITabValidators>();
            validators.Setup(x => x.IdExists(It.IsAny<TabViewModel>())).ReturnsAsync(new List<Error>()
            {
                new Error(){Id = 999, Description = "testing-error"}
            });
            var mapping = new MappingService();
            var controller = new TabController(service.Object, validators.Object, mapping);

            // Act
            var result = await controller.Remove(tab) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            var data = (List<Error>) result.Value;
            Assert.Single(data);
            Assert.Equal(999, data.SingleOrDefault()?.Id);
            Assert.Equal("testing-error", data.SingleOrDefault()?.Description);
        }

        [Fact]
        public async Task Update_UpdatesCorrectTab()
        {
            // Arrange
            var tab = new TabViewModel(){Id = 1};
            var service = new Mock<ITabService>();
            service.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Tab>())).ReturnsAsyncServiceResult();
            var validators = new Mock<ITabValidators>();
            validators.Setup(x => x.IdExists(It.IsAny<TabViewModel>())).ReturnsAsync(new Error[0]);
            var mapping = new MappingService();
            var controller = new TabController(service.Object, validators.Object, mapping);

            // Act
            var result = await controller.Update(tab) as OkResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Update_DoNotUpdateIfDoesNotExist()
        {
            // Arrange
            var tab = new TabViewModel(){Name = "testing-tab"};
            var service = new Mock<ITabService>();
            service.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Tab>())).ReturnsAsyncServiceResult();
            var validators = new Mock<ITabValidators>();
            validators.Setup(x => x.IdExists(It.IsAny<TabViewModel>())).ReturnsAsync(new List<Error>()
            {
                new Error(){Id = 999, Description = "testing-error"}
            });
            var mapping = new MappingService();
            var controller = new TabController(service.Object, validators.Object, mapping);

            // Act
            var result = await controller.Remove(tab) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            var data = (List<Error>) result.Value;
            Assert.Single(data);
            Assert.Equal(999, data.SingleOrDefault()?.Id);
            Assert.Equal("testing-error", data.SingleOrDefault()?.Description);
        }
    }
}