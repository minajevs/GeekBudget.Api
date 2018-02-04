using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using Xunit;

namespace GeekBudget.Tests.Models.ViewModels
{
    public class TabViewModelTests
    {
        [Fact]
        public void CanMapToEntity()
        {
            //Arrange
            var viewModel = new TabViewModel()
            {
                Id = 1,
                Name = "tab-name-1",
                Amount = 100,
                Currency = "USD"
            };

            //Act
            var tab = viewModel.MapToEntity();

            //Assert
            Assert.Equal("tab-name-1", tab.Name);
            Assert.Equal(100, tab.Amount);
            Assert.Equal("USD", tab.Currency);
        }

        [Fact]
        public void CanMapFromEntity()
        {
            //Arrange
            var tab = new Tab()
            {
                Id = 1,
                Name = "tab-name-1",
                Amount = 100,
                Currency = "USD"
            };

            //Act
            var viewModel = new TabViewModel().MapFromEntity(tab);

            //Assert
            Assert.Equal(1, viewModel.Id);
            Assert.Equal("tab-name-1", viewModel.Name);
            Assert.Equal(100, viewModel.Amount);
            Assert.Equal("USD", viewModel.Currency);
        }

        [Fact]
        public void CanGetFromEntity()
        {
            //Arrange
            var tab = new Tab()
            {
                Id = 1,
                Name = "tab-name-1",
                Amount = 100,
                Currency = "USD"
            };

            //Act
            var viewModel = TabViewModel.FromEntity(tab);

            //Assert
            Assert.Equal(1, viewModel.Id);
            Assert.Equal("tab-name-1", viewModel.Name);
            Assert.Equal(100, viewModel.Amount);
            Assert.Equal("USD", viewModel.Currency);
        }
    }
}
