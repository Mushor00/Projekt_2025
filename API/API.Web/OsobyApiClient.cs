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

    public async Task<(bool Success, string Message)> RegisterAsync(RegisterTestPage.RegisterModel model)
    {
        var exists = _osobyList.Any(o => o.Email == model.Email);
        if (exists)
            return (false, "Użytkownik z tym e-mailem już istnieje.");

        var hashedPassword = Hashing.HashPassword(model.Password);
        
        var osoba = new Osoby
        {
            Imie = model.FirstName,
            Nazwisko = model.LastName,
            NumerAlbumu = model.IndexNumber,
            Email = model.Email,
            Haslo = hashedPassword
        };
        
        _osobyList.Add(osoba);
        
        await Task.CompletedTask;

        return (true, "Rejestracja zakończona sukcesem.");
    }
}





