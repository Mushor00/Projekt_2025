using Google.Protobuf.WellKnownTypes;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);
var sensitiveValue = "5bGSj{+sDRVDgt844cwrQc";

//var password = builder.AddParameter("5bGSj%%7B%2BsDRVDgt844cwrQc", secret: true);
var passwordParameter = builder.AddParameter("MojBezpiecznyParametrHasla", sensitiveValue, secret: true);


var mysql = builder.AddMySql("mysql", passwordParameter)
    .WithLifetime(ContainerLifetime.Persistent);

var mysqldb = mysql.AddDatabase("san");

var apiService = builder.AddProject<Projects.API_ApiService>("apiservice")
    .WithReference(mysqldb)
    .WaitFor(mysqldb);


builder.AddProject<Projects.API_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(mysqldb)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
