using DataArc.Core;
using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataArc.Orchestration.Framework.Demo.Persistence.Contracts
{
    public interface IItDbContext : IExecutionContext
    {
        public DbSet<AccessRequest>? AccessRequest { get; set; }
    }
}