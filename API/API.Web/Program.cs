using API.Web;
using API.ApiService.DB;
using API.ApiService;
using API.Web.Components;
using MySqlConnector;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers(); // ? 
builder.Services.AddOutputCache();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });


builder.AddMySqlDataSource("san");

builder.Services.AddScoped<UserSessionService>();
builder.Services.AddAntiforgery();
builder.Services.AddScoped<ISaleApiClient, SaleApiClient>();
builder.Services.AddScoped<IOsobyService, OsobyService>();
builder.Services.AddSingleton<UserSessionService>();
builder.Services.AddScoped<ReservationRepo>();
builder.Services.AddScoped<ReservationService>();

// front
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddScoped<IReservationApiClient, ReservationApiClient>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles(); // je�li nie masz, dodaj to

app.UseRouting();

app.UseAntiforgery(); // ? KLUCZOWE!

app.MapRazorComponents<App>() // ? wa�ne: App z przestrzeni namespace twojego projektu
   .AddInteractiveServerRenderMode();

app.Run();
