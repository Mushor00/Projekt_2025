using API.Web;
using API.Web.Components;
using MySqlConnector;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();


// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
