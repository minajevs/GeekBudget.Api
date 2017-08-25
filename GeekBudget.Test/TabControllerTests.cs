using GeekBudget.Controllers;
using GeekBudget.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GeekBudget.Test
{
    public class TabControllerTests
    {
        [Fact]
        public void CanGetAllTabs()
        {
            // Arrange
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            try
            {
                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
                    .UseSqlite(connection)
                    .Options;
                using (var context = new GeekBudgetContext(options))
                {
                    context.Database.EnsureCreated();
                    context.Tabs.AddRange(CreateTestingTabs());
                    context.SaveChanges();
                }

                // Act
                OkObjectResult allTabs;
                using (var context = new GeekBudgetContext(options))
                {
                    var controller = new TabController(context);
                    allTabs = controller.GetAll() as OkObjectResult;
                }

                // Assert ---
                Assert.NotNull(allTabs);
                var data = allTabs.Value as IEnumerable<Tab>;
                Assert.NotNull(data);
                Assert.Equal(3, data.Count());
                //---
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public void CanGetById()
        {
            // Arrange
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            try
            {
                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
                    .UseSqlite(connection)
                    .Options;
                using (var context = new GeekBudgetContext(options))
                {
                    context.Database.EnsureCreated();
                    context.Tabs.AddRange(CreateTestingTabs());
                    context.SaveChanges();
                }

                // Act
                OkObjectResult tab;
                NotFoundObjectResult tabNotFound;
                using (var context = new GeekBudgetContext(options))
                {
                    var controller = new TabController(context);
                    tab = controller.Get(2) as OkObjectResult;
                    tabNotFound = controller.Get(999) as NotFoundObjectResult;
                }

                // Assert ---
                Assert.NotNull(tab.Value); //Tab is found
                Assert.Equal((tab.Value as Tab).Name, "income-test-2"); //Correct tab is found
                Assert.NotNull(tabNotFound); //Wrong tab not found
                //---
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public void CanAddTab()
        {
            // Arrange
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            try
            {
                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
                    .UseSqlite(connection)
                    .Options;
                using (var context = new GeekBudgetContext(options))
                {
                    context.Database.EnsureCreated();
                    context.Tabs.AddRange(CreateTestingTabs());
                    context.SaveChanges();
                }

                // Act
                OkObjectResult successResult;
                BadRequestObjectResult badRequestResult;
                using (var context = new GeekBudgetContext(options))
                {
                    var controller = PrepareController(context);
                    successResult = controller.Add(new Tab() { Name = "testing2-action" }) as OkObjectResult;
                    controller.ModelState.AddModelError("ModelValidation", "TestingError");
                    badRequestResult = controller.Add(new Tab() { }) as BadRequestObjectResult;
                }

                // Assert ---
                using (var context = new GeekBudgetContext(options))
                {
                    Assert.NotNull(badRequestResult); //Got wrong result
                    Assert.NotNull(successResult);  //Got correct result
                    Assert.Equal(400, badRequestResult.StatusCode); //Wrong result has correct RC
                    Assert.Equal(200, successResult.StatusCode); //Successful result has correct RC
                    Assert.True(context.Tabs.Any(t => t.Name == "testing2-action"));
                    Assert.Equal(4, context.Tabs.Count());
                }
                //---
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public void CanRemoveTab()
        {
            // Arrange
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            try
            {
                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
                    .UseSqlite(connection)
                    .Options;
                using (var context = new GeekBudgetContext(options))
                {
                    context.Database.EnsureCreated();
                    context.Tabs.AddRange(CreateTestingTabs());
                    context.SaveChanges();
                }

                // Act
                OkObjectResult successResult;
                BadRequestObjectResult badRequestResult;
                using (var context = new GeekBudgetContext(options))
                {
                    var controller = PrepareController(context);
                    successResult = controller.Remove(1) as OkObjectResult;
                    badRequestResult = controller.Remove(999) as BadRequestObjectResult;
                }

                // Assert ---
                using (var context = new GeekBudgetContext(options))
                {
                    Assert.NotNull(badRequestResult); //Got wrong result
                    Assert.NotNull(successResult);  //Got correct result
                    Assert.Equal(400, badRequestResult.StatusCode); //Wrong result has correct RC
                    Assert.NotNull(badRequestResult.Value); //Wrong result has return message
                    Assert.Equal(200, successResult.StatusCode); //Successful result has correct RC
                    Assert.Equal(2, context.Tabs.Count()); //1 tab is removed
                    Assert.False(context.Tabs.Any(t => t.Name == "income-test-1")); //Correct tab is removed
                }
                //---
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public void CanUpdateTab()
        {
            // Arrange
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            try
            {
                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
                    .UseSqlite(connection)
                    .Options;
                using (var context = new GeekBudgetContext(options))
                {
                    context.Database.EnsureCreated();
                    context.Tabs.AddRange(CreateTestingTabs());
                    context.SaveChanges();
                }

                // Act
                OkObjectResult successResult1;
                OkObjectResult successResult2;
                BadRequestObjectResult badRequestResult1;
                BadRequestObjectResult badRequestResult2;
                BadRequestObjectResult badRequestResult3;
                using (var context = new GeekBudgetContext(options))
                {
                    var controller = PrepareController(context);
                    successResult1 = controller.Update(1, new Tab() { Name = "name-changed-1" }) as OkObjectResult;
                    successResult2 = controller.Update(2, new Tab() { Id = 999, Name = "name-changed-2", Amount = 333, Currency = "SEK" }) as OkObjectResult;
                    badRequestResult1 = controller.Update(999, new Tab() { Name = "name-changed-999" }) as BadRequestObjectResult;
                    badRequestResult2 = controller.Update(1, null) as BadRequestObjectResult;
                    controller.ModelState.AddModelError("ModelValidation", "TestingError");
                    badRequestResult3 = controller.Update(1, new Tab() { Name = "name-changed-1-wrong-request" }) as BadRequestObjectResult;
                }

                // Assert ---
                using (var context = new GeekBudgetContext(options))
                {
                    Assert.NotNull(badRequestResult1); //Got wrong result
                    Assert.NotNull(badRequestResult2); //Got wrong result
                    Assert.NotNull(badRequestResult3); //Got wrong result
                    Assert.NotNull(successResult1);  //Got correct result
                    Assert.NotNull(successResult2);  //Got correct result
                    Assert.NotNull(badRequestResult1.Value); //Wrong result has return message
                    Assert.NotNull(badRequestResult2.Value); //Wrong result has return message
                    Assert.NotNull(badRequestResult3.Value); //Wrong result has return message

                    Assert.True(context.Tabs.FirstOrDefault(t => t.Id == 1).Name == "name-changed-1"); //Name is updated
                    Assert.True(context.Tabs.FirstOrDefault(t => t.Id == 1).Amount == 500); //Amount is NOT updated
                    Assert.True(context.Tabs.FirstOrDefault(t => t.Id == 1).Currency == "EUR"); //Currency is NOT updated

                    Assert.True(context.Tabs.FirstOrDefault(t => t.Id == 2).Name == "name-changed-2"); //Name is updated
                    Assert.True(context.Tabs.FirstOrDefault(t => t.Id == 2).Amount == 333); //Amount is updated
                    Assert.True(context.Tabs.FirstOrDefault(t => t.Id == 2).Currency == "SEK"); //Currency is updated
                }
                //---
            }
            finally
            {
                connection.Close();
            }
        }

        private TabController PrepareController(GeekBudgetContext context)
        {
            var controller = new TabController(context);
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                              It.IsAny<ValidationStateDictionary>(),
                                              It.IsAny<string>(),
                                              It.IsAny<Object>()));
            controller.ObjectValidator = objectValidator.Object;
            return controller;
        }

        private List<Tab> CreateTestingTabs()
        {
            return new List<Tab>()
            {
                new Tab()
                    {
                        Id = 1,
                        Amount = 500,
                        Currency = "EUR",
                        Name = "income-test-1"
                    },
                new Tab()
                    {
                        Id = 2,
                        Amount = 1000,
                        Currency = "USD",
                        Name = "income-test-2"
                    },
                new Tab()
                    {
                        Id = 3,
                        Amount = 100,
                        Currency = "LVL",
                        Name = "income-test-3"
                    },
            };
        }
    }
}
