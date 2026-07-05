using DataArc.Orchestration.Framework.Demo.Modules.Finance.Features.SalaryAdjustments.Dtos;

namespace DataArc.Orchestration.Framework.Demo.Application.Features.Finance.Repository
{
    public interface IFinanceRepository
    {
        Task<IReadOnlyCollection<PayrollRecordDto>> GetPayrollRecordsAsync();
        Task<PayrollRecordDto?> GetPayrollRecordByIdAsync(int id);
        Task CreatePayrollRecordAsync(PayrollRecordDto payrollRecord);
    }

    public class PayrollRecordDto
    {
        public int Id { get; set; }
    }
}
