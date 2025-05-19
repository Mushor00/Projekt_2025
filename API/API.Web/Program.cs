using API.Web;
using API.ApiService.DB;
using API.ApiService;
using API.Web.Components;
using MySqlConnector;
using Microsoft.OpenApi.Models; // to do swaggera
var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers(); // ? 
builder.Services.AddOutputCache();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });


builder.AddMySqlDataSource("MyDatabase", builder =>
{
    builder.ConnectionString = "Server=localhost;Database=san;User ID=root;Password=;Port=3306;";
});


builder.Services.AddScoped<ISaleApiClient, SaleApiClient>();
builder.Services.AddScoped<IOsobyService, OsobyService>();
builder.Services.AddSingleton<UserSessionService>();
builder.Services.AddScoped<ReservationRepo>();
builder.Services.AddScoped<ReservationService>();

// front
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rezerwacja sal API", Version = "v1" });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rezerwacja sal API v1");
        c.RoutePrefix = string.Empty; // ? opcjonalnie, ¿eby swagger by³ od razu na g³ównej
    });
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();
app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.MapDefaultEndpoints();

app.Run();
