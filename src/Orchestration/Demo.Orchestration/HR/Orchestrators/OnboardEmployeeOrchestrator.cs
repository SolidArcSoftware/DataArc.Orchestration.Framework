using DataArc.Orchestrator;

using Demo.Orchestration.HR.Orchestrators.Input;
using Demo.Orchestration.HR.Orchestrators.Ouput;
using Demo.Persistence.DbContexts;
using Demo.Persistence.DbModels;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Demo.Orchestration.HR.Orchestrators
{
    public sealed class OnboardEmployeeOrchestrator
        : Orchestrator<OnboardEmployeeInput, OnboardEmployeeOutput>
    {
        private readonly IDbContextFactory<FinanceDbContext> _financeDbContextFactory;
        private readonly IDbContextFactory<HrDbContext> _hrDbContextFactory;
        private readonly IDbContextFactory<ItDbContext> _itDbContextFactory;
        private readonly IDbContextFactory<OperationsDbContext> _operationsDbContextFactory;
        private readonly ILogger<OnboardEmployeeOrchestrator> _logger;

        public OnboardEmployeeOrchestrator(
            IDbContextFactory<FinanceDbContext> financeDbContextFactory,
            IDbContextFactory<HrDbContext> hrDbContextFactory,
            IDbContextFactory<ItDbContext> itDbContextFactory,
            IDbContextFactory<OperationsDbContext> operationsDbContextFactory,
            ILogger<OnboardEmployeeOrchestrator> logger)
        {
            _financeDbContextFactory = financeDbContextFactory;
            _hrDbContextFactory = hrDbContextFactory;
            _itDbContextFactory = itDbContextFactory;
            _operationsDbContextFactory = operationsDbContextFactory;
            _logger = logger;
        }

        public override async Task<OnboardEmployeeOutput> ExecuteAsync(
            OnboardEmployeeInput input,
            OnboardEmployeeOutput output)
        {
            await using var hrDbContext =
                await _hrDbContextFactory.CreateDbContextAsync();

            await using var financeDbContext =
                await _financeDbContextFactory.CreateDbContextAsync();

            await using var itDbContext =
                await _itDbContextFactory.CreateDbContextAsync();

            await using var operationsDbContext =
                await _operationsDbContextFactory.CreateDbContextAsync();

            IDbContextTransaction? transaction = null;

            try
            {
                /*
                 * All DbContexts represent application boundaries inside
                 * the same physical relational database.
                 *
                 * HR owns the physical connection and transaction.
                 * Finance, IT and Operations participate in the same
                 * local SQL Server transaction.
                 */
                transaction =
                    await hrDbContext.Database.BeginTransactionAsync();

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

                await financeDbContext.Database.UseTransactionAsync(
                    transaction.GetDbTransaction());

                await itDbContext.Database.UseTransactionAsync(
                    transaction.GetDbTransaction());

                await operationsDbContext.Database.UseTransactionAsync(
                    transaction.GetDbTransaction());

                /*
                 * The onboarding flow starts from an Identity UserId.
                 *
                 * The selected user must not already have an HR Employee
                 * record. The policy should normally prevent this path,
                 * but the orchestrator also protects the persistence boundary.
                 */
                var employeeExists =
                    await hrDbContext.Employee!
                        .AsNoTracking()
                        .AnyAsync(employee =>
                            employee.UserId == input.UserId);

                if (employeeExists)
                {
                    await transaction.RollbackAsync();

                    output.IsSuccess = false;
                    output.FailureReason =
                        "The selected user has already been onboarded as an employee.";

                    return output;
                }

                /*
                 * Resolve the demo HR reference data used by the
                 * onboarding workflow.
                 */
                var employer =
                    await hrDbContext.Set<Employer>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(employer =>
                            employer.Name == "SolidArcSoftware");

                if (employer == null)
                {
                    await transaction.RollbackAsync();

                    output.IsSuccess = false;
                    output.FailureReason =
                        "The demo employer could not be found.";

                    return output;
                }

                var department =
                    await hrDbContext.Set<Department>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(department =>
                            department.Name == "Information Technology");

                if (department == null)
                {
                    await transaction.RollbackAsync();

                    output.IsSuccess = false;
                    output.FailureReason =
                        "The demo department could not be found.";

                    return output;
                }

                var createdUtc = DateTime.UtcNow;
                DateTimeOffset? createdOnUtc = DateTimeOffset.UtcNow;

                /*
                 * Materialise the HR Employee from the Identity user
                 * selected by UserId.
                 */
                var employee = new Employee
                {
                    UserId = input.UserId,
                    Salary = input.AnnualSalary,
                    EmployerId = employer.Id,
                    IsArchived = false,
                    CreatedUtc = createdUtc,
                    LastUpdatedUtc = createdOnUtc,
                    Notes = input.Reason,
                    OnBoardingStatus = "Completed"
                };

                await hrDbContext.Employee!
                    .AddAsync(employee);

                /*
                 * Persist the Employee first so SQL Server generates
                 * Employee.Id.
                 *
                 * This insert remains inside the same uncommitted
                 * transaction.
                 */
                await hrDbContext.SaveChangesAsync();

                /*
                 * The generated EmployeeId is now used by the remaining
                 * records participating in the onboarding workflow.
                 */
                var employeeDepartment = new EmployeeDepartment
                {
                    EmployeeId = employee.Id,
                    DepartmentId = department.Id
                };

                var payrollRecord = new PayrollRecord
                {
                    EmployeeId = employee.Id,
                    AnnualSalary = input.AnnualSalary,
                    CurrencyCode = input.CurrencyCode,
                    CreatedOnUtc = createdOnUtc,
                    IsActive = true
                };

                var accessRequest = new AccessRequest
                {
                    EmployeeId = employee.Id,
                    AccessLevel = "Standard",
                    EmailAddress =
                        $"employee-{employee.Id}@solidarcsoftware.com",
                    RequestStatus = "Requested",
                    RequestedOnUtc = createdOnUtc
                };

                var onboardingTask = new OnboardingTask
                {
                    EmployeeId = employee.Id,
                    TaskName = "Complete employee onboarding",
                    TaskStatus = "Created",
                    CreatedOnUtc = createdOnUtc,
                    DueDateUtc = input.EffectiveOnUtc
                };

                /*
                 * Register the remaining changes across the participating
                 * persistence boundaries.
                 */
                await hrDbContext.EmployeeDepartment!
                    .AddAsync(employeeDepartment);

                await financeDbContext.PayrollRecord!
                    .AddAsync(payrollRecord);

                await itDbContext.AccessRequest!
                    .AddAsync(accessRequest);

                await operationsDbContext.OnboardingTask!
                    .AddAsync(onboardingTask);

                /*
                 * Persist sequentially because every participating
                 * DbContext shares the same physical connection and
                 * local transaction.
                 */
                await hrDbContext.SaveChangesAsync();
                await financeDbContext.SaveChangesAsync();
                await itDbContext.SaveChangesAsync();
                await operationsDbContext.SaveChangesAsync();

                /*
                 * Nothing becomes permanent until every participating
                 * DbContext has completed successfully.
                 */
                await transaction.CommitAsync();

                output.IsSuccess = true;
                output.EmployeeId = employee.Id;
                output.PayrollRecordId = payrollRecord.Id;

                return output;
            }
            catch (OperationCanceledException)
            {
                if (transaction != null)
                {
                    try
                    {
                        await transaction.RollbackAsync();
                    }
                    catch (Exception rollbackException)
                    {
                        _logger.LogError(
                            rollbackException,
                            "Rollback failed after employee onboarding was cancelled for user {UserId}.",
                            input.UserId);
                    }
                }

                throw;
            }
            catch (Exception exception)
            {
                if (transaction != null)
                {
                    try
                    {
                        await transaction.RollbackAsync();
                    }
                    catch (Exception rollbackException)
                    {
                        _logger.LogError(
                            rollbackException,
                            "Rollback failed after employee onboarding failed for user {UserId}.",
                            input.UserId);
                    }
                }

                _logger.LogError(
                    exception,
                    "Employee onboarding failed for user {UserId}.",
                    input.UserId);

                output.IsSuccess = false;
                output.FailureReason =
                    "Employee onboarding could not be completed.";

                return output;
            }
            finally
            {
                if (transaction != null)
                {
                    await transaction.DisposeAsync();
                }
            }
        }
    }
}