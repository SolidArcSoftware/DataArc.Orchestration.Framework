using Microsoft.EntityFrameworkCore;

using DataArc.Orchestration.Framework.Demo.Persistence.Contracts;
using DataArc.Orchestration.Framework.Demo.Persistence.Database.DBModels;

namespace DataArc.Orchestration.Framework.Demo.Persistence.Database.DBContexts
{
    internal class SharedContext : DbContext, ISharedContext
    {
        public SharedContext(DbContextOptions<SharedContext> dbContextOptions) 
            : base(dbContextOptions) { }

        public DbSet<EmployeeOnboardingTask> EmployeeOnboarding { get; set; }
        public DbSet<EmployeeAccessRequest> EmployeeAccessRequest { get; set; }
        public DbSet<EmployeePayrollRecord> EmployeePayrollRecord { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}