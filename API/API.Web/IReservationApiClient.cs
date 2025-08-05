using API.ApiService.DB;

namespace API.Web
{
    public interface IReservationApiClient
    {
        Task<bool> ZarezerwujSaleAsync(RezerwacjaRequest request);
        Task<List<Rezerwacja>> GetRezerwacjeBySalaAndMonth(int salaId, DateTime miesiac);
    }

}
