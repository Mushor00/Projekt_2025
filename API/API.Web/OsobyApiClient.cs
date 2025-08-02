using System.ComponentModel.DataAnnotations;
using API;
using API.ApiService;
using API.ApiService.DB;
using API.ApiService.hashing;
using API.Web.Components.Pages;
using Microsoft.JSInterop;
using MySqlConnector;

namespace API.Web;

public interface IOsobyService
{
    Task<(bool Success, string Message)> RegisterAsync(Register.RegisterModel model);
    Task<(bool Success, string Message)> LoginAsync(string email, string password);
}

public class OsobyService : IOsobyService
{
    private readonly MySqlDataSource _dataSource;
    private readonly UserSessionService _session;
    private readonly IJSRuntime _jsRuntime;

    public OsobyService(MySqlDataSource dataSource, UserSessionService session, IJSRuntime jsRuntime)
    {
        _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        _session = session ?? throw new ArgumentNullException(nameof(session));
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
    }

    public async Task<(bool Success, string Message)> RegisterAsync(Register.RegisterModel model)
    {
        var hashedPassword = Hashing.HashPassword(model.Password);

        var repo = new SaleRepo(_dataSource);
        var result = await repo.RegisterAsync(model.Username, hashedPassword, model.Email, model.FirstName, model.LastName, model.IndexNumber, _dataSource);

        if (result == null)
        {
            return (false, "Wystąpił błąd podczas rejestracji użytkownika.");
        }

        await Task.CompletedTask;

        return (true, "Rejestracja zakończona sukcesem.");
    }

    public async Task<(bool Success, string Message)> LoginAsync(string login, string haslo)
    {
        var repo = new SaleRepo(_dataSource);
        var result = await repo.LoginAsync(login, haslo, _dataSource);

        if (result.Success)
        {
            if (result.osoba is not null)
            {
                _session.SetUser(result.osoba.Imie);
                await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "username", login);
                await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "email", login);
            }
            else
            {
                return (false, "Błąd logowania.");
            }
        }

        return result.Success
            ? (true, "Zalogowano pomyślnie")
            : (false, "Nieprawidłowy login lub hasło");
    }
}






