using DataArc.Core;
using Microsoft.EntityFrameworkCore;

using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;
namespace DataArc.Orchestration.Framework.Demo.Persistence.Contracts
{
    public interface IHrDbContext : IExecutionContext
    {
        DbSet<Employer>? Employer { get; set; }
        DbSet<Employee>? Employee { get; set; }
    }
}