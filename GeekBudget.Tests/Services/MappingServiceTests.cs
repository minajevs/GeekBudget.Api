using GeekBudget.Entities;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using GeekBudget.Services.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GeekBudget.Tests.Services
{
    public class MappingServiceTests
    {
        [Fact]
        public void Map_Tab_Vm()
        {
            // Arrange
            var tab = new Tab()
            {
                Id = 1,
                Name = "test-name",
                Amount = 10,
                Currency = "test-currency",
                Type = TabType.Income
            };
            var service = new MappingService();

            // Act
            var vm = service.Map(tab);

            // Assert
            Assert.Equal(1, vm.Id);
            Assert.Equal("test-name", vm.Name);
            Assert.Equal(10, vm.Amount);
            Assert.Equal("test-currency", vm.Currency);
            Assert.Equal(TabType.Income, vm.Type);
        }

        [Fact]
        public void Map_Vm_Tab()
        {
            // Arrange
            var vm = new TabViewModel()
            {
                Name = "test-name",
                Amount = 10,
                Currency = "test-currency",
                Type = TabType.Income
            };
            var service = new MappingService();

            // Act
            var tab = service.Map(vm);

            // Assert
            Assert.Equal("test-name", tab.Name);
            Assert.Equal(10, tab.Amount);
            Assert.Equal("test-currency", tab.Currency);
            Assert.Equal(TabType.Income, tab.Type);
        }

        [Fact]
        public void Map_Operation_Vm()
        {
            // Arrange
            var operation = new Operation()
            {
                Id = 1,
                Comment = "test-comment",
                Amount = 10,
                Currency = "test-currency",
                From = new Tab() { Id = 1},
                To = new Tab() { Id = 2},
                Date = new DateTime(2018, 1, 23)
            };
            var service = new MappingService();

            // Act
            var vm = service.Map(operation);

            // Assert
            Assert.Equal(1, vm.Id);
            Assert.Equal("test-comment", vm.Comment);
            Assert.Equal(10, vm.Amount);
            Assert.Equal("test-currency", vm.Currency);
            Assert.Equal(1, vm.From);
            Assert.Equal(2, vm.To);
            Assert.Equal(new DateTime(2018, 1, 23), vm.Date);
        }

        [Fact]
        public void Map_Vm_Operation()
        {
            // Arrange
            var vm = new OperationViewModel()
            {
                Comment = "test-comment",
                Amount = 10,
                Currency = "test-currency",
                Date = new DateTime(2018, 1, 23)
            };
            var service = new MappingService();

            // Act
            var operation = service.Map(vm);

            // Assert
            Assert.Equal("test-comment", operation.Comment);
            Assert.Equal(10, operation.Amount);
            Assert.Equal("test-currency", operation.Currency);
            Assert.Equal(new DateTime(2018, 1, 23), operation.Date);
        }
    }
}
