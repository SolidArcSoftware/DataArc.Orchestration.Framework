using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Demo.Persistence.DbModels;

namespace Demo.Persistence.DbContexts
{
    public class AuthDbContext : IdentityDbContext<AuthUser, IdentityRole<int>, int>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AuthUser>(user =>
            {
                user.Property(u => u.Id)
                    .ValueGeneratedOnAdd()
                    .UseIdentityColumn();
            });
        }
    }
}