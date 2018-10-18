using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Models
{
    public interface IGeekBudgetContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Tab> Tabs { get; set; }
        DbSet<Operation> Operations { get; set; }
        Task<int> SaveChangesAsync();
        void SetModified(object entity);
    }
}
