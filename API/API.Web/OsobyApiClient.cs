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
    Task<(bool Success, string Message)> RegisterAsync(RegisterTestPage.RegisterModel model);
}

public class OsobyService : IOsobyService
{
    private static List<Osoby> _osobyList = new List<Osoby>();
    private readonly MySqlDataSource _dataSource;

    public async Task<(bool Success, string Message)> RegisterAsync(RegisterTestPage.RegisterModel model)
    {
        var exists = _osobyList.Any(o => o.Email == model.Email);
        if (exists)
            return (false, "Użytkownik z tym e-mailem już istnieje.");

        var hashedPassword = Hashing.HashPassword(model.Password);

        var repo = new SaleRepo(_dataSource);
        await repo.RegisterAsync(model.Username, hashedPassword, model.Email, model.FirstName, model.LastName, model.IndexNumber, _dataSource);

        await Task.CompletedTask;

        return (true, "Rejestracja zakończona sukcesem.");
    }


}





