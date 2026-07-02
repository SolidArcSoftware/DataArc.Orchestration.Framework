using Microsoft.EntityFrameworkCore;

using DataArc.Orchestration.Framework.Demo.Persistence.Contracts;
using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;

namespace DataArc.Orchestration.Framework.Demo.Persistence.DbContexts
{
    internal class OperationsDbContext : DbContext, IOperationsDbContext
    {
        public OperationsDbContext(DbContextOptions<OperationsDbContext> dbContextOptions) 
            : base(dbContextOptions) { }

        public DbSet<OnboardingTask>? OnboardingTask { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}