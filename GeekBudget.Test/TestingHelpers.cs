using GeekBudget.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeekBudget.Test
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
    }
}
