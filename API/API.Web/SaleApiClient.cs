
using API;
using API.ApiService;
using API.ApiService.DB;
using API.ApiService.Filters;
using API.ApiService.Models; // dla klasy Sala
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace API.Web;

public interface ISaleApiClient
{
    Task<List<Sale>?> IGetSaleAsync(); // STARY BACKEND – oryginalne Sale z DB
    Task<List<Sala>?> GetMappedSaleAsync(); // NOWY FRONTEND – zmapowane na Sala
    Task<bool> UpdateSalaAsync(Sala sala);
    Task<List<Sale>?> GetFilteredSales([FromBody] SaleFilter filter); // bez zmian
}

public class SaleApiClient : ISaleApiClient
{
    private readonly MySqlDataSource _dataSource;

    private readonly HttpClient _http;

    public SaleApiClient(MySqlDataSource dataSource, HttpClient http)
    {
        _dataSource = dataSource;
        _http = http;
    }

    // STARY BACKEND – zwraca oryginalne encje z bazy danych
    public async Task<List<Sale>?> IGetSaleAsync()
    {
        try
        {
            var repo = new SaleRepo(_dataSource);
            return await repo.GetSaleAsync(); // zwraca List<Sale>
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd połączenia z bazą: " + ex.Message);
            return null;
        }
    }

    //NOWY FRONTEND – zwraca zmapowane obiekty typu Sala
    public async Task<List<Sala>?> GetMappedSaleAsync()
    {
        try
        {
            var repo = new SaleRepo(_dataSource);
            var saleZBazy = await repo.GetSaleAsync();

            return saleZBazy?.Select(s => new Sala
            {
                Id = s.Id, // Naprawione - używamy prawdziwego ID z bazy
                Numer = s.Numer,
                Budynek = s.Budynek ?? "",
                Nazwa = s.Nazwa ?? "",
                Pietro = s.Pietro,
                Pojemnosc = s.Pojemnosc,
                Dostepna = (s.Dostepnosc?.ToLower() ?? "nie") == "tak"
            }).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd połączenia z bazą: " + ex.Message);
            return null;
        }
    }

    // public async Task<List<Sala>> GetMappedSaleAsync()
    // {
    //     try
    //     {
    //         var response = await _http.GetFromJsonAsync<List<Sala>>("/api/sale");
    //         return response ?? new List<Sala>();
    //     }
    //     catch (Exception)
    //     {
    //         return new List<Sala>();
    //     }
    // }

    public async Task<bool> UpdateSalaAsync(Sala sala)
    {
        try
        {
            var response = await _http.PutAsJsonAsync($"/api/sale/{sala.Id}", sala);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<List<Sale>?> GetFilteredSales([FromBody] SaleFilter filter)
    {
        try
        {
            var repo = new SaleRepo(_dataSource);
            return await repo.GetFilteredSales(filter);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd połączenia z bazą: " + ex.Message);
            return null;
        }
    }
}
