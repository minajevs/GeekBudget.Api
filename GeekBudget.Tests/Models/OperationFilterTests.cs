using System;
using System.Collections.Generic;
using System.Linq;
using GeekBudget.Models;
using Xunit;

namespace GeekBudget.Tests.Models
{
    public class OperationFilterTests
    {
        [Fact]
        public void CanFilterById()
        {
            //Arrange
            var data = CreateTestingOperations();
            var filterOk = new OperationFilter() { Id = 2 };
            var filterNok = new OperationFilter() { Id = 999 };

            //Act
            var queryOk = filterOk.CreateQuery(data);
            var queryNok = filterNok.CreateQuery(data);

            var filteredDataOk = queryOk.ToList();
            var filteredDataNok = queryNok.ToList();

            //Assert
            Assert.Equal(1, filteredDataOk.Count());
            Assert.Equal(0, filteredDataNok.Count());
            Assert.Equal(2, filteredDataOk.FirstOrDefault().Id);
            Assert.Equal("op-test-2", filteredDataOk.FirstOrDefault().Comment);
        }

        [Fact]
        public void CanFilterByComment()
        {
            //Arrange
            var data = CreateTestingOperations();
            var filterOk1 = new OperationFilter() { Comment = "op-test-3" };
            var filterOk2 = new OperationFilter() { Comment = "TEST" };
            var filterNok = new OperationFilter() { Comment = "wtf-is this!" };

            //Act
            var queryOk1 = filterOk1.CreateQuery(data);
            var queryOk2 = filterOk2.CreateQuery(data);
            var queryNok = filterNok.CreateQuery(data);

            var filteredDataOk1 = queryOk1.ToList();
            var filteredDataOk2 = queryOk2.ToList();
            var filteredDataNok = queryNok.ToList();

            //Assert
            Assert.Equal(1, filteredDataOk1.Count());
            Assert.Equal(3, filteredDataOk2.Count());
            Assert.Equal(0, filteredDataNok.Count());

            Assert.Equal(3, filteredDataOk1.FirstOrDefault().Id);
            Assert.Equal("op-test-1", filteredDataOk2[0].Comment);
            Assert.Equal("op-test-2", filteredDataOk2[1].Comment);
            Assert.Equal("op-test-3", filteredDataOk2[2].Comment);
        }

        [Fact]
        public void CanFilterByCurrency()
        {
            //Arrange
            var data = CreateTestingOperations();
            var filterOk = new OperationFilter() { Currency = "USD" }; 
            var filterNok = new OperationFilter() { Currency = "DOLLARIDOOS" };

            //Act
            var queryOk = filterOk.CreateQuery(data);
            var queryNok = filterNok.CreateQuery(data);

            var filteredDataOk = queryOk.ToList();
            var filteredDataNok = queryNok.ToList();

            //Assert
            Assert.Equal(1, filteredDataOk.Count());
            Assert.Equal(0, filteredDataNok.Count());

            Assert.Equal(2, filteredDataOk.FirstOrDefault().Id);
        }


        [Fact]
        public void CanFilterByAmount()
        {
            //Arrange
            var data = CreateTestingOperations();
            var filterOk1 = new OperationFilter() { Amount = 
                new MinMaxFilter<decimal>() {
                    Min = 200, Max = 700
                }};
            var filterOk2 = new OperationFilter() { Amount = 
                new MinMaxFilter<decimal>() {
                    Min = 1000, Max = 1000
                }};
            var filterOk3 = new OperationFilter() { Amount = 
                new MinMaxFilter<decimal>() {
                    Min = 0, Max = 1000
                }};
            var filterNok1 = new OperationFilter() { Amount = 
                new MinMaxFilter<decimal>() {
                    Min = 5000, Max = 10000
                }};
            var filterNok2 = new OperationFilter() { Amount =
                new MinMaxFilter<decimal>(){
                    Min = 1000,
                    Max = 0
                }};

            //Act
            var queryOk1 = filterOk1.CreateQuery(data);
            var queryOk2 = filterOk2.CreateQuery(data);
            var queryOk3 = filterOk3.CreateQuery(data);
            var queryNok1 = filterNok1.CreateQuery(data);
            var queryNok2 = filterNok2.CreateQuery(data);

            var filteredDataOk1 = queryOk1.ToList();
            var filteredDataOk2 = queryOk2.ToList();
            var filteredDataOk3 = queryOk3.ToList();
            var filteredDataNok1 = queryNok1.ToList();
            var filteredDataNok2 = queryNok2.ToList();

            //Assert
            Assert.Equal(1, filteredDataOk1.Count());
            Assert.Equal(1, filteredDataOk2.Count());
            Assert.Equal(3, filteredDataOk3.Count());

            Assert.Equal(0, filteredDataNok1.Count());
            Assert.Equal(0, filteredDataNok2.Count());
        }

        [Fact]
        public void CanFilterByFrom()
        {
            //Arrange
            var data = CreateTestingOperations();
            var filterOk = new OperationFilter() { From = 1 };
            var filterNok = new OperationFilter() { From = 5 };

            //Act
            var queryOk = filterOk.CreateQuery(data);
            var queryNok = filterNok.CreateQuery(data);

            var filteredDataOk = queryOk.ToList();
            var filteredDataNok = queryNok.ToList();

            //Assert
            Assert.Equal(1, filteredDataOk.Count());
            Assert.Equal(0, filteredDataNok.Count());

            Assert.Equal(1, filteredDataOk.FirstOrDefault().Id);
        }

