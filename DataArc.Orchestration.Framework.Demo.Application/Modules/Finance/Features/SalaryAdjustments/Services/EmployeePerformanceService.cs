using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Policies;
using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Policies.Context;
using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.ValueObjects;
using DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.UseCases;
using DataArc.Orchestration.Framework.Demo.Modules.Finance.Features.SalaryAdjustments.Dtos;
using DataArc.Orchestration.Framework.Demo.Orchestration.Orchestrators;

using DataArc.Orchestrator;

namespace DataArc.Orchestration.Framework.Demo.Modules.Finance.Features.SalaryAdjustments.Services
{
    internal class EmployeePerformanceService : IEmployeePerformanceService
    {
        private readonly IOrchestrator _orchestrator;
        private readonly ISalaryAdjustmentPolicy _salaryAdjustmentPolicy;

        public EmployeePerformanceService(
            IOrchestrator orchestrator,
            ISalaryAdjustmentPolicy salaryAdjustmentPolicy)
        {
            _orchestrator = orchestrator;
            _salaryAdjustmentPolicy = salaryAdjustmentPolicy;
        }

        public async Task<List<SalaryAdjustmentCandidateDto>> GetTopRatedEmployeesAsync(double rating)
        {
            var input = new PrepareTopRatedEmployeesInput(rating);
            var output = new PrepareTopRatedEmployeesOutput();

            var result = await _orchestrator
                .OrchestrateAsync<PrepareTopRatedEmployeesOrchestrator, PrepareTopRatedEmployeesOutput>(
                    input,
                    output);

            foreach (var employee in result.Employees)
            {
                var salaryAdjustment = new SalaryAdjustmentValueObject(
                    employee.EmployeeId,
                    employee.Name,
                    employee.Surname,
                    employee.CurrentSalary,
                    employee.Rating);

                var policyContext = new SalaryAdjustmentPolicyContext(salaryAdjustment);
                var policyResult = _salaryAdjustmentPolicy.Apply(policyContext);

                // TODO: collect or dispatch policy events through Observer.
            }

            return result.Employees
                .Select(employee => new SalaryAdjustmentCandidateDto
                {
                    EmployeeId = employee.EmployeeId,
                    Name = employee.Name,
                    Surname = employee.Surname,
                    CurrentSalary = employee.CurrentSalary,
                })
                .ToList();
        }
    }
}