using GeekBudget.Controllers;
using GeekBudget.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace GeekBudget.Test
{
    public class OperationControllerTests
    {
        [Fact]
        public void CanGetAllOperation()
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
                    var testData = CreateTestingOperationsTabs();
                    context.Database.EnsureCreated();
                    context.Tabs.AddRange(testData.Item2);
                    context.Operations.AddRange(testData.Item1);
                    context.SaveChanges();
                }

                // Act
                OkObjectResult allOps;
                using (var context = new GeekBudgetContext(options))
                {
                    var controller = new OperationController(context);
                    allOps = controller.GetAll() as OkObjectResult;
                }

                // Assert ---
                Assert.NotNull(allOps);
                var data = allOps.Value as IEnumerable<Operation>;
                Assert.NotNull(data);
                Assert.Equal(3, data.Count());
                //---
            }
            finally
            {
                connection.Close();
            }
        }

        private Tuple<List<Operation>, List<Tab>> CreateTestingOperationsTabs()
        {
            var tabs = new List<Tab>()
            {
                new Tab()
                    {
                        Id = 1,
                        Amount = 500,
                        Currency = "EUR",
                        Name = "income-test-1",
                        Operations = new List<Operation>()
                    },
                new Tab()
                    {
                        Id = 2,
                        Amount = 1000,
                        Currency = "USD",
                        Name = "income-test-2",
                        Operations = new List<Operation>()
                    },
                new Tab()
                    {
                        Id = 3,
                        Amount = 100,
                        Currency = "LVL",
                        Name = "income-test-3",
                        Operations = new List<Operation>()
                    },
            };

            var operations = new List<Operation>()
            {
                new Operation()
                    {
                        Id = 1,
                        Amount = 100,
                        Comment = "op-test-1",
                        Currency = "EUR",
                        From = 1,
                        To = 2,
                    },
                new Operation()
                    {
                        Id = 2,
                        Amount = 1000,
                        Comment = "op-test-2",
                        Currency = "EUR",
                        From = 2,
                        To = 3,
                    },
                new Operation()
                    {
                        Id = 3,
                        Amount = 1,
                        Comment = "op-test-3",
                        Currency = "EUR",
                        From = 3,
                        To = 1,
                    },
            };

            tabs[0].Operations.Add(operations[0]);
            tabs[0].Operations.Add(operations[2]);
            tabs[1].Operations.Add(operations[0]);
            tabs[1].Operations.Add(operations[1]);
            tabs[2].Operations.Add(operations[1]);
            tabs[2].Operations.Add(operations[2]);

            return new Tuple<List<Operation>, List<Tab>>(operations, tabs);
        }
    }
}
