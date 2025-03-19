var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.API_ApiService>("apiservice");

builder.AddProject<Projects.API_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
