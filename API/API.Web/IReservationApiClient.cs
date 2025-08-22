using API.ApiService.DB;
using API.ApiService.Models;
using API.Web;

public interface IReservationApiClient
{
    Task<(bool Success, string Message)> ZarezerwujSaleAsync(RezerwacjaRequest request);
    Task<List<Rezerwacja>> GetRezerwacjeBySalaAndMonth(int salaId, DateOnly miesiac);
    Task<List<RezerwacjaDto>?> GetRezerwacjeBySalaAndDateRange(int idSali, DateOnly dataOd, DateOnly dataDo);
    Task<List<RezerwacjaDto>?> GetAllRezerwacjeByDate(DateOnly data);
    Task<List<RezerwacjaKalendarzDto>?> GetRezerwacjeKalendarzByDate(DateOnly data);
    Task<List<RezerwacjaKalendarzDto>?> GetAllRezerwacje();
    Task<RezerwacjaKalendarzDto?> GetRezerwacjaById(int id);
    Task<(bool Success, string Message)> UpdateRezerwacjaAsync(int id, RezerwacjaRequest request);
    Task<bool> DeleteRezerwacjaAsync(int id);
    Task<List<Sala>?> GetSaleAsync();
}