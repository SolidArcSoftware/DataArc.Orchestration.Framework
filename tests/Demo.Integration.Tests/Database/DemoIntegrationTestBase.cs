using DataArc.Core;
using DataArc.EntityFrameworkCore;

using Demo.Application.Modules.Auth;
using Demo.Application.Modules.Finance;
using Demo.Application.Modules.HR;
using Demo.Application.Modules.IT;
using Demo.Application.Modules.Operations;

using Demo.Persistence.DbContexts;
using Demo.Persistence.DbModels;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Integration.Tests.Database
{
    internal abstract class DemoIntegrationTestBase
    {
        protected ServiceProvider ServiceProvider = null!;

        private IDatabaseFactory _databaseFactory = null!;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var configuration = new ConfigurationManager();

            configuration
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: false);

            var licenseKey = configuration["DataArc:LicenseKey"];
            var services = new ServiceCollection();

            services
                .AddDataArcCore(options =>
                {
                    if (string.IsNullOrWhiteSpace(licenseKey))
                        options.UseServerKey();
                    else
                        options.UseKey(licenseKey);
                })
                .ConfigureDataArc();

            // The onboarding integration test intentionally composes the full demo.
            // HR owns the use case, but execution crosses Auth, HR, IT, Operations
            // and Finance persistence boundaries within the same physical database.
            services.AddIdentityModule(configuration);
            services.AddHRModule(configuration);
            services.AddITModule(configuration);
            services.AddOperationsModule(configuration);
            services.AddFinanceModule(configuration);

            ServiceProvider = services.BuildServiceProvider();
            _databaseFactory = ServiceProvider.GetRequiredService<IDatabaseFactory>();
        }

        [SetUp]
        public async Task SetupDatabase()
        {
            ResetDatabase();
            await SeedDatabaseAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            await ServiceProvider.DisposeAsync();
        }

        private void ResetDatabase()
        {
            var databaseBuilder =
                _databaseFactory.CreateDatabaseBuilder();

            var demoDatabase = databaseBuilder
                .IncludeDbContext<AuthDbContext>()
                .IncludeDbContext<HrDbContext>()
                .IncludeDbContext<ItDbContext>()
                .IncludeDbContext<OperationsDbContext>()
                .IncludeDbContext<FinanceDbContext>()
                .Build(
                    generateScripts: true,
                    applyChanges: true);

            demoDatabase.ExecuteDrop();
            demoDatabase.ExecuteCreate();
        }

        private async Task SeedDatabaseAsync()
        {
            var authDbContextFactory =
                ServiceProvider.GetRequiredService<IDbContextFactory<AuthDbContext>>();

            var hrDbContextFactory =
                ServiceProvider.GetRequiredService<IDbContextFactory<HrDbContext>>();

            await using var authDbContext =
                await authDbContextFactory.CreateDbContextAsync();

            await using var hrDbContext =
                await hrDbContextFactory.CreateDbContextAsync();

            var user = new AuthUser
            {
                UserName = "integration.employee@solidarcsoftware.com",
                NormalizedUserName = "INTEGRATION.EMPLOYEE@SOLIDARCSOFTWARE.COM",
                Email = "integration.employee@solidarcsoftware.com",
                NormalizedEmail = "INTEGRATION.EMPLOYEE@SOLIDARCSOFTWARE.COM",
                EmailConfirmed = true
            };

            authDbContext.Set<AuthUser>().Add(user);
            await authDbContext.SaveChangesAsync();

            var employer = new Employer
            {
                Name = "SolidArcSoftware",
                Description = "Software Development Company"
            };

            var department = new Department
            {
                Name = "Information Technology",
                Description = "Information Technology Department"
            };

            hrDbContext.Set<Employer>().Add(employer);
            hrDbContext.Set<Department>().Add(department);

            await hrDbContext.SaveChangesAsync();
        }
    }
}
