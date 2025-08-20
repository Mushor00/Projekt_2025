using API.ApiService.DB;
using API.ApiService.Models;
using API.Web;

public interface IReservationApiClient
{
    Task<bool> ZarezerwujSaleAsync(RezerwacjaRequest request);
    Task<List<Rezerwacja>> GetRezerwacjeBySalaAndMonth(int salaId, DateOnly miesiac);
    Task<List<RezerwacjaDto>?> GetRezerwacjeBySalaAndDateRange(int numerSali, DateOnly dataOd, DateOnly dataDo);
    Task<List<Sala>?> GetSaleAsync();
}