using DataArc.Orchestrator;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators.Input;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators.Ouput;
using DataArc.Orchestration.Framework.Demo.Persistence.DbContexts;
using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators
{
    public sealed class OnboardEmployeeOrchestrator
        : Orchestrator<OnboardEmployeeInput, OnboardEmployeeOutput>
    {
        private readonly IDbContextFactory<FinanceDbContext> _financeDbContextFactory;
        private readonly IDbContextFactory<HrDbContext> _hrDbContextFactory;
        private readonly IDbContextFactory<ItDbContext> _itDbContextFactory;
        private readonly IDbContextFactory<OperationsDbContext> _operationsDbContextFactory;

        public OnboardEmployeeOrchestrator(
            IDbContextFactory<FinanceDbContext> financeDbContextFactory,
            IDbContextFactory<HrDbContext> hrDbContextFactory,
            IDbContextFactory<ItDbContext> itDbContextFactory,
            IDbContextFactory<OperationsDbContext> operationsDbContextFactory)
        {
            _financeDbContextFactory = financeDbContextFactory;
            _hrDbContextFactory = hrDbContextFactory;
            _itDbContextFactory = itDbContextFactory;
            _operationsDbContextFactory = operationsDbContextFactory;
        }

        public override async Task<OnboardEmployeeOutput> ExecuteAsync(
            OnboardEmployeeInput input,
            OnboardEmployeeOutput output)
        {
            try
            {
                await using var hrDbContext =
                    await _hrDbContextFactory.CreateDbContextAsync();

                await using var financeDbContext =
                    await _financeDbContextFactory.CreateDbContextAsync();

                await using var itDbContext =
                    await _itDbContextFactory.CreateDbContextAsync();

                await using var operationsDbContext =
                    await _operationsDbContextFactory.CreateDbContextAsync();

                var employee = await hrDbContext
                    .Set<Employee>()
                    .FirstOrDefaultAsync(employee =>
                        employee.Id == input.EmployeeId);

                if (employee is null)
                {
                    output.IsSuccess = false;
                    output.FailureReason =
                        "Employee onboarding could not be started because the employee was not found.";

                    return output;
                }

                var payrollRecord = await financeDbContext
                    .Set<PayrollRecord>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(record =>
                        record.EmployeeId == input.EmployeeId);

                var accessRequest = await itDbContext
                    .Set<AccessRequest>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(request =>
                        request.EmployeeId == input.EmployeeId);

                var onboardingTask = await operationsDbContext
                    .Set<OnboardingTask>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(task =>
                        task.EmployeeId == input.EmployeeId);

                if (payrollRecord is not null
                    || accessRequest is not null
                    || onboardingTask is not null)
                {
                    output.IsSuccess = false;
                    output.FailureReason =
                        "Employee onboarding could not be started because onboarding records already exist.";

                    if (payrollRecord is not null)
                    {
                        output.PayrollRecordId = payrollRecord.Id;
                    }

                    return output;
                }

                var createdOnUtc = DateTimeOffset.UtcNow;

                payrollRecord = new PayrollRecord
                {
                    EmployeeId = input.EmployeeId,
                    AnnualSalary = input.AnnualSalary,
                    CurrencyCode = input.CurrencyCode,
                    CreatedOnUtc = createdOnUtc,
                    IsActive = true
                };

                accessRequest = new AccessRequest
                {
                    EmployeeId = input.EmployeeId,
                    AccessLevel = "Standard",
                    EmailAddress =
                        $"employee-{input.EmployeeId}@solidarcsoftware.com",
                    RequestStatus = "Requested",
                    RequestedOnUtc = createdOnUtc,
                    CompletedOnUtc = null
                };

                onboardingTask = new OnboardingTask
                {
                    EmployeeId = input.EmployeeId,
                    TaskName = "Complete employee onboarding",
                    TaskStatus = "Created",
                    CreatedOnUtc = createdOnUtc,
                    DueDateUtc = input.EffectiveOnUtc,
                    CompletedOnUtc = null
                };

                employee.OnBoardingStatus = "Active";

                /*
                 * All four DbContexts represent boundaries within the
                 * same physical relational database.
                 *
                 * Share the HR context's connection so that all four
                 * contexts can participate in one local transaction.
                 */
                var connection =
                    hrDbContext.Database.GetDbConnection();

                financeDbContext.Database.SetDbConnection(
                    connection,
                    contextOwnsConnection: false);

                itDbContext.Database.SetDbConnection(
                    connection,
                    contextOwnsConnection: false);

                operationsDbContext.Database.SetDbConnection(
                    connection,
                    contextOwnsConnection: false);

                await using var transaction =
                    await hrDbContext.Database.BeginTransactionAsync();

                var dbTransaction =
                    transaction.GetDbTransaction();

                await financeDbContext.Database.UseTransactionAsync(
                    dbTransaction);

                await itDbContext.Database.UseTransactionAsync(
                    dbTransaction);

                await operationsDbContext.Database.UseTransactionAsync(
                    dbTransaction);

                try
                {
                    financeDbContext
                        .Set<PayrollRecord>()
                        .Add(payrollRecord);

                    itDbContext
                        .Set<AccessRequest>()
                        .Add(accessRequest);

                    operationsDbContext
                        .Set<OnboardingTask>()
                        .Add(onboardingTask);

                    await financeDbContext.SaveChangesAsync();
                    await itDbContext.SaveChangesAsync();
                    await operationsDbContext.SaveChangesAsync();
                    await hrDbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }

                output.IsSuccess = true;
                output.FailureReason = null;
                output.PayrollRecordId = payrollRecord.Id;

                return output;
            }
            catch (Exception exception)
            {
                output.IsSuccess = false;
                output.FailureReason = exception.Message;

                return output;
            }
        }
    }
}