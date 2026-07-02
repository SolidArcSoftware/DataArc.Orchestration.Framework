using DataArc.Core;
using DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.Features.SalaryAdjustments.Dtos;
using DataArc.Orchestration.Framework.Demo.Persistence.Contracts;
using DataArc.Orchestration.Framework.Demo.Persistence.DbModels;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.Features.SalaryAdjustments.Services
{
    internal class EmployeePerformanceService : IEmployeePerformanceService
    {
        private readonly IQueryFactory _queryFactory;
        public EmployeePerformanceService(IQueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        public async Task<List<EmployeeDto>> GetTopRatedEmployeesAsync(double rating)
        {
            var topRatedEmployeesQuery = await _queryFactory.CreateQueryAsync();

            var topRatedEmployees = await topRatedEmployeesQuery
                .UseDbExecutionContext<IHrDbContext, Employee>(e => e.Rating > rating)
                    .Join<IFinanceDbContext, Employee>
                        (bag => bag.Get<Employee>()!.Id, f => f.Id)
                    .Join<IItDbContext, Employee>
                        (bag => bag.Get<Employee>()!.Id, i => i.Id)
                    .Join<IOperationsDbContext, Employee>(bag => bag.Get<Employee>()!.Id, o => o.Id)
                .Select(bag => new EmployeeDto()
                {
                    Id = bag.Get<Employee>()!.Id,
                    Name = bag.Get<Employee>()!.Name!,
                    Surname = bag.Get<Employee>()!.Surname!,
                    Salary = bag.Get<Employee>()!.Salary!

                }).ToListAsync();

            return topRatedEmployees;
        }
    }
}