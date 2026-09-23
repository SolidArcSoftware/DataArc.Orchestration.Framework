using Microsoft.EntityFrameworkCore.Storage;

using DataArc.Core;
using Demo.Persistence.DbContexts;

namespace Demo.Persistence.Database.Creator
{
    internal class DatabaseCreator : IDatabaseCreator
    {
        readonly IDatabaseFactory _databaseFactory;
        public DatabaseCreator(IDatabaseFactory databaseFactory)
        {
            _databaseFactory = databaseFactory;
        }

        public bool CanConnect()
        {
            throw new NotImplementedException();
        }

        public Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public bool EnsureCreated()
        {

           
            return true;
        }

        public Task<bool> EnsureCreatedAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public bool EnsureDeleted()
        {
            //Drop database using isolated dbcontexts
            var databaseBuilder = _databaseFactory.CreateDatabaseBuilder();

            var demoDatabase = databaseBuilder
                .IncludeDbContext<HrDbContext>()
                .IncludeDbContext<ItDbContext>()
                .IncludeDbContext<OperationsDbContext>()
                .IncludeDbContext<FinanceDbContext>()
                .Build(applyChanges: true);

            demoDatabase.ExecuteDrop();
            return true;
        }

        public Task<bool> EnsureDeletedAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}