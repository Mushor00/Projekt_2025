using System.ComponentModel.DataAnnotations;
using API;
using API.ApiService;
using API.ApiService.DB;
using API.ApiService.hashing;
using API.Web.Components.Pages;
using Grpc.Net.Client.Configuration;
using MySqlConnector;

namespace API.Web;
public interface IRezerwacjeService
{
    Task<(bool Success, string Message)> AddRezerwacjaAsync(Rezerwacje rezerwacja);
    Task<(bool Success, string Message)> UpdateRezerwacjaAsync(Rezerwacje rezerwacja);
    Task<(bool Success, string Message)> DeleteRezerwacjaAsync(int id);

}

/*
public class RezerwacjeService : IRezerwacjeService
{
    private readonly MySqlDataSource _dataSource;

    public RezerwacjeService(MySqlDataSource dataSource)
    {
        _dataSource = dataSource ??
                      throw new ArgumentNullException(nameof(dataSource), "Źródło danych nie może być null");
    }

    public async Task<(bool Success, string Message)> AddRezerwacjaAsync(Rezerwacje rezerwacja)
    {
        var repo = new SaleRepo(_dataSource);
        var result = await repo.AddRezerwacjaAsync(rezerwacja, _dataSource);

        if (result == null)
        {
            return (false, "Wystąpił błąd podczas dodawania rezerwacji.");
        }

        await Task.CompletedTask;

        return (true, "Rezerwacja dodana pomyślnie.");
    }
    public async Task<(bool Success, string Message)> UpdateRezerwacjaAsync(Rezerwacje rezerwacja)
    {
        var repo = new SaleRepo(_dataSource);
        var result = await repo.UpdateRezerwacjaAsync(rezerwacja, _dataSource);

        if (result == null)
        {
            return (false, "Wystąpił błąd podczas aktualizacji rezerwacji.");
        }

        await Task.CompletedTask;

        return (true, "Rezerwacja zaktualizowana pomyślnie.");
    }
    public async Task<(bool Success, string Message)> DeleteRezerwacjaAsync(int id)
    {
        var repo = new SaleRepo(_dataSource);
        var result = await repo.DeleteRezerwacjaAsync(id, _dataSource);

        if (result == null)
        {
            return (false, "Wystąpił błąd podczas usuwania rezerwacji.");
        }

        await Task.CompletedTask;

        return (true, "Rezerwacja usunięta pomyślnie.");
    }
    
}
*/