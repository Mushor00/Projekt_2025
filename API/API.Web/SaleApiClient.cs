
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
    Task<List<Sale>?> GetFilteredSales([FromBody] SaleFilter filter); // bez zmian
}

public class SaleApiClient : ISaleApiClient
{
    private readonly MySqlDataSource _dataSource;

    public SaleApiClient(MySqlDataSource dataSource)
    {
        _dataSource = dataSource;
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

    // NOWY FRONTEND – zwraca zmapowane obiekty typu Sala
    public async Task<List<Sala>?> GetMappedSaleAsync()
    {
        try
        {
            var repo = new SaleRepo(_dataSource);
            var saleZBazy = await repo.GetSaleAsync();

            return saleZBazy?.Select(s => new Sala
            {
                Id = 0, // nie używane w jebanej bazie, więc ustawiamy na 0
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
