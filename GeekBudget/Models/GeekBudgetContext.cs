using GeekBudget.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekBudget.Models
{
    public class GeekBudgetContext : DbContext, IGeekBudgetContext
    {
        //public GeekBudgetContext() { }
        public GeekBudgetContext(DbContextOptions options)
            : base(options)
        {
            base.Database.EnsureCreated();
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Tab> Tabs { get; set; }
        public virtual DbSet<Operation> Operations { get; set; }

        Task<int> IGeekBudgetContext.SaveChangesAsync()
        {
            return this.SaveChangesAsync();
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
