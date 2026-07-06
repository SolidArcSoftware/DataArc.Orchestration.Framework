using Microsoft.EntityFrameworkCore;

using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;
using DataArc.Orchestration.Framework.Demo.Persistence.Contracts;

namespace DataArc.Orchestration.Framework.Demo.Persistence.DbContexts
{
    internal class ItDbContext : DbContext, IItDbContext
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