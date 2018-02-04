using System;
using System.Collections.Generic;
using GeekBudget.Entities;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using Xunit;

namespace GeekBudget.Tests.Models
{
    public class TabTests
    {
        [Fact]
        public void CanMapNewValues()
        {
            //Arrange
            var tab1 = new Tab()
            {
                Id = 1,
                Name = "testing-tab-1",
                Currency = "EUR",
                Amount = 1000
            };

            var updateTab = new TabViewModel()
            {
                Id = 999,
                Name = "testing-tab-1-changed",
                Amount = 500,
                Currency = "USD"
            };

            //Act
            tab1.MapNewValues(updateTab);

            //Assert
            Assert.Equal("testing-tab-1-changed", tab1.Name);
            Assert.Equal("USD", tab1.Currency);
            Assert.Equal(500, tab1.Amount);
            Assert.Equal(1, tab1.Id);
        }

        [Fact]
        public void CanAddNewOperation()
        {
            //Arrange
            var tab1 = new Tab() { Id = 1,Amount = 1000,Type = Enums.TabType.Account };
            var tab2 = new Tab() { Id = 2,Amount = 500,Type = Enums.TabType.Account };

            var op = new Operation()
            {
                Id = 1,
                From = tab1,
                To = tab2,
                Amount = 100,
            };

            //Act
            tab1.AddNewOperation(Enums.TargetTabType.From, op);
            tab2.AddNewOperation(Enums.TargetTabType.To, op);

            //Assert
            Assert.Equal(1, tab1.Operations.Count);
            Assert.Equal(900, tab1.Amount);
            Assert.Equal(1, tab2.Operations.Count);
            Assert.Equal(600, tab2.Amount);
        }

        [Fact]
        public void CantAddSameOperationOnFromTab()
        {
            //Arrange
            var tab1 = new Tab() { Id = 1, Amount = 1000,Type = Enums.TabType.Account };
            var tab2 = new Tab() { Id = 2, Amount = 500,Type = Enums.TabType.Account };

            var op = new Operation()
            {
                Id = 1,
                From = tab1,
                To = tab2,
                Amount = 100,
            };

            //Act
            tab1.AddNewOperation(Enums.TargetTabType.From, op);
            Action act = () => tab1.AddNewOperation(Enums.TargetTabType.From, op);

            //Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void CantAddSameOperationOnToTab()
        {
            //Arrange
            var tab1 = new Tab() { Id = 1, Amount = 1000,Type = Enums.TabType.Account };
            var tab2 = new Tab() { Id = 2, Amount = 500,Type = Enums.TabType.Account };

            var op = new Operation()
            {
                Id = 1,
                From = tab1,
                To = tab2,
                Amount = 100,
            };

            //Act
            tab2.AddNewOperation(Enums.TargetTabType.To, op);
            Action act = () => tab2.AddNewOperation(Enums.TargetTabType.To, op);

            //Assert
            Assert.Throws<InvalidOperationException>(act);
        }

        [Fact]
        public void CanAddOperationWithAnotherCurrency()
        {
            //Arrange
            var tab1 = new Tab() { Id = 1, Amount = 1000,Type = Enums.TabType.Account };
            var tab2 = new Tab() { Id = 2, Amount = 500,Type = Enums.TabType.Account };

            var op = new Operation()
            {
                Id = 1,
                From = tab1,
                To = tab2,
                Amount = 100,
                Currency = "USD"
            };

            //Act
            Action actTo = () => tab1.AddNewOperation(Enums.TargetTabType.From, op);
            Action actFrom = () => tab2.AddNewOperation(Enums.TargetTabType.To, op);

            //Assert
            Assert.Throws<NotImplementedException>(actTo); //TODO
            Assert.Throws<NotImplementedException>(actFrom); //TODO
        }

        [Fact]
        public void CanRemoveOperation()
        {
            //Arrange

            var op = new Operation()
            {
                Id = 1,
                Amount = 100,
                Currency = "EUR"
            };

            var tab1 = new Tab()
            {
                Id = 1,
                Amount = 1000,
                OperationsFrom = new List<Operation>() { op }
            };

            var tab2 = new Tab()
            {
                Id = 2,
                Amount = 500,
                OperationsTo = new List<Operation>() { op }
            };

            op.From = tab1;
            op.To = tab2;

            //Act
            tab1.RemoveOperation(op);
            tab2.RemoveOperation(op);

            //Assert
            Assert.Equal(0, tab1.Operations.Count);
            Assert.Equal(1100, tab1.Amount);
            Assert.Equal(0, tab2.Operations.Count);
            Assert.Equal(400, tab2.Amount);
        }

        [Fact]
        public void CantRemoveUnknownOperation()
        {
            //Arrange

            var op = new Operation()
            {
                Id = 1,
                Amount = 100,
                Currency = "EUR"
            };

            var tab1 = new Tab()
            {
                Id = 1,
                Amount = 1000,
                OperationsFrom = new List<Operation>() { op }
            };

            var tab2 = new Tab()
            {
                Id = 2,
                Amount = 500,
                //OperationsTo = new List<Operation>() { op }
            };

            op.From = tab1;
            op.To = tab2;

            //Act
            tab1.RemoveOperation(op);
            Action act = () => tab2.RemoveOperation(op);

            //Assert
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void CanRemoveOperationWithAnotherCurrency()
        {
            //Arrange

            var op = new Operation()
            {
                Id = 1,
                Amount = 100,
                Currency = "USD"
            };

            var tab1 = new Tab()
            {
                Id = 1,
                Amount = 1000,
                OperationsFrom = new List<Operation>() { op }
            };

            var tab2 = new Tab()
            {
                Id = 2,
                Amount = 500,
                OperationsTo = new List<Operation>() { op }
            };

            op.From = tab1;
            op.To = tab2;

            //Act
            Action actFrom = () => tab1.RemoveOperation(op);
            Action actTo = () => tab2.RemoveOperation(op);

            //Assert
            Assert.Throws<NotImplementedException>(actFrom);
            Assert.Throws<NotImplementedException>(actTo);
        }
    }
}
