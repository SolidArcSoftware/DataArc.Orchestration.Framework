var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Demo_WebApi>(
    "apiservice");

builder.AddProject<Projects.Demo_Host_Aspire_Web>(
        "webfrontend")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();