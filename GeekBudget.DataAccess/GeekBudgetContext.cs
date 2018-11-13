using System.Threading.Tasks;
using GeekBudget.Domain.Operations;
using GeekBudget.Domain.Tabs;
using GeekBudget.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GeekBudget.DataAccess
{
    public class GeekBudgetContext : DbContext, IGeekBudgetContext
    {
        public GeekBudgetContext(DbContextOptions options)
            : base(options)
        {
            base.Database.EnsureCreated();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Tab> Tabs { get; set; }
        public virtual DbSet<Operation> Operations { get; set; }

        private IDbContextTransaction DbTransaction { get; set; }

        Task<int> IGeekBudgetContext.SaveChangesAsync()
        {
            return this.SaveChangesAsync();
        }

        public void SetModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public void BeginTransaction()
        {
            if (this.DbTransaction == null)
                this.DbTransaction = base.Database.BeginTransaction();
        }

        public void CommitTransaction(bool nested = false)
        {
            if (this.DbTransaction == null || nested) return;

            this.DbTransaction.Commit();
            this.DbTransaction = null;
        }

        public void RollbackTransaction(bool nested = false)
        {
            if (this.DbTransaction == null || nested) return;

            this.DbTransaction.Rollback();
            this.DbTransaction = null;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tab>()
                .Ignore(t => t.Operations);

            modelBuilder.Entity<Operation>()
                .HasOne(o => o.From)
                .WithMany("OperationsFrom");

            modelBuilder.Entity<Operation>()
                .HasOne(o => o.To)
                .WithMany("OperationsTo");
        }
        
    }
}
