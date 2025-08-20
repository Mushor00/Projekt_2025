using API.ApiService.DB;
using API.ApiService.Models;
using API.Web;
using System.Net.Http.Json;

public class ReservationApiClient : IReservationApiClient
{
    private readonly HttpClient _http;

    public ReservationApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> ZarezerwujSaleAsync(RezerwacjaRequest request)
    {
        var response = await _http.PostAsJsonAsync("/api/rezerwacje/rezerwuj", request);
        return response.IsSuccessStatusCode;
    }

    public async Task<List<Rezerwacja>> GetRezerwacjeBySalaAndMonth(int salaId, DateOnly miesiac)
    {
        string miesiacParam = miesiac.ToString("yyyy-MM");
        var response = await _http.GetFromJsonAsync<List<Rezerwacja>>($"/api/rezerwacje/sala/{salaId}?miesiac={miesiacParam}");

        return response ?? new();
    }
    public async Task<List<RezerwacjaDto>?> GetRezerwacjeBySalaAndDateRange(int numerSali, DateOnly dataOd, DateOnly dataDo)
    {
        var response = await _http.GetAsync($"/api/rezerwacje/sala/{numerSali}?dataOd={dataOd:O}&dataDo={dataDo:O}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<RezerwacjaDto>>();
        }

        return null;
    }
    public async Task<List<Sala>?> GetSaleAsync()
    {
        var response = await _http.GetFromJsonAsync<List<Sala>>("/api/sale");
        return response;
    }

}
