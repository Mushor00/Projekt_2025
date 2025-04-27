using MySqlConnector;
using API.ApiService.hashing;
using Microsoft.VisualBasic;
using System.Security.Principal;


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
                        Dostepnosc = reader.GetString("dostępność"),
                        ProjektorHDMI = reader.GetInt32("ProjektorHDMI"),
                        ProjektorVGA = reader.GetInt32("ProjektorVGA"),
                        TablicaMultimedialna = reader.GetInt32("TablicaMultimedialna"),
                        TablicaSuchoscieralna = reader.GetInt32("TablicaSuchoscieralna"),
                        Klimatyzacja = reader.GetInt32("Klimatyzacja"),
                        Komputerowa = reader.GetInt32("Komputerowa")
                    };
                    result.Add(sale);
                }
            }
            return result;
        }

        public async Task<Osoby> LoginAsync(string login, string haslo, MySqlDataSource database)
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


        public async Task<Osoby> RegisterAsync(string login, string haslo, string email, string imie, string nazwisko, string numerAlbumu, MySqlDataSource database)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM osoby WHERE email = @email";
            if (await command.ExecuteReaderAsync() != null)
            {
                Console.WriteLine("Użytkownik z tym e-mailem już istnieje.");
                return null;
            }
            command.CommandText = "INSERT INTO osoby (ID, login, password, email, imie, nazwisko, NrAlbumu) VALUES (, @login, @haslo, @email, @imie, @nazwisko, @numerAlbumu)";
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@haslo", haslo);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@imie", imie);
            command.Parameters.AddWithValue("@nazwisko", nazwisko);
            command.Parameters.AddWithValue("@numerAlbumu", numerAlbumu);
            Console.WriteLine(command.CommandText);
            await command.ExecuteNonQueryAsync();
            connection.Close();
            return new Osoby
            {
                Login = login,
                Haslo = haslo,
                Email = email,
                Imie = imie,
                Nazwisko = nazwisko,
                NumerAlbumu = numerAlbumu
            };
        }
    }
}
