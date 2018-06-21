using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeekBudget.Controllers;
using GeekBudget.Entities;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using GeekBudget.Services;
using GeekBudget.Services.Implementations;
using GeekBudget.Validators;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Operation = GeekBudget.Models.Operation;

namespace GeekBudget.Tests.Controllers
{
    public class OperationControllerTests
    {
        [Fact]
        public async Task GetAll_ReturnsAllOperations()
        {
            //Arrange
            var operations = new List<Operation>()
            {
                new Operation(){To = new Tab(){Id = 1}, From = new Tab(){Id = 2}},
                new Operation(){To = new Tab(){Id = 2}, From = new Tab(){Id = 1}},
            };
            var service = new Mock<IOperationService>();
            service.Setup(x => x.GetAll()).ReturnsAsyncServiceResult(operations);
            var validators = new Mock<IOperationValidators>();
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.GetAll() as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            var data = result.Value as IEnumerable<OperationViewModel>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
        }

        [Fact]
        public async Task Get_ReturnsFilteredOperations()
        {
            //Arrange
            var operations = new List<Operation>()
            {
                new Operation(){To = new Tab(){Id = 1}, From = new Tab(){Id = 2}},
                new Operation(){To = new Tab(){Id = 2}, From = new Tab(){Id = 1}},
            };
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Get(It.IsAny<OperationFilter>())).ReturnsAsyncServiceResult(operations);
            var validators = new Mock<IOperationValidators>();
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.Get(new OperationFilter()) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            var data = result.Value as IEnumerable<OperationViewModel>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count());
        }

        [Fact]
        public async Task Get_ReturnsNotFoundIfNoOperations()
        {
            //Arrange
            var operations = new List<Operation>();
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Get(It.IsAny<OperationFilter>())).ReturnsAsyncServiceResult(operations);
            var validators = new Mock<IOperationValidators>();
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.Get(new OperationFilter()) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Add_OperationAdded()
        {
            //Arrange
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Add(It.IsAny<Operation>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsyncServiceResult(1);
            var validators = new Mock<IOperationValidators>();
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.Add(new OperationViewModel()) as OkObjectResult;

            //Assert
            Assert.NotNull(result);
            var data = (int)result.Value;
            Assert.Equal(1, data);
        }

        [Fact]
        public async Task Add_DoesNotAddIncorrectOperation()
        {
            //Arrange
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Add(It.IsAny<Operation>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsyncServiceResult(1);
            var validators = new Mock<IOperationValidators>();
            validators.Setup(x => x.NotNull(It.IsAny<OperationViewModel>())).ReturnsAsync(new List<Error>()
            {
                new Error() {Id = 999, Description = "testing-error"}
            });
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.Add(new OperationViewModel()) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(result);
            var data = (List<Error>)result.Value;
            Assert.Single(data);
            Assert.Equal(999, data.SingleOrDefault()?.Id);
            Assert.Equal("testing-error", data.SingleOrDefault()?.Description);
        }

        [Fact]
        public async Task Remove_RemovesOperation()
        {
            //Arrange
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Remove(It.IsAny<int>())).ReturnsAsyncServiceResult();
            var validators = new Mock<IOperationValidators>();
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.Remove(1) as OkResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Update_UdpdatesCorrectOperation()
        {
            //Arrange
            var service = new Mock<IOperationService>();
            service.Setup(x => x.Update(It.IsAny<int>(), It.IsAny<Operation>(), It.IsAny<OperationViewModel>())).ReturnsAsyncServiceResult();
            var validators = new Mock<IOperationValidators>();
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.Update(new OperationViewModel()) as OkResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Update_DoesNotUpdateIncorrectOperation()
        {
            //Arrange
            var service = new Mock<IOperationService>();
            var validators = new Mock<IOperationValidators>();
            validators.Setup(x => x.NotNull(It.IsAny<OperationViewModel>())).ReturnsAsync(new List<Error>()
            {
                new Error() {Id = 999, Description = "testing-error"}
            });
            var mapping = new MappingService();
            var controller = new OperationController(service.Object, validators.Object, mapping);

            //Act
            var result = await controller.Update(new OperationViewModel()) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(result);
        }
    }
}

//        [Fact]
//        public void CanUpdate()
//        {
//            using (var connection = new TestingConnection())
//            {
//                //Arrange
//                var context = connection.CreateNewContext();

//                context.Database.EnsureCreated();
//                var tab1 = context.Tabs.Add(new Tab() { Id = 1, Amount = 500, Type = Enums.TabType.Account }).Entity;
//                var tab2 = context.Tabs.Add(new Tab() { Id = 2, Amount = 500, Type = Enums.TabType.Account }).Entity;
//                context.Operations.Add(new Operation() { Id = 1, From = tab1, To = tab2, Amount = 100 });
//                context.SaveChanges();

//                //Act
//                context = connection.CreateNewContext();
//                var controller = PrepareController(context);

//                var result = controller.Update(new OperationViewModel() { Id = 1, Amount = 200, From = 2, To = 1 });

//                //Assert
//                context = connection.CreateNewContext();
//                Assert.IsType<OkResult>(result);
//                Assert.Equal(200, context.Operations.FirstOrDefault(o => o.Id == 1).Amount);
//                Assert.Equal(600, context.Tabs.FirstOrDefault(t => t.Id == 1).Amount);
//                Assert.Equal(400, context.Tabs.FirstOrDefault(t => t.Id == 2).Amount);
//            }
//        }

//        [Fact]
//        public void CantUpdateWithNullValue()
//        {
//            using (var connection = new TestingConnection())
//            {
//                //Arrange
//                var context = connection.CreateNewContext();

//                context.Database.EnsureCreated();
//                var tab1 = context.Tabs.Add(new Tab() { Id = 1, Amount = 500 }).Entity;
//                var tab2 = context.Tabs.Add(new Tab() { Id = 2, Amount = 500 }).Entity;
//                context.Operations.Add(new Operation() { Id = 1, From = tab1, To = tab2, Amount = 100 });
//                context.SaveChanges();

//                //Act
//                context = connection.CreateNewContext();
//                var controller = PrepareController(context);

//                var result = controller.Update(null);

//                //Assert
//                context = connection.CreateNewContext();
//                Assert.IsType<BadRequestObjectResult>(result);
//            }
//        }

//        [Fact]
//        public void CantUpdateWithWrongValue()
//        {
//            using (var connection = new TestingConnection())
//            {
//                //Arrange
//                var context = connection.CreateNewContext();

//                context.Database.EnsureCreated();
//                var tab1 = context.Tabs.Add(new Tab() { Id = 1, Amount = 500 }).Entity;
//                var tab2 = context.Tabs.Add(new Tab() { Id = 2, Amount = 500 }).Entity;
//                context.Operations.Add(new Operation() { Id = 1, From = tab1, To = tab2, Amount = 100 });
//                context.SaveChanges();

//                //Act
//                context = connection.CreateNewContext();
//                var controller = PrepareController(context);
//                controller.ModelState.AddModelError("ModelValidation", "TestError");

//                var result = controller.Update(new OperationViewModel() { Id = 1, Amount = 200 });

//                //Assert
//                context = connection.CreateNewContext();
//                Assert.IsType<BadRequestObjectResult>(result);
//            }
//        }

//        [Fact]
//        public void CantUpdateNotExistingOperation()
//        {
//            using (var connection = new TestingConnection())
//            {
//                //Arrange
//                var context = connection.CreateNewContext();

//                context.Database.EnsureCreated();
//                var tab1 = context.Tabs.Add(new Tab() { Id = 1, Amount = 500 }).Entity;
//                var tab2 = context.Tabs.Add(new Tab() { Id = 2, Amount = 500 }).Entity;
//                context.Operations.Add(new Operation() { Id = 1, From = tab1, To = tab2, Amount = 100 });
//                context.SaveChanges();

//                //Act
//                context = connection.CreateNewContext();
//                var controller = PrepareController(context);

//                var result = controller.Update(new OperationViewModel() { Id = 99, Amount = 200 });

//                //Assert
//                context = connection.CreateNewContext();
//                Assert.IsType<BadRequestObjectResult>(result);
//            }
//        }

//        [Fact]
//        public void CantUpdateWithWrongNewFromTab()
//        {
//            using (var connection = new TestingConnection())
//            {
//                //Arrange
//                var context = connection.CreateNewContext();

//                context.Database.EnsureCreated();
//                var tab1 = context.Tabs.Add(new Tab() { Id = 1, Amount = 500 }).Entity;
//                var tab2 = context.Tabs.Add(new Tab() { Id = 2, Amount = 500 }).Entity;
//                context.Operations.Add(new Operation() { Id = 1, From = tab1, To = tab2, Amount = 100 });
//                context.SaveChanges();

//                //Act
//                context = connection.CreateNewContext();
//                var controller = PrepareController(context);

//                var result = controller.Update(new OperationViewModel() { Id = 1, Amount = 200, From = 99 });

//                //Assert
//                context = connection.CreateNewContext();
//                Assert.IsType<BadRequestObjectResult>(result);
//            }
//        }

//        [Fact]
//        public void CantUpdateWithWrongNewToTab()
//        {
//            using (var connection = new TestingConnection())
//            {
//                //Arrange
//                var context = connection.CreateNewContext();

//                context.Database.EnsureCreated();
//                var tab1 = context.Tabs.Add(new Tab() { Id = 1, Amount = 500 }).Entity;
//                var tab2 = context.Tabs.Add(new Tab() { Id = 2, Amount = 500 }).Entity;
//                context.Operations.Add(new Operation() { Id = 1, From = tab1, To = tab2, Amount = 100 });
//                context.SaveChanges();

//                //Act
//                context = connection.CreateNewContext();
//                var controller = PrepareController(context);

//                var result = controller.Update(new OperationViewModel() { Id = 1, Amount = 200, To = 99 });

//                //Assert
//                context = connection.CreateNewContext();
//                Assert.IsType<BadRequestObjectResult>(result);
//            }
//        }
//    }
//}
