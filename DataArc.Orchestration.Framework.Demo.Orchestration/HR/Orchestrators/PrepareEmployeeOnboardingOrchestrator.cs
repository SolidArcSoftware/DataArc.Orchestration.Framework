using DataArc.Core;
using DataArc.Orchestrator;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Ouput;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Input;
using DataArc.Orchestration.Framework.Demo.Persistence.Contracts;
using DataArc.Orchestration.Framework.Demo.Persistence.Database.DBModels;
using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;

namespace DataArc.Orchestration.Framework.Demo.Orchestration
{
    public class PrepareEmployeeOnboardingOrchestrator
        : Orchestrator<PrepareEmployeeOnboardingInput, PrepareEmployeeOnbardingOutput>
    {
        readonly IQueryFactory _queryFactory;
        public PrepareEmployeeOnboardingOrchestrator(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        public override async Task<PrepareEmployeeOnbardingOutput> ExecuteAsync(
            PrepareEmployeeOnboardingInput input,
            PrepareEmployeeOnbardingOutput output)
        {
            try
            {
                var employeesQuery = await _queryFactory.CreateQueryAsync();
                var employees = await employeesQuery.UseDbExecutionContext<IHrDbContext>()
                    .ReadWhereAsync<Employee>(employee => employee.Id == input.EmployeeId);

                var employee = employees?.FirstOrDefault();

                if (employee == null)
                {
                    output.IsSuccess = false;
                    output.FailureReason = "Employee onboarding context could not be prepared because the employee was not found.";

                    return output;
                }

                output.EmployeeId = employee.Id;
                output.EmployeeName = employee.Name;
                output.EmployeeNameSurname = employee.Surname;
                output.Status = employee.Status;
                output.Rating = employee.Rating;
                output.EmployeeSalary = employee.Salary;

                var employeePayrollQuery = await _queryFactory.CreateQueryAsync();
                var employeePayrollRecords = await employeePayrollQuery.UseDbExecutionContext<ISharedContext>()
                    .ReadWhereAsync<EmployeePayrollRecord>(employeePayrollRecord =>
                        employeePayrollRecord.EmployeeId == input.EmployeeId);

                var employeePayrollRecord = employeePayrollRecords?.FirstOrDefault();
                if (employeePayrollRecord != null)
                {
                    output.PayrollRecordId = employeePayrollRecord.PayrollRecordId;

                    var payrollRecordQuery = await _queryFactory.CreateQueryAsync();
                    var payrollRecords = await payrollRecordQuery
                        .UseDbExecutionContext<IFinanceDbContext>()
                        .ReadWhereAsync<PayrollRecord>(payrollRecord =>
                            payrollRecord.Id == employeePayrollRecord.PayrollRecordId);

                    var payrollRecord = payrollRecords?.FirstOrDefault();

                    if (payrollRecord != null)
                    {
                        output.PayrollAnnualSalary = payrollRecord.AnnualSalary;
                        output.PayrollCurrencyCode = payrollRecord.CurrencyCode;
                        output.PayrollIsActive = payrollRecord.IsActive;
                    }
                }

                var employeeAccessRequestsQuery = await _queryFactory.CreateQueryAsync();
                var employeeAccessRequests = await employeeAccessRequestsQuery
                    .UseDbExecutionContext<ISharedContext>()
                    .ReadWhereAsync<EmployeeAccessRequest>(employeeAccessRequest =>
                        employeeAccessRequest.EmployeeId == input.EmployeeId);

                var employeeAccessRequest = employeeAccessRequests?.FirstOrDefault();

                if (employeeAccessRequest != null)
                {
                    output.AccessRequestId = employeeAccessRequest.AccessRequestId;

                    var accessRequestsQuery = await _queryFactory.CreateQueryAsync();
                    var accessRequests = await accessRequestsQuery
                        .UseDbExecutionContext<IItDbContext>()
                        .ReadWhereAsync<AccessRequest>(accessRequest =>
                            accessRequest.Id == employeeAccessRequest.AccessRequestId);

                    var accessRequest = accessRequests?.FirstOrDefault();
                    if (accessRequest != null)
                    {
                        output.AccessLevel = accessRequest.AccessLevel;
                        output.AccessRequestStatus = accessRequest.RequestStatus;
                        output.AccessRequestedOnUtc = accessRequest.RequestedOnUtc;
                        output.AccessCompletedOnUtc = accessRequest.CompletedOnUtc;
                    }
                }

                var employeeOnboardingTasksQuery = await _queryFactory.CreateQueryAsync();
                var employeeOnboardingTasks = await employeeOnboardingTasksQuery
                    .UseDbExecutionContext<ISharedContext>()
                    .ReadWhereAsync<EmployeeOnboardingTask>(employeeOnboardingTask =>
                        employeeOnboardingTask.EmployeeId == input.EmployeeId);

                var employeeOnboardingTask = employeeOnboardingTasks?.FirstOrDefault();

                if (employeeOnboardingTask != null)
                {
                    output.OnboardingTaskId = employeeOnboardingTask.OnBoardingTaskId;

                    var onboardingTasksQuery = await _queryFactory.CreateQueryAsync();
                    var onboardingTasks = await onboardingTasksQuery
                        .UseDbExecutionContext<IOperationsDbContext>()
                        .ReadWhereAsync<OnboardingTask>(onboardingTask =>
                            onboardingTask.Id == employeeOnboardingTask.OnBoardingTaskId);

                    var onboardingTask = onboardingTasks?.FirstOrDefault();

                    if (onboardingTask != null)
                    {
                        output.OnboardingTaskName = onboardingTask.TaskName;
                        output.OnboardingTaskStatus = onboardingTask.TaskStatus;
                        output.OnboardingTaskCreatedOnUtc = onboardingTask.CreatedOnUtc;
                        output.OnboardingTaskDueDateUtc = onboardingTask.DueDateUtc;
                        output.OnboardingTaskCompletedOnUtc = onboardingTask.CompletedOnUtc;
                    }
                }

                output.IsSuccess = true;
                output.FailureReason = null;

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