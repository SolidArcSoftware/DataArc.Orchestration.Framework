using DataArc.Observer;
using DataArc.Orchestration.Framework.Demo.Application.Domain.Modules.Finance.Events;

namespace DataArc.Orchestration.Framework.Demo.Application.UseCases.Finance.Observers
{
    internal class EmployeeSalaryAdjustmentAcceptedObserver : IEventObserver<EmployeeSalaryAdjustmentAcceptedEvent>
    {
        public EmployeeSalaryAdjustmentAcceptedObserver()
        {

        }

        public Task HandleAsync(EmployeeSalaryAdjustmentAcceptedEvent evt)
        {
            throw new NotImplementedException();
        }
    }
}