using DataArc.Orchestration.Framework.Demo.Application.Features.Finance.Repository;
using DataArc.Orchestration.Framework.Demo.Persistence.Contracts;

namespace DataArc.Orchestration.Framework.Demo.Application.Modules.Finance.Adapters
{
    internal sealed class FinanceRepository : IFinanceRepository
    {
        private readonly IFinanceDbContext _dbContext;

        public FinanceRepository(IFinanceDbContext financeDbContext)
        {
            _dbContext = financeDbContext;
        }

        public Task CreatePayrollRecordAsync(PayrollRecordDto payrollRecord)
        {
            throw new NotImplementedException();
        }

        public Task<PayrollRecordDto?> GetPayrollRecordByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<PayrollRecordDto>> GetPayrollRecordsAsync()
        {
            throw new NotImplementedException();
        }
    }
}