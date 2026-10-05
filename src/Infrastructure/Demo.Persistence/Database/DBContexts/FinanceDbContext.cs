using Demo.Persistence.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Demo.Persistence.DbContexts
{
    public class FinanceDbContext : DbContext
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