        [Fact]
        public void CanFilterByTo()
        {
            //Arrange
            var data = CreateTestingOperations();
            var filterOk = new OperationFilter() { To = 1 };
            var filterNok = new OperationFilter() { To = 5 };

            //Act
            var queryOk = filterOk.CreateQuery(data);
            var queryNok = filterNok.CreateQuery(data);

            var filteredDataOk = queryOk.ToList();
            var filteredDataNok = queryNok.ToList();

            //Assert
            Assert.Equal(1, filteredDataOk.Count());
            Assert.Equal(0, filteredDataNok.Count());

            Assert.Equal(3, filteredDataOk.FirstOrDefault().Id);
        }

        [Fact]
        public void CanFilterByDate()
        {
            //Arrange
            var data = CreateTestingOperations();
            var filterOk1 = new OperationFilter()
            {
                Date = new MinMaxFilter<DateTime>()
                {
                    Min = new DateTime(2017, 1, 1),
                    Max = new DateTime(2018, 1, 1)
                }
            };
            var filterOk2 = new OperationFilter() {
                Date = new MinMaxFilter<DateTime>()
                {
                    Min = new DateTime(2015, 5, 5),
                    Max = new DateTime(2015, 5, 5)
                }
            };
            var filterOk3 = new OperationFilter() {
                Date = new MinMaxFilter<DateTime>()
                {
                    Min = new DateTime(2014, 1, 1),
                    Max = new DateTime(2018, 1, 1)
                }
            };
            var filterNok1 = new OperationFilter() {
                Date = new MinMaxFilter<DateTime>()
                {
                    Min = new DateTime(2018, 1, 1),
                    Max = new DateTime(2017, 1, 1)
                }
            };
            var filterNok2 = new OperationFilter()
            {
                Date = new MinMaxFilter<DateTime>()
                {
                    Min = new DateTime(2019, 1, 1),
                    Max = new DateTime(2020, 1, 1)
                }
            };

            //Act
            var queryOk1 = filterOk1.CreateQuery(data);
            var queryOk2 = filterOk2.CreateQuery(data);
            var queryOk3 = filterOk3.CreateQuery(data);
            var queryNok1 = filterNok1.CreateQuery(data);
            var queryNok2 = filterNok2.CreateQuery(data);

            var filteredDataOk1 = queryOk1.ToList();
            var filteredDataOk2 = queryOk2.ToList();
            var filteredDataOk3 = queryOk3.ToList();
            var filteredDataNok1 = queryNok1.ToList();
            var filteredDataNok2 = queryNok2.ToList();

            //Assert
            Assert.Equal(1, filteredDataOk1.Count());
            Assert.Equal(1, filteredDataOk2.Count());
            Assert.Equal(3, filteredDataOk3.Count());

            Assert.Equal(0, filteredDataNok1.Count());
            Assert.Equal(0, filteredDataNok2.Count());
        }

        [Fact]
        public void CanFilterByAllAtOnce()
        {
            //Arrange
            var data = CreateTestingOperations();
            var filterOk = new OperationFilter()
            {
                Id = 2,
                Amount = new MinMaxFilter<decimal>() { Min = 900, Max = 1000 },
                Comment = "op-test-2",
                Currency = "USD",
                From = 2,
                To = 3,
                Date = new MinMaxFilter<DateTime>() { Min = new DateTime(2016, 1, 1) , Max = new DateTime(2016, 1, 3) }
            };
            var filterNok = new OperationFilter()
            {
                Id = 2,
                Amount = new MinMaxFilter<decimal>() { Min = 900, Max = 1000 },
                Comment = "op-test-2",
                Currency = "EUR",  //<----- changed
                From = 2,
                To = 3,
                Date = new MinMaxFilter<DateTime>() { Min = new DateTime(2016, 1, 1), Max = new DateTime(2016, 1, 3) }
            };

            //Act
            var queryOk = filterOk.CreateQuery(data);
            var queryNok = filterNok.CreateQuery(data);

            var filteredDataOk = queryOk.ToList();
            var filteredDataNok = queryNok.ToList();

            //Assert
            Assert.Equal(1, filteredDataOk.Count());
            Assert.Equal(0, filteredDataNok.Count());

            Assert.Equal(2, filteredDataOk.FirstOrDefault().Id);
        }

        private IQueryable<Operation> CreateTestingOperations()
        {
            var tabs = new List<Tab>()
            {
                new Tab()
                    {
                        Id = 1,
                        Amount = 500,
                        Currency = "EUR",
                        Name = "income-test-1",
                    },
                new Tab()
                    {
                        Id = 2,
                        Amount = 1000,
                        Currency = "USD",
                        Name = "income-test-2",
                    },
                new Tab()
                    {
                        Id = 3,
                        Amount = 100,
                        Currency = "LVL",
                        Name = "income-test-3",
                    },
            };

            var operations = new List<Operation>()
            {
                new Operation()
                    {
                        Id = 1,
                        Amount = 500,
                        Comment = "op-test-1",
                        Currency = "EUR",
                        From = tabs[0],
                        To = tabs[1],
                        Date = new DateTime(2017, 10, 10)
                    },
                new Operation()
                    {
                        Id = 2,
                        Amount = 1000,
                        Comment = "op-test-2",
                        Currency = "USD",
                        From = tabs[1],
                        To = tabs[2],
                        Date = new DateTime(2016, 1, 2)
                    },
                new Operation()
                    {
                        Id = 3,
                        Amount = 100,
                        Comment = "op-test-3",
                        Currency = "EUR",
                        From = tabs[2],
                        To = tabs[0],
                        Date = new DateTime(2015, 5, 5)
                    },
            };

            tabs[0].OperationsFrom.Add(operations[0]);
            tabs[0].Operations.Add(operations[2]);
            tabs[1].Operations.Add(operations[0]);
            tabs[1].Operations.Add(operations[1]);
            tabs[2].Operations.Add(operations[1]);
            tabs[2].Operations.Add(operations[2]);

            return operations.AsQueryable();
        }
    }
}
