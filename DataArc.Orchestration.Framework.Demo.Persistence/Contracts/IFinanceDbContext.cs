using Microsoft.EntityFrameworkCore;
using DataArc.Core;

using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;

namespace DataArc.Orchestration.Framework.Demo.Persistence.Contracts
{
    public interface IFinanceDbContext : IExecutionContext
    {
        DbSet<PayrollRecord>? PayrollRecord { get; set; }
    }
}