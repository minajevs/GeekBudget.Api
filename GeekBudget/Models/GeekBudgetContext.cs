using GeekBudget.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Models
{
    public class GeekBudgetContext : DbContext, IGeekBudgetContext
    {
        public GeekBudgetContext(DbContextOptions<GeekBudgetContext> options)
            :base(options)
        { }

        public DbSet<User> Users { get; set; }
    }
}
