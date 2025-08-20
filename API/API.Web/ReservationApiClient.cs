using API.ApiService.DB;
using API.ApiService.Models;
using API.Web;
using System.Net.Http.Json;
using MySqlConnector;
using API.ApiService;

public class ReservationApiClient : IReservationApiClient
{

    private readonly MySqlDataSource _dataSource;

    public ReservationApiClient(MySqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<bool> ZarezerwujSaleAsync(RezerwacjaRequest request)
    {
        var repo = new ReservationRepo(_dataSource);
        var result = await repo.ZarezerwujSale(
            request.NumerSali,
            request.Imie,
            request.NazwaPrzedmiotu,
            request.Data,
            request.DataOd,
            request.DataDo
        );

        return result;
    }

    public async Task<List<Rezerwacja>> GetRezerwacjeBySalaAndMonth(int salaId, DateOnly miesiac)
    {
        return null;
    }
    public async Task<List<RezerwacjaDto>?> GetRezerwacjeBySalaAndDateRange(int numerSali, DateOnly dataOd, DateOnly dataDo)
    {


        return null;
    }
    public async Task<List<Sala>?> GetSaleAsync()
    {
        return null;
    }

}
