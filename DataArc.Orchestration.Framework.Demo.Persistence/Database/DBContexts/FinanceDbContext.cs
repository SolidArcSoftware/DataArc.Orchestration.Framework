using Microsoft.EntityFrameworkCore;

using DataArc.Orchestration.Framework.Demo.Persistence.Contracts;
using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;

namespace DataArc.Orchestration.Framework.Demo.Persistence.DbContexts
{
    internal class FinanceDbContext : DbContext, IFinanceDbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> dbContextOptions)
            : base(dbContextOptions) { }

        public DbSet<PayrollRecord>? PayrollRecord { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}