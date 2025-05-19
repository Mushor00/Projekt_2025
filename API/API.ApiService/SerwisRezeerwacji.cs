using API.ApiService.DB;

namespace API.ApiService
{
    public class ReservationService
    {
        private readonly ReservationRepo _repo;

        public ReservationService(ReservationRepo repo)
        {
            _repo = repo;
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
                Console.WriteLine($"Błąd podczas usuwania rezerwacji: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> EdytujRezerwacjeAsync(int idRezerwacji, DateTime nowaDataOd, DateTime nowaDataDo)
        {
            return await _repo.EdytujRezerwacjeAsync(idRezerwacji, nowaDataOd, nowaDataDo);
        }


    }
}
