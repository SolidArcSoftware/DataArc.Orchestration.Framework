using Demo.Persistence.DbModels;

namespace Demo.Persistence.Utils
{
    public static class SeedDataGenerator
    {
        public static List<AuthUser> GenerateIdentityUserSeedData(
            int count = 50)
        {
            var users = new List<AuthUser>();

            for (int i = 1; i <= count; i++)
            {
                var email = $"employee{i}@solidarcsoftware.com";

                users.Add(new AuthUser
                {
                    UserName = email,
                    NormalizedUserName = email.ToUpperInvariant(),

                    Email = email,
                    NormalizedEmail = email.ToUpperInvariant(),

                    EmailConfirmed = true,

                    AccessFailedCount = 0,
                    LockoutEnabled = true,
                    LockoutEnd = null,

                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,

                    TwoFactorEnabled = false,

                    SecurityStamp = Guid.NewGuid().ToString(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),

                    UserTimeZone = "Africa/Johannesburg"
                });
            }

            return users;
        }
    }
}