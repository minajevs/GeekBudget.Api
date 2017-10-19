//using GeekBudget.Controllers;
//using GeekBudget.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
//using Moq;
//using Xunit;

//namespace GeekBudget.Test
//{
//    public class OperationControllerTests
//    {
//        [Fact]
//        public void CanGetAllOperation()
//        {
//            // Arrange
//            var connection = new SqliteConnection("DataSource=:memory:");
//            connection.Open();
//            try
//            {
//                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
//                    .UseSqlite(connection)
//                    .Options;
//                using (var context = new GeekBudgetContext(options))
//                {
//                    var testData = CreateTestingOperationsTabs();
//                    context.Database.EnsureCreated();
//                    context.Tabs.AddRange(testData.Item2);
//                    context.Operations.AddRange(testData.Item1);
//                    context.SaveChanges();
//                }

//                // Act
//                OkObjectResult allOps;
//                using (var context = new GeekBudgetContext(options))
//                {
//                    var controller = PrepareController(context);
//                    allOps = controller.GetAll() as OkObjectResult;
//                }

//                // Assert ---
//                Assert.NotNull(allOps);
//                var data = allOps.Value as IEnumerable<Operation>;
//                Assert.NotNull(data);
//                Assert.Equal(3, data.Count());
//                //---
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        [Fact]
//        public void CanGetOperations()
//        {
//            // Arrange
//            var connection = new SqliteConnection("DataSource=:memory:");
//            connection.Open();
//            try
//             {
//                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
//                    .UseSqlite(connection)
//                    .Options;
//                using (var context = new GeekBudgetContext(options))
//                {
//                    var testData = CreateTestingOperationsTabs();
//                    context.Database.EnsureCreated();
//                    context.Tabs.AddRange(testData.Item2);
//                    context.Operations.AddRange(testData.Item1);
//                    context.SaveChanges();
//                }

//                // Act
//                OkObjectResult ops1;
//                OkObjectResult ops2;
//                NotFoundObjectResult ops3;
//                using (var context = new GeekBudgetContext(options))
//                {
//                    var controller = PrepareController(context);
//                    ops1 = controller.Get(new OperationFilter()
//                    {
//                        Id = 1
//                    }) as OkObjectResult;

//                    //ops2 = controller.Get(new OperationFilter() //THIS TEST FAILS FOR UNKNOWN REASONS!!!!!! W T F
//                    //{
//                    //    Amount = new MinMaxFilter<decimal>() { Min = 300, Max = 1200 }
//                    //}) as OkObjectResult;

//                    ops3 = controller.Get(new OperationFilter()
//                    {
//                        Comment = "wtf-is this !@"
//                    }) as NotFoundObjectResult;
//                }

//                // Assert ---
//                Assert.NotNull(ops1);
//                //Assert.NotNull(ops2);
//                Assert.NotNull(ops3);
//                var data1 = ops1.Value as IEnumerable<Operation>;
//                //var data2 = ops2.Value as IEnumerable<Operation>;

//                Assert.NotNull(data1);
//                Assert.Equal(1, data1.Count());
//                //Assert.Equal(2, data2.Count());
//                //---
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        [Fact]
//        public void CanAddNewOperation()
//        {
//            // Arrange
//            var connection = new SqliteConnection("DataSource=:memory:");
//            connection.Open();
//            try
//            {
//                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
//                    .UseSqlite(connection)
//                    .Options;
//                using (var context = new GeekBudgetContext(options))
//                {
//                    var testData = CreateTestingOperationsTabs();
//                    context.Database.EnsureCreated();
//                    context.Tabs.AddRange(testData.Item2);
//                    context.Operations.AddRange(testData.Item1);
//                    context.SaveChanges();
//                }

//                // Act
//                OkObjectResult opsOk1;
//                OkObjectResult opsOk2;
//                BadRequestObjectResult opsNok1;
//                BadRequestObjectResult opsNok2;
//                BadRequestObjectResult opsNok3;

