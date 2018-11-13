using System.Threading.Tasks;
using GeekBudget.Domain.Operations;
using GeekBudget.Domain.Tabs;
using GeekBudget.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace GeekBudget.DataAccess
{
    public interface IGeekBudgetContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Tab> Tabs { get; set; }
        DbSet<Operation> Operations { get; set; }
        Task<int> SaveChangesAsync();
        void SetModified(object entity);
        void BeginTransaction();
        void CommitTransaction(bool nested = false);
        void RollbackTransaction(bool nested = false);
    }
}
