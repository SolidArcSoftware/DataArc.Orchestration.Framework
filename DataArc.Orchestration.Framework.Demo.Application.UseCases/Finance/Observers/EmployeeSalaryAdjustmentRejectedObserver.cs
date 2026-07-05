using DataArc.Observer;
using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Events;

namespace DataArc.Orchestration.Framework.Demo.Application.UseCases.Finance.Observers
{
    public class EmployeeSalaryAdjustmentRejectedObserver : IEventObserver<EmployeeSalaryAdjustmentRejectedEvent>
    {
        public Task HandleAsync(EmployeeSalaryAdjustmentRejectedEvent evt)
        {
            throw new NotImplementedException();
        }
    }
}