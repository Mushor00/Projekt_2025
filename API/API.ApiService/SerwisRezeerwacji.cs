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

        public async Task<bool> ZarezerwujSaleAsync(int numerSali, string login, DateTime dataOd, DateTime dataDo)
        {
            return await _repo.ZarezerwujSale(numerSali, login, dataOd, dataDo);
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

        public async Task<bool> EdytujRezerwacjeAsync(int idRezerwacji, DateTime nowaDataOd, DateTime nowaDataDo)
        {
            try
            {
                return await _repo.EdytujRezerwacjeAsync(idRezerwacji, nowaDataOd, nowaDataDo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Błąd podczas edytowania rezerwacji.");
                return false;
            }
        }
    }
}
