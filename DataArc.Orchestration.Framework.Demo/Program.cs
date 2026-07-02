using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using DataArc.Orchestration.Framework.Demo.Persistence;
using DataArc.Orchestration.Framework.Demo.Workers;
using DataArc.Orchestration.Framework.Demo.Application.Registration;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var configurationManager = new ConfigurationManager();

        configurationManager
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        services
            .AddFinanceModule(configurationManager)
            .AddHostedService<DemoWorkflowWorker>();
    })
    .Build();

using var scope = host.Services.CreateScope();
await DemoDatabaseInitializer.InitializeAsync(scope.ServiceProvider);
await host.RunAsync();

Console.WriteLine();
Console.WriteLine("Press any key to exit.");
Console.ReadKey();