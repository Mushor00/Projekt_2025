using API;
using API.ApiService;
using API.ApiService.DB;
using MySqlConnector;

namespace API.Web;

public interface ISaleApiClient
{
    Task<Sale?> IGetSaleAsync();
}

public class SaleApiClient : ISaleApiClient
{
    private readonly MySqlDataSource _dataSource;

    public SaleApiClient(MySqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<Sale?> IGetSaleAsync()
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
}





