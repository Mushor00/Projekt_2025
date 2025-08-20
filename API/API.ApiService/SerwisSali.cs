using API.ApiService.DB;
using API.ApiService.Models;

namespace API.ApiService
{
    public class SalaService
    {
        private readonly SalaRepo _repo;

        public SalaService(SalaRepo repo)
        {
            _repo = repo;
        }

        public Task<List<Sala>> PobierzSaleAsync() => _repo.PobierzSaleAsync();
        public Task<bool> ZapiszSaleAsync(Sala sala) => _repo.ZapiszSaleAsync(sala);
        public async Task<bool> UpdateSalaAsync(Sala sala)
        {
            return await _repo.UpdateSalaAsync(sala);
        }
    }

}
