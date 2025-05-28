using Microsoft.AspNetCore.Mvc;
using API.ApiService;
using API.ApiService.DB;
using API.ApiService.Models;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// === Rejestracja zale¿noœci ===
builder.Services.AddScoped<ReservationRepo>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<SalaRepo>();
builder.Services.AddScoped<SalaService>();

// Dodaj MySQL
builder.AddMySqlDataSource("MyDatabase", options =>
{
    options.ConnectionString = "Server=localhost;Database=san;User ID=oskar;Password=1234;Port=3306;";
});



// === CORS dla po³¹czenia z frontem ===
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

app.UseCors(); // U¿yj CORS przed jakimikolwiek endpointami

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();


// === Minimal API ===
var rezerwacje = app.MapGroup("/api/rezerwacje");

rezerwacje.MapPost("/rezerwuj", async (
    [FromBody] RezerwacjaRequest request, // <-- u¿yj RezerwacjaRequest
    ReservationService reservationService) =>
{
    var sukces = await reservationService.ZarezerwujSaleAsync(
        request.NumerSali,
        request.Login,
        request.DataOd,
        request.DataDo
    );

    return sukces
        ? Results.Ok(new { message = "Sala zarezerwowana pomyœlnie!" })
        : Results.Conflict(new { message = "Sala jest ju¿ zajêta w tym terminie." });
});

rezerwacje.MapDelete("/usun/{idRezerwacji}", async (
    int idRezerwacji,
    ReservationService reservationService) =>
{
    var sukces = await reservationService.UsunRezerwacjeAsync(idRezerwacji);
    return sukces
        ? Results.Ok(new { message = "Rezerwacja usuniêta." })
        : Results.NotFound(new { message = "Nie znaleziono rezerwacji." });
});

rezerwacje.MapPut("/edytuj/{idRezerwacji}", async (
    int idRezerwacji,
    [FromBody] EdytujRezerwacjeRequest request,
    ReservationService reservationService) =>
{
    var sukces = await reservationService.EdytujRezerwacjeAsync(idRezerwacji, request.NowaDataOd, request.NowaDataDo);
    return sukces
        ? Results.Ok(new { message = "Rezerwacja zaktualizowana." })
        : Results.Conflict(new { message = "Nie uda³o siê zaktualizowaæ rezerwacji." });
});
var sale = app.MapGroup("/api/sale");

sale.MapGet("/", async (SalaService service) =>
{
    var wynik = await service.PobierzSaleAsync();
    return Results.Ok(wynik);
});

sale.MapPut("/{id}", async (int id, [FromBody] Sala sala, SalaService service) =>
{
    if (id != sala.Id) return Results.BadRequest();
    var sukces = await service.ZapiszSaleAsync(sala);
    return sukces ? Results.Ok() : Results.NotFound();
});


app.Run();

