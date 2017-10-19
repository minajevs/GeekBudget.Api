using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeekBudget.Controllers;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace GeekBudget.Test.Controllers
{
    public class OperationControllerTests
    {
        [Fact]
        public void CanGetAll()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() {Id = 1});
                var tab2 = context.Tabs.Add(new Tab() {Id = 2});
                context.Operations.Add(new Operation(){Id = 1, From = tab1.Entity, To = tab2.Entity});
                context.Operations.Add(new Operation(){Id = 2, From = tab2.Entity, To = tab1.Entity});
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);

                var result = controller.GetAll();


                //Assert
                Assert.IsType<OkObjectResult>(result);
                var data = ((OkObjectResult)result).Value as IEnumerable<OperationViewModel>;
                Assert.NotNull(data);
                Assert.Equal(2, data.Count());
            }
        }

        [Fact]
        public void CanGetFiltered()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1 });
                var tab2 = context.Tabs.Add(new Tab() { Id = 2 });
                context.Operations.Add(new Operation() { Id = 1, Amount = 100, From = tab1.Entity, To = tab2.Entity });
                context.Operations.Add(new Operation() { Id = 2, Amount = 600, From = tab2.Entity, To = tab1.Entity });
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);

                var result = controller.Get(new OperationFilter(){Amount = new MinMaxFilter<decimal>(){Min = 100, Max = 500}});

                //Assert
                Assert.IsType<OkObjectResult>(result);
                var data = ((OkObjectResult)result).Value as IEnumerable<OperationViewModel>;
                Assert.NotNull(data);
                Assert.Equal(1, data.Count());
                Assert.Equal(1, data.FirstOrDefault().Id);
            }
        }

        [Fact]
        public void CantGetNotExistingFiltered()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1 });
                var tab2 = context.Tabs.Add(new Tab() { Id = 2 });
                context.Operations.Add(new Operation() { Id = 1, Amount = 100, From = tab1.Entity, To = tab2.Entity });
                context.Operations.Add(new Operation() { Id = 2, Amount = 600, From = tab2.Entity, To = tab1.Entity });
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);

                var result = controller.Get(new OperationFilter() { Amount = new MinMaxFilter<decimal>() { Min = 800 } });

                //Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        [Fact]
        public void CanRemoveById()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1 });
                var tab2 = context.Tabs.Add(new Tab() { Id = 2 });
                context.Operations.Add(new Operation() { Id = 1, Amount = 100, From = tab1.Entity, To = tab2.Entity });
                context.Operations.Add(new Operation() { Id = 2, Amount = 600, From = tab2.Entity, To = tab1.Entity });
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);

                var result = controller.Remove(1);

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<OkResult>(result);
                Assert.False(context.Operations.Any(t => t.Id == 1));
                Assert.Equal(1, context
                    .Tabs
                        .Include(t => t.OperationsFrom)
                        .Include(t => t.OperationsTo)
                    .FirstOrDefault(t => t.Id == 1)
                        .Operations
                        .Count());
            }
        }

        [Fact]
        public void CantRemoveByUnknownId()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1 });
                var tab2 = context.Tabs.Add(new Tab() { Id = 2 });
                context.Operations.Add(new Operation() { Id = 1, Amount = 100, From = tab1.Entity, To = tab2.Entity });
                context.Operations.Add(new Operation() { Id = 2, Amount = 600, From = tab2.Entity, To = tab1.Entity });
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);

                var result = controller.Remove(99);

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public void CanAddNewOperation()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1 }).Entity;
                var tab2 = context.Tabs.Add(new Tab() { Id = 2 }).Entity;
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);

                var result = controller.Add(new OperationViewModel(){Amount = 100, From = 1, To = 2});

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<OkObjectResult>(result);
                var data = ((OkObjectResult)result).Value as int?;
                Assert.NotNull(data);
                Assert.Equal(1, data);
                Assert.Equal(1, context
                    .Tabs
                        .Include(t => t.OperationsFrom)
                        .Include(t => t.OperationsTo)
                    .FirstOrDefault(t => t.Id == 1)
                        .Operations
                        .Count());
            }
        }

        [Fact]
        public void CantAddNewOperationWithWrongModel()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1 }).Entity;
                var tab2 = context.Tabs.Add(new Tab() { Id = 2 }).Entity;
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);
                controller.ModelState.AddModelError("ModelValidation", "TestError");

                var result = controller.Add(new OperationViewModel() { Amount = 100, From = 1, To = 2 });

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public void CantAddNewOperationWithoutFromTab()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1 }).Entity;
                var tab2 = context.Tabs.Add(new Tab() { Id = 2 }).Entity;
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);
                controller.ModelState.AddModelError("ModelValidation", "TestError");

                var result = controller.Add(new OperationViewModel() { Amount = 100, /*From = 1,*/ To = 2 });

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public void CantAddNewOperationWithoutToTab()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1 }).Entity;
                var tab2 = context.Tabs.Add(new Tab() { Id = 2 }).Entity;
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);
                controller.ModelState.AddModelError("ModelValidation", "TestError");

                var result = controller.Add(new OperationViewModel() { Amount = 100, From = 1, /*To = 2*/ });

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public void CantAddNewOperationWithSameTabs()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1 }).Entity;
                var tab2 = context.Tabs.Add(new Tab() { Id = 2 }).Entity;
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);
                controller.ModelState.AddModelError("ModelValidation", "TestError");

                var result = controller.Add(new OperationViewModel() { Amount = 100, From = 2, To = 2 });

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public void CantAddNewOperationWithNotExistingFromTab()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1 }).Entity;
                var tab2 = context.Tabs.Add(new Tab() { Id = 2 }).Entity;
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);

                var result = controller.Add(new OperationViewModel() { Amount = 100, From = 99, To = 1 });

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public void CantAddNewOperationWithNotExistingToTab()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1 }).Entity;
                var tab2 = context.Tabs.Add(new Tab() { Id = 2 }).Entity;
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);

                var result = controller.Add(new OperationViewModel() { Amount = 100, From = 1, To = 99 });

                //Assert
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public void CanUpdate()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1, Amount = 500 }).Entity;
                var tab2 = context.Tabs.Add(new Tab() { Id = 2, Amount = 500 }).Entity;
                context.Operations.Add(new Operation() { Id = 1, From = tab1, To = tab2, Amount = 100});
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);

                var result = controller.Update(new OperationViewModel() {Id = 1, Amount = 200, From = 2, To = 1});

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<OkResult>(result);
                Assert.Equal(200, context.Operations.FirstOrDefault(o => o.Id == 1).Amount);
                Assert.Equal(600, context.Tabs.FirstOrDefault(t => t.Id == 1).Amount);
                Assert.Equal(400, context.Tabs.FirstOrDefault(t => t.Id == 2).Amount);
            }
        }

        [Fact]
        public void CantUpdateWithNullValue()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1, Amount = 500 }).Entity;
                var tab2 = context.Tabs.Add(new Tab() { Id = 2, Amount = 500 }).Entity;
                context.Operations.Add(new Operation() { Id = 1, From = tab1, To = tab2, Amount = 100 });
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);

                var result = controller.Update(null);

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public void CantUpdateWithWrongValue()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1, Amount = 500 }).Entity;
                var tab2 = context.Tabs.Add(new Tab() { Id = 2, Amount = 500 }).Entity;
                context.Operations.Add(new Operation() { Id = 1, From = tab1, To = tab2, Amount = 100 });
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);
                controller.ModelState.AddModelError("ModelValidation", "TestError");

                var result = controller.Update(new OperationViewModel() { Id = 1, Amount = 200 });

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public void CantUpdateNotExistingOperation()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1, Amount = 500 }).Entity;
                var tab2 = context.Tabs.Add(new Tab() { Id = 2, Amount = 500 }).Entity;
                context.Operations.Add(new Operation() { Id = 1, From = tab1, To = tab2, Amount = 100 });
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);

                var result = controller.Update(new OperationViewModel() { Id = 99, Amount = 200 });

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public void CantUpdateWithWrongNewFromTab()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1, Amount = 500 }).Entity;
                var tab2 = context.Tabs.Add(new Tab() { Id = 2, Amount = 500 }).Entity;
                context.Operations.Add(new Operation() { Id = 1, From = tab1, To = tab2, Amount = 100 });
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);

                var result = controller.Update(new OperationViewModel() { Id = 1, Amount = 200, From = 99 });

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public void CantUpdateWithWrongNewToTab()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                var tab1 = context.Tabs.Add(new Tab() { Id = 1, Amount = 500 }).Entity;
                var tab2 = context.Tabs.Add(new Tab() { Id = 2, Amount = 500 }).Entity;
                context.Operations.Add(new Operation() { Id = 1, From = tab1, To = tab2, Amount = 100 });
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);

                var result = controller.Update(new OperationViewModel() { Id = 1, Amount = 200, To = 99 });

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        private OperationController PrepareController(GeekBudgetContext context)
        {
            var controller = new OperationController(context);
            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<Object>()));
            controller.ObjectValidator = objectValidator.Object;
            return controller;
        }
    }
}
