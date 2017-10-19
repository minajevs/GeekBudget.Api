using GeekBudget.Middleware;
using GeekBudget.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using User = GeekBudget.Models.User;


namespace GeekBudget.Test
{
    public class UserKeyValidatorTests
    {
        //[Fact] //TODO: Implement middleware tests!!!!!!
        public void KeyValidationWorksCorrectly()
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

                // Create new user repo
                using (var context = new GeekBudgetContext(options))
                {
                    var userRepo = new UserRepository(context);
                    var builder = new WebHostBuilder()
                        .Configure(app => {
                            app.UseMiddleware<UserKeyValidatorTests>(userRepo);
                        }
                    );

                    var server = new TestServer(builder);
                    var request = new HttpRequestMessage(new HttpMethod("GET"), "");

                }

                // Use a separate instance of the context to verify correct data was saved to database
                using (var context = new GeekBudgetContext(options))
                {
                   // Assert.NotNull(retKey);
                    Assert.Equal(1, context.Users.Count());
                    Assert.Equal("testinguser", context.Users.FirstOrDefault().Username);
                }
            }
            finally
            {
                connection.Close();
            }

        }
    }
}
