using Microsoft.AspNetCore.Mvc;
using API.ApiService;
using API.ApiService.DB;
using API.ApiService.Models;


var builder = WebApplication.CreateBuilder(args);

// === Rejestracja zależności ===
builder.Services.AddScoped<ReservationRepo>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<SalaRepo>();
builder.Services.AddScoped<SalaService>();
builder.Services.AddScoped<DatabaseInitializer>();

builder.AddMySqlDataSource(connectionName: "san");

// === CORS dla po��czenia z frontem ===
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// === Inicjalizacja bazy danych ===
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await initializer.InitializeAsync();
}

app.UseCors(); // U�yj CORS przed jakimikolwiek endpointami

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.Run();
