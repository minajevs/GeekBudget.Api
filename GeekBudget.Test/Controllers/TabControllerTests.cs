using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeekBudget.Controllers;
using GeekBudget.Models;
using GeekBudget.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Data.Sqlite;
using Moq;
using Xunit;

namespace GeekBudget.Test.Controllers
{
    public class TabControllerTests
    {
        [Fact]
        public void CanGetAll()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                context.Tabs.AddRange(new List<Tab>
                {
                    new Tab(){Id = 1}, new Tab(){Id=2}
                });
                context.SaveChanges();

                //Act
                context = connection.CreateNewContext();
                var controller = PrepareController(context);

                var result = controller.GetAll();


                //Assert
                Assert.IsType<OkObjectResult>(result);
                var data = ((OkObjectResult)result).Value as IEnumerable<TabViewModel>;
                Assert.NotNull(data);
                Assert.Equal(2, data.Count());
            }
        }

        [Fact]
        public void CanGetById()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                context.Tabs.AddRange(new List<Tab>
                {
                    new Tab() {Id = 1},
                    new Tab() {Id = 2}
                });
                context.SaveChanges();

                //Act 
                context = connection.CreateNewContext();
                var controller = PrepareController(context);
                var result = controller.Get(2);

                //Assert
                Assert.IsType<OkObjectResult>(result);
                var data = ((OkObjectResult)result).Value as TabViewModel;
                Assert.NotNull(data);
                Assert.Equal(2, data.Id);
            }
        }

        [Fact]
        public void CantFindUnknownIdTab()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                context.Tabs.AddRange(new List<Tab>
                {
                    new Tab() {Id = 1},
                    new Tab() {Id = 2}
                });
                context.SaveChanges();

                //Act 
                context = connection.CreateNewContext();
                var controller = PrepareController(context);
                var tab = controller.Get(99);

                //Assert
                Assert.IsType<NotFoundResult>(tab);
            }
        }

        [Fact]
        public void CanAddTab()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                context.Tabs.Add(new Tab() {Id = 1});
                context.SaveChanges();

                //Act 
                context = connection.CreateNewContext();
                var controller = PrepareController(context);
                var result = controller.Add(new TabViewModel(){Amount = 123, Name = "new-tab"});

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<OkObjectResult>(result);
                var data = ((OkObjectResult)result).Value as int?;
                Assert.NotNull(data);
                Assert.Equal(2, data);
                Assert.Equal(2, context.Tabs.Count());
                Assert.True(context.Tabs.Any(t => t.Name == "new-tab"));
            }
        }

        [Fact]
        public void CanеAddWrongModeledTab()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                context.Tabs.Add(new Tab() { Id = 1 });
                context.SaveChanges();

                //Act 
                context = connection.CreateNewContext();
                var controller = PrepareController(context);
                controller.ModelState.AddModelError("ModelValidation", "TestError");
                var result = controller.Add(new TabViewModel() { Amount = 123, Name = "new-tab" });

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<BadRequestObjectResult>(result);
                Assert.False(context.Tabs.Any(t => t.Name == "new-tab"));
            }
        }

        [Fact]
        public void CanRemoveTab()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                context.Tabs.Add(new Tab() { Id = 1 });
                context.Tabs.Add(new Tab() { Id = 2 });
                context.SaveChanges();

                //Act 
                context = connection.CreateNewContext();
                var controller = PrepareController(context);
                var result = controller.Remove(1);

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<OkResult>(result);
                Assert.Equal(1, context.Tabs.Count());
                Assert.True(context.Tabs.Any(t => t.Id == 2));
            }
        }

        [Fact]
        public void CantRemoveNotExistingTab()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                context.Tabs.Add(new Tab() { Id = 1 });
                context.Tabs.Add(new Tab() { Id = 2 });
                context.SaveChanges();

                //Act 
                context = connection.CreateNewContext();
                var controller = PrepareController(context);
                var result = controller.Remove(99);

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(2, context.Tabs.Count());
            }
        }

        [Fact]
        public void CanUpdateTab()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                context.Tabs.Add(new Tab() { Id = 1, Name = "test-tab-1"});
                context.SaveChanges();

                //Act 
                context = connection.CreateNewContext();
                var controller = PrepareController(context);
                var result = controller.Update(new TabViewModel(){Id = 1, Name = "test-tab-1-updated"});

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<OkResult>(result);
                Assert.Equal("test-tab-1-updated", context.Tabs.FirstOrDefault(t => t.Id == 1).Name);
            }
        }

        [Fact]
        public void CantUpdateTabWithNullValue()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                context.Tabs.Add(new Tab() { Id = 1, Name = "test-tab-1" });
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
        public void CantUpdateTabWithNotExistingId()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                context.Tabs.Add(new Tab() { Id = 1, Name = "test-tab-1" });
                context.SaveChanges();

                //Act 
                context = connection.CreateNewContext();
                var controller = PrepareController(context);
                var result = controller.Update(new TabViewModel(){Id = 99, Name = "test-tab-99-changed"});

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<BadRequestObjectResult>(result);
            }
        }

        [Fact]
        public void CantUpdateTabWithWrongModel()
        {
            using (var connection = new TestingConnection())
            {
                //Arrange
                var context = connection.CreateNewContext();

                context.Database.EnsureCreated();
                context.Tabs.Add(new Tab() { Id = 1, Name = "test-tab-1"});
                context.SaveChanges();

                //Act 
                context = connection.CreateNewContext();
                var controller = PrepareController(context);
                controller.ModelState.AddModelError("ModelValidation", "TestError");
                var result = controller.Update(new TabViewModel() { Id = 1, Name = "test-tab-1-changed" });

                //Assert
                context = connection.CreateNewContext();
                Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal("test-tab-1", context.Tabs.FirstOrDefault(t => t.Id == 1).Name);
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
    }
}