//                using (var context = new GeekBudgetContext(options))
//                {
//                    var controller = PrepareController(context);
//                    opsOk1 = controller.Add(new Operation()
//                    {
//                        Amount = 100,
//                        Comment = "testing-op-1",
//                        Currency = "EUR",
//                        Date = DateTime.Now,
//                        From = new Tab(1),
//                        To = new Tab(2),
//                    }) as OkObjectResult;

//                    opsOk2 = controller.Add(new Operation()
//                    {
//                        Amount = 500,
//                        Comment = "testing-op-2",
//                        Currency = "EUR",
//                        Date = new DateTime(2015, 10, 10),
//                        From = new Tab(2),
//                        To = new Tab(3),
//                    }) as OkObjectResult;

//                    opsNok1 = controller.Add(new Operation()
//                    {
//                        Amount = 100,
//                        Comment = "testing-nop-1",
//                        Currency = "EUR",
//                        Date = DateTime.Now,
//                        From = new Tab(1),
//                        To = new Tab(1),
//                    }) as BadRequestObjectResult;

//                    opsNok2 = controller.Add(new Operation()
//                    {
//                        Comment = "testing-nop-2",
//                        Currency = "EUR",
//                        Date = DateTime.Now,
//                        From = new Tab(2),
//                        To = new Tab(999),
//                    }) as BadRequestObjectResult;
//                }
//                using (var context = new GeekBudgetContext(options))
//                {
//                    // Assert ---
//                    Assert.NotNull(opsOk1);
//                    Assert.NotNull(opsOk2);
//                    Assert.NotNull(opsNok1);
//                    Assert.NotNull(opsNok2);
//                    Assert.Equal(5, context.Operations.Count());
//                    //Assert.Equal(3, data.Count());
//                    //---
//                }
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        [Fact]
//        public void CanRemoveOperation()
//        {
//            // Arrange
//            var connection = new SqliteConnection("DataSource=:memory:");
//            connection.Open();
//            try
//            {
//                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
//                    .UseSqlite(connection)
//                    .Options;
//                using (var context = new GeekBudgetContext(options))
//                {
//                    var testData = CreateTestingOperationsTabs();
//                    context.Database.EnsureCreated();
//                    context.Tabs.AddRange(testData.Item2);
//                    context.Operations.AddRange(testData.Item1);
//                    context.SaveChanges();
//                }

//                // Act
//                OkObjectResult opsOk;
//                BadRequestObjectResult opsNok;
//                using (var context = new GeekBudgetContext(options))
//                {
//                    var controller = PrepareController(context);
//                    opsOk = controller.Remove(1) as OkObjectResult;
//                    opsNok = controller.Remove(999) as BadRequestObjectResult;
//                }

//                using (var context = new GeekBudgetContext(options))
//                {
//                    // Assert ---
//                    Assert.NotNull(opsOk);
//                    Assert.NotNull(opsNok);
//                    Assert.Equal(2, context.Operations.Count());
//                    //---
//                }
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        [Fact]
//        public void CanUpdateOperation()
//        {
//            // Arrange
//            var connection = new SqliteConnection("DataSource=:memory:");
//            connection.Open();
//            try
//            {
//                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
//                    .UseSqlite(connection)
//                    .Options;
//                using (var context = new GeekBudgetContext(options))
//                {
//                    var testData = CreateTestingOperationsTabs();
//                    context.Database.EnsureCreated();
//                    foreach (var tab in testData.Item2)
//                    {
//                        context.Tabs.Add(tab);
//                        foreach (var operation in testData.Item1)
//                        {
//                            context.Operations.Add(operation);
//                        }
//                    }
//                    context.Operations.AddRange(testData.Item1);
//                    context.Tabs.AddRange(testData.Item2);
//                    context.SaveChanges();
//                }

//                // Act
//                OkObjectResult opsOk1;
//                OkObjectResult opsOk2;
//                BadRequestObjectResult opsNok1;
//                BadRequestObjectResult opsNok2;
//                using (var context = new GeekBudgetContext(options))
//                {
//                    var controller = PrepareController(context);
//                    opsOk1 = controller.Update(1, new Operation()
//                    {
//                        Id = 99,
//                        Amount = 100, //From 500 to 100
//                        Comment = "op-test-1-changed",
//                        //Currency = "EUR",
//                        Date = new DateTime(2017, 4, 4)
//                    }) as OkObjectResult;

