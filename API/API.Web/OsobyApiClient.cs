using System.ComponentModel.DataAnnotations;
using API;
using API.ApiService;
using API.ApiService.DB;
using API.ApiService.hashing;
using API.Web.Components.Pages;

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
        var result =  await repo.RegisterAsync(model.Username, hashedPassword, model.Email, model.FirstName, model.LastName, model.IndexNumber, _dataSource);

        if (result == null)
        {
            return (false, "Wystąpił błąd podczas rejestracji użytkownika.");
        }
        
        await Task.CompletedTask;

        return (true, "Rejestracja zakończona sukcesem.");
    }

    public async Task<(bool Success, string Message)> LoginAsync(string login, string haslo)
    {
        using var connection = await _dataSource.OpenConnectionAsync(); 
        using var command = connection.CreateCommand();

        command.CommandText = "SELECT * FROM osoby WHERE login = @login";
        command.Parameters.AddWithValue("@login", login);

        using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var storedHashedPassword = reader.GetString(reader.GetOrdinal("password"));
            var inputHashedPassword = Hashing.HashPassword(haslo);

            if (storedHashedPassword == inputHashedPassword)
            {
                Console.WriteLine("Hasło jest poprawne");
                return (true, "Zalogowano pomyślnie");
            }
        }

        Console.WriteLine("Hasło jest niepoprawne");
        return (false, "Nieprawidłowy login lub hasło");
    }


}





