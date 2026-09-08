using Microsoft.EntityFrameworkCore;

using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;

namespace DataArc.Orchestration.Framework.Demo.Persistence.DbContexts
{
    public class ItDbContext : DbContext
    {
        public ItDbContext(DbContextOptions<ItDbContext> dbContextOptions) 
            : base(dbContextOptions) { }

        public DbSet<AccessRequest>? AccessRequest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}