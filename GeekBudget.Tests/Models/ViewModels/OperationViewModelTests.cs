//using System;
//using GeekBudget.Models;
//using GeekBudget.Models.ViewModels;
//using Xunit;

//namespace GeekBudget.Tests.Models.ViewModels
//{
//    public class OperationViewModelTests
//    {
//        [Fact]
//        public void CanMapToEntity()
//        {
//            //Arrange
//            var viewModel = new OperationViewModel()
//            {
//                Id = 1,
//                Comment = "op-comment-1",
//                Amount = 100,
//                Currency = "USD",
//                Date = new DateTime(1999, 10, 10)
//            };

//            //Act
//            var op = viewModel.MapToEntity();

//            //Assert
//            Assert.Equal("op-comment-1", op.Comment);
//            Assert.Equal("USD", op.Currency);
//            Assert.Equal(1999, op.Date.Year);
//            Assert.Equal(100, op.Amount);
//        }

//        [Fact]
//        public void CanMapFromEntity()
//        {
//            //Arrange
//            var op = new Operation()
//            {
//                Id = 1,
//                Comment = "op-comment-1",
//                Amount = 100,
//                Currency = "USD",
//                From = new Tab() { Id = 1},
//                To = new Tab() { Id = 2},
//                Date = new DateTime(1999, 10, 10)
//            };

//            //Act
//            var viewModel = new OperationViewModel().MapFromEntity(op);

//            //Assert
//            Assert.Equal(1, viewModel.Id);
//            Assert.Equal("op-comment-1", viewModel.Comment);
//            Assert.Equal(100, viewModel.Amount);
//            Assert.Equal("USD", viewModel.Currency);
//            Assert.Equal(1, viewModel.From);
//            Assert.Equal(2, viewModel.To);
//            Assert.Equal(1999, op.Date.Year);
//        }

//        [Fact]
//        public void CanGetFromEntity()
//        {
//            //Arrange
//            var op = new Operation()
//            {
//                Id = 1,
//                Comment = "op-comment-1",
//                Amount = 100,
//                Currency = "USD",
//                From = new Tab() { Id = 1 },
//                To = new Tab() { Id = 2 },
//                Date = new DateTime(1999, 10, 10)
//            };

//            //Act
//            var viewModel = OperationViewModel.FromEntity(op);

//            //Assert
//            Assert.Equal(1, viewModel.Id);
//            Assert.Equal("op-comment-1", viewModel.Comment);
//            Assert.Equal(100, viewModel.Amount);
//            Assert.Equal("USD", viewModel.Currency);
//            Assert.Equal(1, viewModel.From);
//            Assert.Equal(2, viewModel.To);
//            Assert.Equal(1999, op.Date.Year);
//        }
//    }
//}
