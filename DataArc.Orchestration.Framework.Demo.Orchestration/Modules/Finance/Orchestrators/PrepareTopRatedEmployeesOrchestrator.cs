using DataArc.Core;
using DataArc.Orchestrator;

using DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.UseCases;
using DataArc.Orchestration.Framework.Demo.Persistence.Contracts;
using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;

namespace DataArc.Orchestration.Framework.Demo.Orchestration.Orchestrators
{
    public class PrepareTopRatedEmployeesOrchestrator : Orchestrator<PrepareTopRatedEmployeesInput, PrepareTopRatedEmployeesOutput>
    {
        private readonly IQueryFactory _queryFactory;
        public PrepareTopRatedEmployeesOrchestrator(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        public override async Task<PrepareTopRatedEmployeesOutput> ExecuteAsync(
            PrepareTopRatedEmployeesInput input, PrepareTopRatedEmployeesOutput output)
        {
            var topRatedEmployeesQuery = await _queryFactory.CreateQueryAsync();

            var topRatedEmployees = await topRatedEmployeesQuery
                .UseDbExecutionContext<IHrDbContext, Employee>(e => e.Rating > input.Rating)
                    .Join<IFinanceDbContext, Employee>
                        (bag => bag.Get<Employee>()!.Id, f => f.Id)
                    .Join<IItDbContext, Employee>
                        (bag => bag.Get<Employee>()!.Id, i => i.Id)
                    .Join<IOperationsDbContext, Employee>(bag => bag.Get<Employee>()!.Id, o => o.Id)
                .Select(bag => new PreparedEmployeeSalaryAdjustment
                {
                    EmployeeId = bag.Get<Employee>()!.Id,
                    Name = bag.Get<Employee>()!.Name,
                    Surname = bag.Get<Employee>()!.Surname,
                    CurrentSalary = bag.Get<Employee>()!.Salary,
                    Rating = bag.Get<Employee>()!.Rating
                }).ToListAsync();

            output.Employees = topRatedEmployees;
            return output;
        }
    }
}