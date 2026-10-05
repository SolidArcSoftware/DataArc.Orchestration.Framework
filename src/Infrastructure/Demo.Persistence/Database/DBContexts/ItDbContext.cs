using Microsoft.EntityFrameworkCore;

using Demo.Persistence.DbModels;

namespace Demo.Persistence.DbContexts
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