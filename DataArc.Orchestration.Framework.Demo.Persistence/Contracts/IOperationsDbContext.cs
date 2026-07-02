using DataArc.Core;
using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataArc.Orchestration.Framework.Demo.Persistence.Contracts
{
    public interface IOperationsDbContext : IExecutionContext
    {
        DbSet<OnboardingTask>? OnboardingTask { get; set; }
    }
}