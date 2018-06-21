using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using GeekBudget.Validators;
using GeekBudget.Validators.Implementations;
using Moq;
using Xunit;

namespace GeekBudget.Tests.Validators
{
    public class OperationValidatorsTests : IClassFixture<TestingConnection>
    {
        private readonly TestingConnection _connection;

        public OperationValidatorsTests(TestingConnection connection)
        {
            _connection = connection;
        }

        [Fact]
        public async Task IdExists_IdExists_NoErrors()
        {
            // Arrange
            var context = _connection.CreateNewContext();
            context.Operations.Add(new Operation() { Id = 11 });
            context.SaveChanges();

            var tabValidator = new Mock<ITabValidators>();

            var validator = new OperationValidators(context, tabValidator.Object);

            // Act
            var result = await validator.IdExists(new OperationViewModel() { Id = 11 });

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task IdExists_IdDoesNotExist_Error()
        {
            // Arrange
            var context = _connection.CreateNewContext();
            context.Operations.Add(new Operation() { Id = 11 });
            context.SaveChanges();

            var tabValidator = new Mock<ITabValidators>();

            var validator = new OperationValidators(context, tabValidator.Object);

            // Act
            var result = await validator.IdExists(new OperationViewModel() { Id = 22 });

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task FromNotNull_FromNotNull_NoErrors()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var tabValidator = new Mock<ITabValidators>();

            var validator = new OperationValidators(context.Object, tabValidator.Object);

            // Act
            var result = await validator.FromNotNull(new OperationViewModel() { From = 1});

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task FromNotNull_FromNull_Error()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var tabValidator = new Mock<ITabValidators>();

            var validator = new OperationValidators(context.Object, tabValidator.Object);

            // Act
            var result = await validator.FromNotNull(new OperationViewModel());

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task ToNotNull_ToNotNull_NoErrors()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var tabValidator = new Mock<ITabValidators>();

            var validator = new OperationValidators(context.Object, tabValidator.Object);

            // Act
            var result = await validator.ToNotNull(new OperationViewModel() { To = 1 });

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task ToNotNull_ToNull_Error()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var tabValidator = new Mock<ITabValidators>();

            var validator = new OperationValidators(context.Object, tabValidator.Object);

            // Act
            var result = await validator.ToNotNull(new OperationViewModel());

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task FromAndToAreNotEqual_FromAndToAreNotEqual_NoErrors()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var tabValidator = new Mock<ITabValidators>();

            var validator = new OperationValidators(context.Object, tabValidator.Object);

            // Act
            var result = await validator.FromAndToAreNotEqual(new OperationViewModel() {From = 1, To = 2 });

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task FromAndToAreNotEqual_FromAndToAreEqual_Error()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var tabValidator = new Mock<ITabValidators>();

            var validator = new OperationValidators(context.Object, tabValidator.Object);

            // Act
            var result = await validator.FromAndToAreNotEqual(new OperationViewModel() {From = 1, To = 1});

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task FromAndToAreNotEqual_OneIsNull_NoErrors()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var tabValidator = new Mock<ITabValidators>();

            var validator = new OperationValidators(context.Object, tabValidator.Object);

            // Act
            var result = await validator.FromAndToAreNotEqual(new OperationViewModel() { To = 1 });

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task FromTabExists_FromTabExists_NoErrors()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var tabValidator = new Mock<ITabValidators>();
            tabValidator.Setup(x => x.IdExists(It.IsAny<TabViewModel>())).ReturnsAsync(new List<Error>());

            var validator = new OperationValidators(context.Object, tabValidator.Object);

            // Act
            var result = await validator.FromTabExists(new OperationViewModel() { From = 11 });

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task FromTabExists_FromTabDoesNotExist_Error()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var tabValidator = new Mock<ITabValidators>();
            tabValidator.Setup(x => x.IdExists(It.IsAny<TabViewModel>())).ReturnsAsync(new List<Error>(){new Error()});

            var validator = new OperationValidators(context.Object, tabValidator.Object);

            // Act
            var result = await validator.FromTabExists(new OperationViewModel() { From = 11 });

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task ToTabExists_ToTabExists_NoErrors()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var tabValidator = new Mock<ITabValidators>();
            tabValidator.Setup(x => x.IdExists(It.IsAny<TabViewModel>())).ReturnsAsync(new List<Error>());

            var validator = new OperationValidators(context.Object, tabValidator.Object);

            // Act
            var result = await validator.ToTabExists(new OperationViewModel() { To = 11 });

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task ToTabExists_ToTabDoesNotExist_Error()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var tabValidator = new Mock<ITabValidators>();
            tabValidator.Setup(x => x.IdExists(It.IsAny<TabViewModel>())).ReturnsAsync(new List<Error>() { new Error() });

            var validator = new OperationValidators(context.Object, tabValidator.Object);

            // Act
            var result = await validator.ToTabExists(new OperationViewModel() { To = 11 });

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task NotNull_NotNull_NoErrors()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var tabValidator = new Mock<ITabValidators>();

            var validator = new OperationValidators(context.Object, tabValidator.Object);

            // Act
            var result = await validator.NotNull(new OperationViewModel());

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task NotNull_NotNull_Error()
        {
            // Arrange
            var context = new Mock<IGeekBudgetContext>();
            var tabValidator = new Mock<ITabValidators>();

            var validator = new OperationValidators(context.Object, tabValidator.Object);

            // Act
            var result = await validator.NotNull(null);

            // Assert
            Assert.Single(result);
        }

    }
}
