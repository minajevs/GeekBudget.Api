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
using GeekBudget.Entities;
using GeekBudget.Models.ViewModels;
using Xunit;

namespace GeekBudget.Test.Models
{
    public class OperationTests
    {
        [Fact]
        public void CanMapNewValues()
        {
            //Arrange
            var op = new Operation()
            {
                Id = 1,
                Comment = "op-comment-1",
                Amount = 100,
                Currency = "EUR",
                From = new Tab() { Id = 1, Amount = 1900},
                To = new Tab() { Id = 2, Amount = 2100},
                Date = DateTime.Now
            };

            var newOp = new OperationViewModel()
            {
                Id = 2,
                Comment = "op-comment-1-new",
                Amount = 1000,
                Currency = "USD",
                From = 1,
                To = 2,
                Date = new DateTime(1992, 10, 10)
            };

            //Act
            op.MapNewValues(newOp);

            //Assert
            Assert.Equal("op-comment-1-new", op.Comment);
            Assert.Equal("USD", op.Currency);
            Assert.Equal(1992, op.Date.Year);
            Assert.Equal(1000, op.From.Amount);
            Assert.Equal(3000, op.To.Amount);
        }

        [Fact]
        public void DoesNotUpdateAmountIfIsNull()
        {
            //Arrange
            var op = new Operation()
            {
                Id = 1,
                Comment = "op-comment-1",
                Amount = 100,
                Currency = "EUR",
                From = new Tab() { Id = 1, Amount = 1900 },
                To = new Tab() { Id = 2, Amount = 2100 },
                Date = DateTime.Now
            };

            var newOp = new OperationViewModel()
            {
                Comment = "op-comment-1-new",
            };

            //Act
            op.MapNewValues(newOp);

            //Assert
            Assert.Equal("op-comment-1-new", op.Comment);
            Assert.Equal(100, op.Amount);
        }

        [Fact]
        public void CanUpdateTabs()
        {
            //Arrange
            var tab1 = new Tab()
            {
                Id = 1,
                Amount = 1000
            };

            var tab2 = new Tab()
            {
                Id = 2,
                Amount = 1000
            };

            var tab3 = new Tab()
            {
                Id = 3,
                Amount = 1000
            };

            var tab4 = new Tab()
            {
                Id = 4,
                Amount = 1000
            };

            var op = new Operation()
            {
                Id = 1,
                From = tab1,
                To = tab2,
                Amount = 100
            };

            tab1.OperationsFrom = new List<Operation>(){op};
            tab2.OperationsTo = new List<Operation>(){op};

            //Act
            op.UpdateTab(Enums.TabType.From, tab3);
            op.UpdateTab(Enums.TabType.To, tab4);

            //Assert
            Assert.Equal(3, op.From.Id);
            Assert.Equal(4, op.To.Id);

            Assert.Empty(tab1.Operations); //integration tests -------
            Assert.Empty(tab2.Operations);
            Assert.NotEmpty(tab3.OperationsFrom);
            Assert.NotEmpty(tab4.OperationsTo);
            Assert.Equal(1100, tab1.Amount);
            Assert.Equal(900, tab2.Amount);
            Assert.Equal(900, tab3.Amount);
            Assert.Equal(1100, tab4.Amount); //-----------------------
        }

        [Fact]
        public void DoesNotUpdateTabIfNull()
        {
            //Arrange
            var tab1 = new Tab()
            {
                Id = 1,
                Amount = 1000
            };

            var tab2 = new Tab()
            {
                Id = 2,
                Amount = 1000
            };

            var op = new Operation()
            {
                Id = 1,
                From = tab1,
                To = tab2,
                Amount = 100
            };

            //Act
            op.UpdateTab(Enums.TabType.From, null);
            op.UpdateTab(Enums.TabType.To, null);

            //Assert
            Assert.Equal(1, op.From.Id);
            Assert.Equal(2, op.To.Id);
        }
    }
}
