using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GeekBudget.Entities;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using GeekBudget.Validators.Implementations;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GeekBudget.Tests.Validators
{
    public class TabValidatorsTests : IClassFixture<TestingConnection>
    {
        private readonly TestingConnection _connection;

        public TabValidatorsTests(TestingConnection connection)
        {
            _connection = connection;
        }

        [Fact]
        public async Task IdExists_IdExists_NoErrors()
        {
            // Arrange
            var context = _connection.CreateNewContext();
            context.Tabs.Add(new Tab() {Id = 11});
            context.SaveChanges();

            var validator = new TabValidators(context);

            // Act
            var result = await validator.IdExists(new TabViewModel() {Id = 11});

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task IdExists_IdDoesNotExist_Error()
        {
            // Arrange
            var context = _connection.CreateNewContext();
            context.Tabs.Add(new Tab() { Id = 11 });
            context.SaveChanges();

            var validator = new TabValidators(context);

            // Act
            var result = await validator.IdExists(new TabViewModel() { Id = 22 });

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task TabTypeRequired_TypeIsNotNull_NoErrors()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var validator = new TabValidators(context.Object);

            // Act
            var result = await validator.TabTypeRequired(new TabViewModel() { Type = TabType.Account});

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task TabTypeRequired_TypeIsNull_Error()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var validator = new TabValidators(context.Object);

            // Act
            var result = await validator.TabTypeRequired(new TabViewModel());

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task NotNull_NotNull_NoErrors()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var validator = new TabValidators(context.Object);

            // Act
            var result = await validator.NotNull(new TabViewModel());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task NotNull_Null_Error()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var validator = new TabValidators(context.Object);

            // Act
            var result = await validator.NotNull(null);

            // Assert
            Assert.Single(result);
        }
    }
}
