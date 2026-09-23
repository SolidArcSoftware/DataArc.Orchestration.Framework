using Microsoft.EntityFrameworkCore;

using Demo.Persistence.DbModels;

namespace Demo.Persistence.DbContexts
{
    public class HrDbContext : DbContext
    {
        public HrDbContext(DbContextOptions<HrDbContext> dbContextOptions)
            : base(dbContextOptions) { }

        public DbSet<Employer>? Employer { get; set; }
        public DbSet<Employee>? Employee { get; set; }
        public DbSet<Department>? Department { get; set; }
        public DbSet<EmployeeDepartment> EmployeeDepartment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employer>()
                .Property(x => x.Description)
                .HasColumnType("text");

            modelBuilder.Entity<EmployeeDepartment>()
                .HasIndex(x => new
                {
                    x.EmployeeId,
                    x.DepartmentId
                })
                .IsUnique();
        }
    }
}