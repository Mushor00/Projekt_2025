using API.Web;
using API.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Aspire defaults
builder.AddServiceDefaults();

// Razor components i Blazor:
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // to ju¿ masz

// Antiforgery
builder.Services.AddAntiforgery(); // dodaj tê liniê jeœli jeszcze nie ma

// HTTP klienty, np.:
builder.Services.AddHttpClient<ReservationApiClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5001");
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // jeœli nie masz, dodaj to

app.UseRouting();


app.UseAntiforgery(); // ? KLUCZOWE!

app.MapRazorComponents<App>() // ? wa¿ne: App z przestrzeni namespace twojego projektu
   .AddInteractiveServerRenderMode();

app.Run();
