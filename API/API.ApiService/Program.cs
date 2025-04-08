using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using API.ApiService.DB;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddMySqlDataSource("MyDatabase", builder =>
{
    builder.ConnectionString = "Server=localhost;Database=san;User=oskar;Password=1234;";
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapGet("/weatherforecast", async ([FromServices] MySqlDataSource db) =>
{
    var repository = new SaleRepo(db);
    return await repository.GetSaleAsync();
});

app.MapDefaultEndpoints();

app.Run();
