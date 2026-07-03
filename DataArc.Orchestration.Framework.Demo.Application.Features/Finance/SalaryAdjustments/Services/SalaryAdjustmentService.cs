using DataArc.Observer;
using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Policies;
using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Policies.Context;
using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.ValueObjects;
using DataArc.Orchestration.Framework.Demo.Application.Features.Finance.SalaryAdjustments.Dtos;
using DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.UseCases;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.Modules.Finance.Ports;
using DataArc.Orchestration.Framework.Demo.Modules.Finance.Features.SalaryAdjustments.Dtos;

namespace DataArc.Orchestration.Framework.Demo.Modules.Finance.Features.SalaryAdjustments.Services
{
    public class SalaryAdjustmentService : ISalaryAdjustmentService
    {
        private readonly IFinanceOrchestrationPort _financeOrchestrationPort;
        private readonly ISalaryAdjustmentPolicy _salaryAdjustmentPolicy;
        private readonly IObservableEventHandler _observableEventHandler;

        public SalaryAdjustmentService(
           IFinanceOrchestrationPort financeOrchestrationPort,
           ISalaryAdjustmentPolicy salaryAdjustmentPolicy,
           IObservableEventHandler observableEventHandler)
        {
            _financeOrchestrationPort = financeOrchestrationPort;
            _salaryAdjustmentPolicy = salaryAdjustmentPolicy;
            _observableEventHandler = observableEventHandler;
        }

        public async Task<List<SalaryAdjustmentCandidateResponseDto>> ProcessEmployeeSalaryAdjustmentsAsync(SalaryAdjustmentCandidateRequestDto request)
        {
            var result = await _financeOrchestrationPort.PrepareTopRatedEmployeesAsync(
                new PrepareTopRatedEmployeesInput(request.Rating));

            var salaryAdjustmentCriteria = new SalaryAdjustmentCriteriaValueObject(
                request.SalaryAdjustmentBaseRate,
                request.SalaryThreshold,
                request.BatchSize);

            foreach (var employee in result.PrepareTopRatedEmployees)
            {
                var salaryAdjustment = new SalaryAdjustmentValueObject(
                    employee.EmployeeId,
                    employee.Name,
                    employee.Surname,
                    employee.CurrentSalary,
                    employee.Rating);

                var policyContext = new SalaryAdjustmentPolicyContext(
                    salaryAdjustment,
                    salaryAdjustmentCriteria);

                var policyResult = _salaryAdjustmentPolicy.Apply(policyContext);

                await _observableEventHandler.DispatchAsync(policyResult.DomainEvents);
            }

            return result.PrepareTopRatedEmployees
                .Select(employee => new SalaryAdjustmentCandidateResponseDto
                {
                    //EmployeeId = employee.EmployeeId,
                    //Name = employee.Name,
                    //Surname = employee.Surname,
                    //CurrentSalary = employee.CurrentSalary,
                    //Rating = employee.Rating
                })
                .ToList();
        }
    }
}