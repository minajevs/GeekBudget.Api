using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Application.Tabs;
using GeekBudget.DataAccess.Tests;
using GeekBudget.Domain.Tabs;
using GeekBudget.Tests;
using Xunit;

namespace GeekBudget.Application.Tests
{
    public class TabServiceTests : IClassFixture<TestingConnection>
    {
        private readonly TestingConnection _connection;

        public TabServiceTests(TestingConnection connection)
        {
            _connection = connection;
        }

        [Fact]
        public async Task GetAll_ReturnAll()
        {
            // Arrange
            var context = _connection.CreateNewContext();
            context.Tabs.Add(new Tab() {Id = 1});
            context.Tabs.Add(new Tab() {Id = 2});
            context.SaveChanges();
            var service = new TabService(context);

            // Act
            var result = await service.GetAll();

            // Assert
            Assert.Equal(2, result.Data.Count());
        }

        [Fact]
        public async Task Get_ExistingTab_ReturnsTab()
        {
            // Arrange
            var context = _connection.CreateNewContext();
            context.Tabs.Add(new Tab() { Id = 1 });
            context.SaveChanges();
            var service = new TabService(context);

            // Act
            var result = await service.Get(1);

            // Assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task Get_NonExistingTab_ReturnsNull()
        {
            // Arrange
            var context = _connection.CreateNewContext();
            context.Tabs.Add(new Tab() { Id = 1 });
            context.SaveChanges();
            var service = new TabService(context);

            // Act
            var result = await service.Get(9);

            // Assert
            Assert.Null(result.Data);
        }
    }
}
