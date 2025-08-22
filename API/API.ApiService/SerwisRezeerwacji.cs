using API.ApiService.DB;

using Microsoft.Extensions.Logging;

namespace API.ApiService
{
    public class ReservationService
    {
        private readonly ReservationRepo _repo;
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(ReservationRepo repo, ILogger<ReservationService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> ZarezerwujSaleAsync(int idSali, string imie, string nazwisko, string nazwaPrzedmiotu, DateOnly data, TimeOnly godzinaOd, TimeOnly godzinaDo)
        {
            return await _repo.ZarezerwujSale(idSali, imie, nazwisko, nazwaPrzedmiotu, data, godzinaOd, godzinaDo);
        }

        public async Task<bool> UsunRezerwacjeAsync(int idRezerwacji)
        {
            try
            {
                return await _repo.UsunRezerwacjeAsync(idRezerwacji);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas usuwania rezerwacji.");
                return false;
            }
        }

        public async Task<(bool Success, string Message)> EdytujRezerwacjeAsync(int idRezerwacji, DateOnly data, TimeOnly nowaGodzinaOd, TimeOnly nowaGodzinaDo)
        {
            try
            {
                return await _repo.EdytujRezerwacjeAsync(idRezerwacji, data, nowaGodzinaOd, nowaGodzinaDo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas edytowania rezerwacji.");
                return (false, "Wystąpił błąd podczas edytowania rezerwacji");
            }
        }
    }
}
