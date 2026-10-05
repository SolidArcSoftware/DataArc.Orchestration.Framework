using Microsoft.EntityFrameworkCore;

using Demo.Persistence.DbModels;

namespace Demo.Persistence.DbContexts
{
    public class OperationsDbContext : DbContext
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