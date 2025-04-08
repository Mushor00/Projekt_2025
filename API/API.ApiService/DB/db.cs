using MySqlConnector;

namespace API.ApiService.DB
{
    public class SaleRepo(MySqlDataSource database)
    {
        public async Task<Sale?> GetSaleAsync()
        {
            try
            {
                using var connection = await database.OpenConnectionAsync();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM sale";
                var result = await ReadAllAsync(await command.ExecuteReaderAsync());
                connection.Close();
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }

        }

        private static async Task<List<Sale>> ReadAllAsync(MySqlDataReader reader)
        {
            var result = new List<Sale>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var sale = new Sale
                    {
                        Numer = reader.GetInt32("numer"),
                        Budynek = reader.GetString("budynek"),
                        Nazwa = reader.GetString("nazwa"),
                        Pietro = reader.GetInt32("piętro"),
                        Pojemosc = reader.GetInt32("pojemność"),
                        Dostepnosc = reader.GetString("dostępność")
                    };
                    result.Add(sale);
                }
            }
            return result;
        }

    }
}
