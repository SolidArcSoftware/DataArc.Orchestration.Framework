using DataArc.Orchestration.Framework.Demo.Application.Features.Finance.SalaryAdjustments.Dtos;
using DataArc.Orchestration.Framework.Demo.Modules.Finance.Features.SalaryAdjustments.Dtos;

namespace DataArc.Orchestration.Framework.Demo.Modules.Finance.Features.SalaryAdjustments.Services
{
    public interface ISalaryAdjustmentService
    {
        Task<List<SalaryAdjustmentCandidateResponseDto>> ProcessEmployeeSalaryAdjustmentsAsync(SalaryAdjustmentCandidateRequestDto request);
    }
}