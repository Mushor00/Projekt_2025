using System.ComponentModel.DataAnnotations;
using API;
using API.ApiService;
using API.ApiService.DB;
using API.ApiService.hashing;
using API.Web.Components.Pages;
using Grpc.Net.Client.Configuration;
using MySqlConnector;

namespace API.Web;

public interface IOsobyService
{
    Task<(bool Success, string Message)> RegisterAsync(Register.RegisterModel model);
    Task<(bool Success, string Message)> LoginAsync(string email, string password);
}

public class OsobyService : IOsobyService
{
    private static List<Osoby> _osobyList = new List<Osoby>();
    private readonly MySqlDataSource _dataSource;

    public OsobyService(MySqlDataSource dataSource)
    {
        _dataSource = dataSource ??
                      throw new ArgumentNullException(nameof(dataSource), "Źródło danych nie może być null");
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
        var result = repo.LoginAsync(login, haslo, _dataSource);
        await result;

        return result.Result.Success ? (true, "Zalogowano pomyślnie") : (false, "Nieprawidłowy login lub hasło");
    }


}





