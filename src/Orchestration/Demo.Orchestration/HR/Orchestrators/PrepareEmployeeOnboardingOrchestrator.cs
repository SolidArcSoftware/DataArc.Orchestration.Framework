using DataArc.Orchestrator;

using Demo.Orchestration.HR.Orchestrators.Input;
using Demo.Orchestration.HR.Orchestrators.Output;
using Demo.Persistence.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Demo.Orchestration.HR.Orchestrators
{
    public sealed class PrepareEmployeeOnboardingOrchestrator
        : Orchestrator<PrepareEmployeeOnboardingInput, PrepareEmployeeOnboardingOutput>
    {
        private readonly ILogger<PrepareEmployeeOnboardingOrchestrator> _logger;
        private readonly IDbContextFactory<AuthDbContext> _authDbContextFactory;
        private readonly IDbContextFactory<HrDbContext> _hrDbContextFactory;
        private readonly IDbContextFactory<FinanceDbContext> _financeDbContextFactory;
        private readonly IDbContextFactory<ItDbContext> _itDbContextFactory;
        private readonly IDbContextFactory<OperationsDbContext> _operationsDbContextFactory;

        public PrepareEmployeeOnboardingOrchestrator(
            IDbContextFactory<AuthDbContext> authDbContextFactory,
            IDbContextFactory<HrDbContext> hrDbContextFactory,
            IDbContextFactory<FinanceDbContext> financeDbContextFactory,
            IDbContextFactory<ItDbContext> itDbContextFactory,
            IDbContextFactory<OperationsDbContext> operationsDbContextFactory,
            ILogger<PrepareEmployeeOnboardingOrchestrator> logger)
        {
            _authDbContextFactory = authDbContextFactory;
            _hrDbContextFactory = hrDbContextFactory;
            _financeDbContextFactory = financeDbContextFactory;
            _itDbContextFactory = itDbContextFactory;
            _operationsDbContextFactory = operationsDbContextFactory;
            _logger = logger;
        }

        public override async Task<PrepareEmployeeOnboardingOutput> ExecuteAsync(
            PrepareEmployeeOnboardingInput input,
            PrepareEmployeeOnboardingOutput output)
        {
            try
            {
                /*
                 * Identity is the starting point for employee onboarding.
                 */
                await using var authDbContext =
                    await _authDbContextFactory.CreateDbContextAsync();

                var user = await authDbContext.Users
                    .AsNoTracking()
                    .Where(user => user.Id == input.UserId)
                    .Select(user => new
                    {
                        user.Id,
                        user.UserName,
                        user.Email
                    })
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    output.IsSuccess = true;
                    output.UserExists = false;

                    return output;
                }

                output.UserExists = true;
                output.UserId = user.Id;
                output.UserName = user.UserName;
                output.EmailAddress = user.Email;

                /*
                 * Determine whether HR has already materialised an Employee
                 * for the selected Identity user.
                 */
                await using var hrDbContext =
                    await _hrDbContextFactory.CreateDbContextAsync();

                var employee = await hrDbContext.Employee!
                    .AsNoTracking()
                    .Where(employee =>
                        employee.UserId == input.UserId)
                    .Select(employee => new
                    {
                        employee.Id,
                        employee.OnBoardingStatus,
                        employee.IsArchived
                    })
                    .FirstOrDefaultAsync();

                /*
                 * No Employee is the normal starting state for a new
                 * onboarding request.
                 */
                if (employee == null)
                {
                    output.IsSuccess = true;
                    output.EmployeeExists = false;

                    return output;
                }

                output.EmployeeExists = true;
                output.EmployeeId = employee.Id;
                output.OnBoardingStatus = employee.OnBoardingStatus;
                output.IsArchived = employee.IsArchived;

                /*
                 * An Employee already exists, so gather the current
                 * onboarding state across the remaining persistence
                 * boundaries for policy evaluation.
                 */
                await using var financeDbContext =
                    await _financeDbContextFactory.CreateDbContextAsync();

                await using var itDbContext =
                    await _itDbContextFactory.CreateDbContextAsync();

                await using var operationsDbContext =
                    await _operationsDbContextFactory.CreateDbContextAsync();

                var hasDepartmentTask =
                    hrDbContext.EmployeeDepartment!
                        .AsNoTracking()
                        .AnyAsync(employeeDepartment =>
                            employeeDepartment.EmployeeId == employee.Id);

                var payrollRecordExistsTask =
                    financeDbContext.PayrollRecord!
                        .AsNoTracking()
                        .AnyAsync(payrollRecord =>
                            payrollRecord.EmployeeId == employee.Id);

                var accessRequestExistsTask =
                    itDbContext.AccessRequest!
                        .AsNoTracking()
                        .AnyAsync(accessRequest =>
                            accessRequest.EmployeeId == employee.Id);

                var onboardingTaskExistsTask =
                    operationsDbContext.OnboardingTask!
                        .AsNoTracking()
                        .AnyAsync(onboardingTask =>
                            onboardingTask.EmployeeId == employee.Id);

                await Task.WhenAll(
                    hasDepartmentTask,
                    payrollRecordExistsTask,
                    accessRequestExistsTask,
                    onboardingTaskExistsTask);

                output.HasDepartment =
                    await hasDepartmentTask;

                output.PayrollRecordExists =
                    await payrollRecordExistsTask;

                output.AccessRequestExists =
                    await accessRequestExistsTask;

                output.OnboardingTaskExists =
                    await onboardingTaskExistsTask;

                output.IsSuccess = true;

                return output;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Failed to prepare employee onboarding for user {UserId}.",
                    input.UserId);

                output.IsSuccess = false;
                output.FailureReason =
                    "Employee onboarding preparation could not be completed.";

                return output;
            }
        }
    }
}