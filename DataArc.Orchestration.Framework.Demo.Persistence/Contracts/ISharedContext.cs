using Microsoft.EntityFrameworkCore;
using DataArc.Core;

using DataArc.Orchestration.Framework.Demo.Persistence.Database.DBModels;

namespace DataArc.Orchestration.Framework.Demo.Persistence.Contracts
{
    public interface ISharedContext : IExecutionContext
    {
        DbSet<EmployeeOnboardingTask> EmployeeOnboarding { get; set; }
        DbSet<EmployeeAccessRequest> EmployeeAccessRequest { get; set; }
        DbSet<EmployeePayrollRecord> EmployeePayrollRecord { get; set; }
    }
}