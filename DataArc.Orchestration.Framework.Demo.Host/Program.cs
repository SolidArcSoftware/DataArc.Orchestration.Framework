using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using DataArc.Orchestration.Framework.Demo.Application.Modules;

var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        var configurationManager = new ConfigurationManager();

        configurationManager
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        services
            .AddHRModule(configurationManager)
            .AddHostedService<HrWorkerProcess>();
    })
    .Build();

using var scope = host.Services.CreateScope();
await host.RunAsync();