using API;
using API.ApiService;
using API.ApiService.DB;
using API.ApiService.Filters;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace API.Web;

public interface ISaleApiClient
{
    Task<List<Sale>?> IGetSaleAsync();
    Task<List<Sale>?> GetFilteredSales([FromBody] SaleFilter filter);
}

public class SaleApiClient : ISaleApiClient
{
    private readonly MySqlDataSource _dataSource;

    public SaleApiClient(MySqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<List<Sale>?> IGetSaleAsync()
    {
        try
        {
            var repo = new SaleRepo(_dataSource);
            return await repo.GetSaleAsync();
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





