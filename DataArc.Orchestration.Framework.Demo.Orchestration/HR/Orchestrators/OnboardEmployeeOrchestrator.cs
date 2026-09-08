using DataArc.Core;
using DataArc.Orchestrator;
using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators.Ouput;
using DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators.Input;
using DataArc.Orchestration.Framework.Demo.Persistence.DbContexts;

namespace DataArc.Orchestration.Framework.Demo.Orchestration.HR.Orchestrators
{
    public sealed class OnboardEmployeeOrchestrator
        : Orchestrator<OnboardEmployeeInput, OnboardEmployeeOutput>
    {
        private readonly IQueryFactory _queryFactory;
        private readonly ICommandFactory _commandFactory;

        public OnboardEmployeeOrchestrator(
            IQueryFactory queryFactory,
            ICommandFactory commandFactory)
        {
            _queryFactory = queryFactory;
            _commandFactory = commandFactory;
        }

        public override async Task<OnboardEmployeeOutput> ExecuteAsync(
            OnboardEmployeeInput input,
            OnboardEmployeeOutput output)
        {
            //try
            //{
            //    var employeeOnboardingStateQuery = await _queryFactory
            //        .CreateQueryAsync();

            //    var employeeOnboardingStates = await employeeOnboardingStateQuery
            //        .UseDbExecutionContext<HrDbContext, Employee>(employee =>
            //            employee.Id == input.EmployeeId)
            //        .JoinLeft<FinanceDbContext, PayrollRecord>(
            //            bag => bag.Get<Employee>()!.Id,
            //            payrollRecord => payrollRecord.EmployeeId)
            //        .JoinLeft<ItDbContext, AccessRequest>(
            //            bag => bag.Get<Employee>()!.Id,
            //            accessRequest => accessRequest.EmployeeId)
            //        .JoinLeft<OperationsDbContext, OnboardingTask>(
            //            bag => bag.Get<Employee>()!.Id,
            //            onboardingTask => onboardingTask.EmployeeId)
            //        .Select(bag => new
            //        {
            //            Employee = bag.Get<Employee>(),
            //            PayrollRecord = bag.Get<PayrollRecord>(),
            //            AccessRequest = bag.Get<AccessRequest>(),
            //            OnboardingTask = bag.Get<OnboardingTask>()
            //        })
            //        .ToListAsync();

            //    var employeeOnboardingState = employeeOnboardingStates
            //        .FirstOrDefault();

            //    if (employeeOnboardingState?.Employee == null)
            //    {
            //        output.IsSuccess = false;
            //        output.FailureReason = "Employee onboarding could not be started because the employee was not found.";

            //        return output;
            //    }

            //    if (employeeOnboardingState.PayrollRecord != null
            //        || employeeOnboardingState.AccessRequest != null
            //        || employeeOnboardingState.OnboardingTask != null)
            //    {
            //        output.IsSuccess = false;
            //        output.FailureReason = "Employee onboarding could not be started because onboarding records already exist.";
            //        output.PayrollRecordId = employeeOnboardingState!.PayrollRecord!.Id;

            //        return output;
            //    }

            //    var createdOnUtc = DateTimeOffset.UtcNow;

            //    var payrollRecord = new PayrollRecord
            //    {
            //        EmployeeId = input.EmployeeId,
            //        AnnualSalary = input.AnnualSalary,
            //        CurrencyCode = input.CurrencyCode,
            //        CreatedOnUtc = createdOnUtc,
            //        IsActive = true
            //    };

            //    var accessRequest = new AccessRequest
            //    {
            //        EmployeeId = input.EmployeeId,
            //        AccessLevel = "Standard",
            //        EmailAddress = $"employee-{input.EmployeeId}@solidarcsoftware.com",
            //        RequestStatus = "Requested",
            //        RequestedOnUtc = createdOnUtc,
            //        CompletedOnUtc = null
            //    };

            //    var onboardingTask = new OnboardingTask
            //    {
            //        EmployeeId = input.EmployeeId,
            //        TaskName = "Complete employee onboarding",
            //        TaskStatus = "Created",
            //        CreatedOnUtc = createdOnUtc,
            //        DueDateUtc = input.EffectiveOnUtc,
            //        CompletedOnUtc = null
            //    };

            //    employeeOnboardingState.Employee.OnBoardingStatus = "Active";

            //    var onboardingCommandBuilder = await _commandFactory
            //        .CreateTransactionalCommandBuilderAsync();

            //    onboardingCommandBuilder
            //        .UseDbExecutionContext<FinanceDbContext>()
            //        .Add(payrollRecord);

            //    onboardingCommandBuilder
            //        .UseDbExecutionContext<ItDbContext>()
            //        .Add(accessRequest);

            //    onboardingCommandBuilder
            //        .UseDbExecutionContext<OperationsDbContext>()
            //        .Add(onboardingTask);

            //    onboardingCommandBuilder
            //        .UseDbExecutionContext<HrDbContext>()
            //        .Update(employeeOnboardingState.Employee);

            //    var onboardingCommand = await onboardingCommandBuilder
            //        .BuildAsync();

            //    var onboardingResult = await onboardingCommand
            //        .CommitTransactionAsync();

            //    if (!onboardingResult.Success)
            //    {
            //        output.IsSuccess = false;
            //        output.FailureReason = onboardingResult.Exception?.Message
            //            ?? "Employee onboarding transaction could not be committed.";

            //        return output;
            //    }

            //    output.IsSuccess = true;
            //    output.FailureReason = null;
            //    output.PayrollRecordId = payrollRecord.Id;

            //    return output;
            //}
            //catch (Exception exception)
            //{
            //    output.IsSuccess = false;
            //    output.FailureReason = exception.Message;

            //    return output;
            //}

            throw new NotImplementedException();
        }
    }
}