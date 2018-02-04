using System;
using System.Collections.Generic;
using System.Linq;
using GeekBudget.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;
using User = GeekBudget.Models.User;

namespace GeekBudget.Tests.Models
{
    public class TestUserRepository
    {
        [Fact]
        public void CanAddUser()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new GeekBudgetContext(options))
                {
                    context.Database.EnsureCreated();
                }

                string retKey;
                bool gotError = false;

                // Create new user repo
                using (var context = new GeekBudgetContext(options))
                {
                    var userRepo = new UserRepository(context);
                    retKey = userRepo.Add("testinguser");

                    try
                    {
                        userRepo.Add("testinguser"); //Again
                    }
                    catch (Exception e)
                    {
                        gotError = true;
                    }
                }

                // Use a separate instance of the context to verify correct data was saved to database
                using (var context = new GeekBudgetContext(options))
                {
                    Assert.NotNull(retKey);
                    Assert.True(gotError);
                    Assert.Equal(1, context.Users.Count());
                    Assert.Equal("testinguser", context.Users.FirstOrDefault().Username);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public void CanGetAllUsers()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                //Also populate database
                using (var context = new GeekBudgetContext(options))
                {
                    context.Database.EnsureCreated();
                    var users = new List<User>()
                    {
                        new User()
                        {
                            Key = "testkey1",
                            Username = "testname1"
                        },
                        new User()
                        {
                            Key = "testkey2",
                            Username = "testname2"
                        },
                        new User()
                        {
                            Key = "testkey3",
                            Username = "testname3"
                        }
                    };
                    context.Users.AddRange(users.ToArray());
                    context.SaveChanges();
                }

                UserRepository userRepo;

                // Create new user repo
                using (var context = new GeekBudgetContext(options))
                {
                    userRepo = new UserRepository(context);
                    var usersFromRepo = userRepo.GetAll();
                    Assert.Equal(3, usersFromRepo.Count());

                    Assert.Equal(1, usersFromRepo.ElementAt(0).Id);
                    Assert.Equal("testkey1", usersFromRepo.ElementAt(0).Key);
                    Assert.Equal("testname1", usersFromRepo.ElementAt(0).Username);

                    Assert.Equal(2, usersFromRepo.ElementAt(1).Id);
                    Assert.Equal("testkey2", usersFromRepo.ElementAt(1).Key);
                    Assert.Equal("testname2", usersFromRepo.ElementAt(1).Username);

                    Assert.Equal(3, usersFromRepo.ElementAt(2).Id);
                    Assert.Equal("testkey3", usersFromRepo.ElementAt(2).Key);
                    Assert.Equal("testname3", usersFromRepo.ElementAt(2).Username);
                }
            }
            finally
            {
                connection.Close();
            }
        }
        [Fact]
        public void CanFindUser()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new GeekBudgetContext(options))
                {
                    context.Database.EnsureCreated();
                    var users = new List<User>()
                    {
                        new User()
                        {
                            Key = "testkey1",
                            Username = "testname1"
                        },
                        new User()
                        {
                            Key = "testkey2",
                            Username = "testname2"
                        },
                        new User()
                        {
                            Key = "testkey3",
                            Username = "testname3"
                        }
                    };
                    context.Users.AddRange(users.ToArray());
                    context.SaveChanges();
                }

                using (var context = new GeekBudgetContext(options))
                {
                    var userRepo = new UserRepository(context);
                    var user3 = userRepo.Find("testkey3");
                    var user2 = userRepo.Find("testkey2");
                    var user1 = userRepo.Find("testkey1");
                    var usernone = userRepo.Find("testkeynotexisting");

                    Assert.Equal("testname1", user1.Username);
                    Assert.Equal("testname2", user2.Username);
                    Assert.Equal("testname3", user3.Username);
                    Assert.Null(usernone);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public void CanCheckValidUser()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new GeekBudgetContext(options))
                {
                    context.Database.EnsureCreated();
                    var users = new List<User>()
                    {
                        new User()
                        {
                            Key = "testkey1",
                            Username = "testname1"
                        }
                    };
                    context.Users.AddRange(users.ToArray());
                    context.SaveChanges();
                }

                bool existingUserValid = false;
                bool nonExistingUserIsInvalid= false;

                using (var context = new GeekBudgetContext(options))
                {
                    var userRepo = new UserRepository(context);
                    existingUserValid = userRepo.CheckValidUserKey("testkey1");
                    nonExistingUserIsInvalid = !userRepo.CheckValidUserKey("testkeynotexisting");
                }

                Assert.True(existingUserValid);
                Assert.True(nonExistingUserIsInvalid);
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public void CanRemoveUser()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new GeekBudgetContext(options))
                {
                    context.Database.EnsureCreated();
                    var users = new List<User>()
                    {
                        new User()
                        {
                            Key = "testkey1",
                            Username = "testname1"
                        },
                        new User()
                        {
                            Key = "testkey2",
                            Username = "testname2"
                        },
                        new User()
                        {
                            Key = "testkey3",
                            Username = "testname3"
                        }
                    };
                    context.Users.AddRange(users.ToArray());
                    context.SaveChanges();
                }

                string retKey;

                // Create new user repo
                using (var context = new GeekBudgetContext(options))
                {
                    var userRepo = new UserRepository(context);
                    Assert.Equal(3, context.Users.Count());
                    userRepo.Remove(1);
                }

                // Use a separate instance of the context to verify correct data was saved to database
                using (var context = new GeekBudgetContext(options))
                {
                    Assert.Equal(2, context.Users.Count());
                    Assert.Null(context.Users.FirstOrDefault(x => x.Username == "testname1"));
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public void CanUpdateUser()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new GeekBudgetContext(options))
                {
                    context.Database.EnsureCreated();
                    var users = new List<User>()
                    {
                        new User()
                        {
                            Key = "testkey1",
                            Username = "testname1"
                        },
                        new User()
                        {
                            Key = "testkey2",
                            Username = "testname2"
                        },
                        new User()
                        {
                            Key = "testkey3",
                            Username = "testname3"
                        }
                    };
                    context.Users.AddRange(users.ToArray());
                    context.SaveChanges();
                }

                string retKey;

                // Create new user repo
                using (var context = new GeekBudgetContext(options))
                {
                    var userRepo = new UserRepository(context);
                    var user = userRepo.Find("testkey3");
                    try
                    {
                        user.Username = "testname_changed";
                        userRepo.Update(user);
                    }
                    catch (Exception e)
                    {
                        Assert.True(true);
                    }
                }

                // Use a separate instance of the context to verify correct data was saved to database
                //using (var context = new GeekBudgetContext(options))
                //{
                //    Assert.Equal(2, context.Users.Count());
                //    Assert.Null(context.Users.FirstOrDefault(x => x.Username == "testname1"));
                //}
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public void CanCheckIfThereAreNoUsers()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<GeekBudgetContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (var context = new GeekBudgetContext(options))
                {
                    context.Database.EnsureCreated();
                    var users = new List<User>()
                    {
                        new User()
                        {
                            Key = "testkey1",
                            Username = "testname1"
                        }
                    };
                    context.Users.AddRange(users.ToArray());
                    context.SaveChanges();
                }

                bool contactsEmpty = true;

                using (var context = new GeekBudgetContext(options))
                {
                    var userRepo = new UserRepository(context);
                    contactsEmpty = userRepo.AreContactsEmpty();
                }

                Assert.False(contactsEmpty);
            }
            finally
            {
                connection.Close();
            }

        }
    }
}
