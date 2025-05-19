using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using API.ApiService.DB;

var builder = WebApplication.CreateBuilder(args);

// Dodaj integracje Aspire + problemy HTTP
builder.AddServiceDefaults();
builder.Services.AddProblemDetails();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dodaj MySQL
builder.AddMySqlDataSource("MyDatabase", options =>
{
    options.ConnectionString = "Server=localhost;Database=san;User ID=oskar;Password=1234;Port=3306;";
});

// Dodaj repozytorium (wstrzykiwanie zale¿noœci)
builder.Services.AddScoped<SaleRepo>();

var app = builder.Build();

// Middleware b³êdów
app.UseExceptionHandler();

// Swagger w trybie dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Endpoint testowy
app.MapGet("/api/sale", async ([FromServices] SaleRepo repo) =>
{
    var result = await repo.GetSaleAsync();
    return result is not null ? Results.Ok(result) : Results.NotFound();
});

// Domyœlne endpointy Aspire
app.MapDefaultEndpoints();

app.Run();
