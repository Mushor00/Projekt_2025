using MySqlConnector;
using API.ApiService.hashing;
using Microsoft.VisualBasic;


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
                var result = await ReadAllSaleAsync(await command.ExecuteReaderAsync());
                connection.Close();
                return result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }

        }

        private static async Task<List<Sale>> ReadAllSaleAsync(MySqlDataReader reader)
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
                        Pojemnosc = reader.GetInt32("pojemność"),
                        Dostepnosc = reader.GetString("dostępność")
                    };
                    result.Add(sale);
                }
            }
            return result;
        }

        public static async Task<Osoby> Login(string login, string haslo, MySqlDataSource database)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT password FROM osoby WHERE login = @login";
            command.Parameters.AddWithValue("@login", login);
            var hashedPassword = Hashing.HashPassword(haslo);
            //command.Parameters.AddWithValue("@haslo", haslo);
            var result = await TryLoginOsobyAsync(await command.ExecuteReaderAsync());
            if (result.ToString() == hashedPassword)
            {
                Console.WriteLine("Hasło jest poprawne");
            }
            else
            {
                Console.WriteLine("Hasło jest niepoprawne");
            }
            connection.Close();
            return result;

        }
        private static async Task<Osoby> TryLoginOsobyAsync(MySqlDataReader reader)
        {
            var result = new Osoby();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var osoby = new Osoby
                    {
                        Haslo = reader.GetString("Password")
                    };
                    osoby.ToString();
                    result = osoby;
                }
            }
            return result;
        }
    }
}
