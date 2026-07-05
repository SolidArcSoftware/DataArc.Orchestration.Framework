using DataArc.Observer;
using DataArc.Orchestration.Framework.Demo.Application.Domain.HR.Events;
using DataArc.Orchestration.Framework.Demo.Application.Domain.SharedKernel;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Orchestration.Input;
using DataArc.Orchestration.Framework.Demo.Application.UseCases.HR.Ports;

namespace DataArc.Orchestration.Framework.Demo.Application.Features.HR.EmployeeImports.Services
{
    internal class EmployeeImportsService : IEmployeeImportsService
    {
        readonly IObservableEventHandler _observableEventHandler;
        public EmployeeImportsService(IObservableEventHandler observableEventHandler)
        {
            _observableEventHandler = observableEventHandler;
        }

        public async Task ImportEmployeeData()
        {
            var policyResult = PolicyResult.Success(new ImportEmployeeDataRequestAcceptedEvent(100_000, 100_000));
            await _observableEventHandler.DispatchAsync(policyResult.DomainEvents);
            //var output = await _hrOrchestrationPort
            //  .ImportEmployeesDataAsync(new ImportEmployeesInput()
            //  {
            //      ImportBatchSize = 100_000,
            //      ImportEmployeeCount = 100_000,
            //  });
        }
    }
}