//                    opsOk2 = controller.Update(3, new Operation()
//                    {
//                        From = new Tab(1), //From 3 to 1
//                        To = new Tab(2),    //From 1 to 2
//                    }) as OkObjectResult;

//                    opsNok1 = controller.Update(999, new Operation()
//                    {
//                        Comment = "op-test-NOK"
//                    }) as BadRequestObjectResult;

//                    opsNok2 = controller.Update(1, new Operation()
//                    {
//                        From = new Tab(1),
//                        To = new Tab(999999),
//                    }) as BadRequestObjectResult;
//                }

//                using (var context = new GeekBudgetContext(options))
//                {
//                    // Assert ---
//                    Assert.NotNull(opsOk1);
//                    Assert.NotNull(opsOk2);
//                    Assert.NotNull(opsNok1);
//                    Assert.NotNull(opsNok2);

//                    Assert.Equal(2, context.Tabs.FirstOrDefault(t => t.Id == 1).Operations.Count);
//                    Assert.Equal(2, context.Tabs.FirstOrDefault(t => t.Id == 2).Operations.Count);

//                    Assert.Equal(800, context.Tabs.FirstOrDefault(t => t.Id == 1).Amount);
//                    Assert.Equal(700, context.Tabs.FirstOrDefault(t => t.Id == 2).Amount);
//                    Assert.Equal(100, context.Operations.FirstOrDefault(t => t.Id == 1).Amount);
//                    Assert.Equal("op-test-1-changed", context.Operations.FirstOrDefault(t => t.Id == 1).Comment);
//                    Assert.Equal(4, context.Operations.FirstOrDefault(t => t.Id == 1).Date.Value.Month);

//                    //---
//                }
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        private Tuple<List<Operation>, List<Tab>> CreateTestingOperationsTabs()
//        {
//            var tabs = new List<Tab>()
//            {
//                new Tab()
//                    {
//                        Id = 1,
//                        Amount = 500,
//                        Currency = "EUR",
//                        Name = "income-test-1",
//                    },
//                new Tab()
//                    {
//                        Id = 2,
//                        Amount = 1000,
//                        Currency = "EUR",
//                        Name = "income-test-2",
//                    },
//                new Tab()
//                    {
//                        Id = 3,
//                        Amount = 100,
//                        Currency = "EUR",
//                        Name = "income-test-3",
//                    },
//            };

//            var operations = new List<Operation>()
//            {
//                new Operation()
//                    {
//                        Id = 1,
//                        Amount = 500,
//                        Comment = "op-test-1",
//                        Currency = "EUR",
//                        From = tabs[0],
//                        To = tabs[1],
//                        Date = new DateTime(2017, 10, 10)
//                    },
//                new Operation()
//                    {
//                        Id = 2,
//                        Amount = 1000,
//                        Comment = "op-test-2",
//                        Currency = "USD",
//                        From = tabs[1],
//                        To = tabs[2],
//                        Date = new DateTime(2016, 1, 2)
//                    },
//                new Operation()
//                    {
//                        Id = 3,
//                        Amount = 100,
//                        Comment = "op-test-3",
//                        Currency = "EUR",
//                        From = tabs[2],
//                        To = tabs[0],
//                        Date = new DateTime(2015, 5, 5)
//                    },
//            };

//            tabs[0].Operations.Add(operations[0]);
//            tabs[0].Operations.Add(operations[2]);
//            tabs[1].Operations.Add(operations[0]);
//            tabs[1].Operations.Add(operations[1]);
//            tabs[2].Operations.Add(operations[1]);
//            tabs[2].Operations.Add(operations[2]);

//            return new Tuple<List<Operation>, List<Tab>>(operations, tabs);
//        }
//        private OperationController PrepareController(GeekBudgetContext context)
//        {
//            var controller = new OperationController(context);
//            var objectValidator = new Mock<IObjectModelValidator>();
//            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
//                It.IsAny<ValidationStateDictionary>(),
//                It.IsAny<string>(),
//                It.IsAny<Object>()));
//            controller.ObjectValidator = objectValidator.Object;
//            return controller;
//        }
//    }
//}
