using System;
using System.Collections.Generic;
using System.Linq;
using GeekBudget.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GeekBudget.Tests
{
    public static class TestingHelpers
    {
        public static Mock<DbSet<TEntity>> CreateMockDbSet<TEntity>(ref List<TEntity> data) where TEntity : class
        {
            var mockSet = new Mock<DbSet<TEntity>>();
            var queryData = data.AsQueryable();
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(queryData.Provider);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryData.Expression);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            mockSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockSet;
        }

        public static DbContextOptions CreateOptions(SqliteConnection connection)
        {
            return new DbContextOptionsBuilder<GeekBudgetContext>()
                    .UseSqlite(connection)
                    .Options;
        }
    }

    public class TestingConnection : IDisposable
    {
        private readonly SqliteConnection _connection;
        public DbContextOptions Options => new DbContextOptionsBuilder<GeekBudgetContext>()
            .UseSqlite(_connection)
            .Options;

        private GeekBudgetContext _context;

        public TestingConnection()
        {
            //Create new in-memory db
            this._connection = new SqliteConnection("DataSource=:memory:");
            this._connection.Open();
        }

        public GeekBudgetContext CreateNewContext()
        {
            if (this._context != null)
            {
                this._context.Dispose();
                this._context = null;
            }

            return this._context = new GeekBudgetContext(this.Options);
        }

        public void Dispose()
        {
            if (this._context != null)
            {
                this._context.Dispose();
                this._context = null;
            }

            this._connection.Close();
        }
    }

    // https://stackoverflow.com/questions/38285815/how-to-write-unit-test-for-actionfilter-when-using-service-locator
    public class HttpContextUtils
    {
        public static ActionExecutingContext MockedActionExecutingContext(
            HttpContext context,
            IList<IFilterMetadata> filters,
            IDictionary<string, object> actionArguments,
            object controller
        )
        {
            var actionContext = new ActionContext() { HttpContext = context };

            return new ActionExecutingContext(actionContext, filters, actionArguments, controller);
        }
        public static ActionExecutingContext MockedActionExecutingContext(
            HttpContext context,
            object controller
        )
        {
            return MockedActionExecutingContext(context, new List<IFilterMetadata>(), new Dictionary<string, object>(), controller);
        }
    }
}
