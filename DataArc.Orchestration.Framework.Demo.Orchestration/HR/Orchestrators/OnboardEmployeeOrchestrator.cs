using DataArc.Core;
using DataArc.Orchestrator;
using DataArc.Orchestration.Framework.Demo.Persistence.Contracts;
using DataArc.Orchestration.Framework.Demo.Persistence.Database.DBModels;
using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Input;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Ouput;

namespace DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators
{
    public sealed class OnboardEmployeeOrchestrator
        : Orchestrator<OnboardEmployeeInput, OnboardEmployeeOutput>
    {
        private readonly ICommandFactory _commandFactory;
        public OnboardEmployeeOrchestrator(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        public override async Task<OnboardEmployeeOutput> ExecuteAsync(
            OnboardEmployeeInput input,
            OnboardEmployeeOutput output)
        {
            try
            {
                var createdOnUtc = DateTimeOffset.UtcNow;

                var payrollRecord = new PayrollRecord
                {
                    AnnualSalary = input.AnnualSalary,
                    CurrencyCode = input.CurrencyCode,
                    CreatedOnUtc = createdOnUtc,
                    IsActive = true
                };

                var accessRequest = new AccessRequest
                {
                    AccessLevel = "Standard",
                    EmailAddress = $"employee-{input.EmployeeId}@solidarcsoftware.demo",
                    RequestStatus = "Requested",
                    RequestedOnUtc = createdOnUtc,
                    CompletedOnUtc = null
                };

                var onboardingTask = new OnboardingTask
                {
                    TaskName = "Complete employee onboarding",
                    TaskStatus = "Created",
                    CreatedOnUtc = createdOnUtc,
                    DueDateUtc = input.EffectiveOnUtc,
                    CompletedOnUtc = null
                };

                var onboardingSetupCommandBuilder = await _commandFactory
                    .CreateTransactionalCommandBuilderAsync();

                onboardingSetupCommandBuilder
                    .UseDbExecutionContext<IFinanceDbContext>()
                        .Add(payrollRecord);

                onboardingSetupCommandBuilder
                    .UseDbExecutionContext<IItDbContext>()
                        .Add(accessRequest);

                onboardingSetupCommandBuilder
                    .UseDbExecutionContext<IOperationsDbContext>()
                        .Add(onboardingTask);

                var onboardingSetupCommand = await onboardingSetupCommandBuilder
                    .BuildAsync();

                var onboardingSetupResult = await onboardingSetupCommand
                    .CommitTransactionAsync();

                if (!onboardingSetupResult.Success)
                {
                    output.IsSuccess = false;
                    output.FailureReason = onboardingSetupResult.Exception?.Message
                        ?? "Employee onboarding setup records could not be created.";

                    return output;
                }

                var employeePayrollRecord = new EmployeePayrollRecord
                {
                    EmployeeId = input.EmployeeId,
                    PayrollRecordId = payrollRecord.Id
                };

                var employeeAccessRequest = new EmployeeAccessRequest
                {
                    EmployeeId = input.EmployeeId,
                    AccessRequestId = accessRequest.Id
                };

                var employeeOnboardingTask = new EmployeeOnboardingTask
                {
                    EmployeeId = input.EmployeeId,
                    OnBoardingTaskId = onboardingTask.Id
                };

                var employeeOnboardingLinksCommandBuilder = await _commandFactory
                    .CreateTransactionalCommandBuilderAsync();

                employeeOnboardingLinksCommandBuilder
                    .UseDbExecutionContext<ISharedContext>()
                        .Add(employeePayrollRecord)
                        .Add(employeeAccessRequest)
                        .Add(employeeOnboardingTask);

                var employeeOnboardingLinksCommand = await employeeOnboardingLinksCommandBuilder.BuildAsync();
                var employeeOnboardingLinksResult = await employeeOnboardingLinksCommand.CommitTransactionAsync();

                if (!employeeOnboardingLinksResult.Success)
                {
                    output.IsSuccess = false;
                    output.FailureReason = employeeOnboardingLinksResult.Exception?.Message
                        ?? "Employee onboarding link records could not be created.";

                    return output;
                }

                output.IsSuccess = true;
                output.FailureReason = null;
                output.PayrollRecordId = payrollRecord.Id;
                output.EmployeePayrollRecordId = employeePayrollRecord.Id;

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
