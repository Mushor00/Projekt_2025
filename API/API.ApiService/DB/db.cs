using System.Data.Common;
using MySqlConnector;
using API.ApiService.hashing;
using Microsoft.VisualBasic;
using System.Security.Principal;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using API.ApiService.Filters;
using Microsoft.AspNetCore.Mvc;


namespace API.ApiService.DB
{
    public class SaleRepo(MySqlDataSource database)
    {
        public async Task<List<Sale>?> GetSaleAsync()
        {
            try
            {
                using var connection = await database.OpenConnectionAsync();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM sale";
                var result = await ReadAllSaleAsync(await command.ExecuteReaderAsync());
                
                
                connection.Close();
                return result ?? new List<Sale>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }

        }
        
        public async Task<List<Sale>> GetFilteredSales([FromBody] SaleFilter filter)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            var query = new StringBuilder("SELECT * FROM sale WHERE 1=1");

            if (!string.IsNullOrWhiteSpace(filter.Numer))
            {
                query.Append(" AND Numer LIKE @Numer");
                command.Parameters.AddWithValue("@Numer", $"%{filter.Numer}%");
            }

            if (!string.IsNullOrWhiteSpace(filter.Budynek))
            {
                query.Append(" AND Budynek LIKE @Budynek");
                command.Parameters.AddWithValue("@Budynek", $"%{filter.Budynek}%");
            }

            if (!string.IsNullOrWhiteSpace(filter.Nazwa))
            {
                query.Append(" AND Nazwa LIKE @Nazwa");
                command.Parameters.AddWithValue("@Nazwa", $"%{filter.Nazwa}%");
            }

            if (filter.Pietro.HasValue)
            {
                query.Append(" AND Piętro = @Pietro");
                command.Parameters.AddWithValue("@Pietro", filter.Pietro.Value);
            }

            if (filter.Pojemnosc.HasValue)
            {
                query.Append(" AND Pojemność = @Pojemnosc");
                command.Parameters.AddWithValue("@Pojemnosc", filter.Pojemnosc.Value);
            }

            if (filter.Projektor == "HDMI")
            {
                query.Append(" AND ProjektorHDMI = 1");
            }
            else if (filter.Projektor == "VGA")
            {
                query.Append(" AND ProjektorVGA = 1");
            }

            if (filter.Tablica == "Multimedialna")
            {
                query.Append(" AND TablicaMultimedialna = 1");
            }
            else if (filter.Tablica == "Suchoscierna")
            {
                query.Append(" AND TablicaSuchoscieralna = 1");
            }

            if (filter.Klimatyzacja.HasValue)
            {
                query.Append(" AND Klimatyzacja = @Klimatyzacja");
                command.Parameters.AddWithValue("@Klimatyzacja", filter.Klimatyzacja.Value ? 1 : 0);
            }

            if (filter.Komputerowa.HasValue)
            {
                query.Append(" AND Komputerowa = @Komputerowa");
                command.Parameters.AddWithValue("@Komputerowa", filter.Komputerowa.Value ? 1 : 0);
            }

            if (!string.IsNullOrWhiteSpace(filter.Dostepnosc))
            {
                query.Append(" AND Status = @Status");
                command.Parameters.AddWithValue("@Status", filter.Dostepnosc);
            }

            command.CommandText = query.ToString();

            var sales = new List<Sale>();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                sales.Add(new Sale
                {
                    Numer = reader.GetInt32(reader.GetOrdinal("Numer")),
                    Budynek = reader.GetString(reader.GetOrdinal("Budynek")),
                    Nazwa = reader.GetString(reader.GetOrdinal("Nazwa")),
                    Pietro = reader.GetInt32(reader.GetOrdinal("Piętro")),
                    Pojemnosc = reader.GetInt32(reader.GetOrdinal("Pojemność")),
                    ProjektorVGA = reader.GetInt32(reader.GetOrdinal("ProjektorVGA")),
                    ProjektorHDMI = reader.GetInt32(reader.GetOrdinal("ProjektorHDMI")),
                    TablicaMultimedialna = reader.GetInt32(reader.GetOrdinal("TablicaMultimedialna")),
                    TablicaSuchoscieralna = reader.GetInt32(reader.GetOrdinal("TablicaSuchoscieralna")),
                    Klimatyzacja = reader.GetInt32(reader.GetOrdinal("Klimatyzacja")),
                    Komputerowa = reader.GetInt32(reader.GetOrdinal("Komputerowa")),
                });
            }

            return sales;
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

        public async Task<(bool Success, string Message)> LoginAsync(string login, string haslo, MySqlDataSource database)
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
                return (true, "Zalogowano pomyślnie");
            }
            else
            {
                Console.WriteLine("Hasło jest niepoprawne");
                return (false, "Nieprawidłowy login lub hasło");
            }
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

            // Sprawdzenie, czy email już istnieje
            using (var checkCommand = connection.CreateCommand())
            {
                checkCommand.CommandText = "SELECT 1 FROM osoby WHERE email = @email";
                checkCommand.Parameters.AddWithValue("@email", email);

                using var reader = await checkCommand.ExecuteReaderAsync();
                if (reader.HasRows)
                {
                    Console.WriteLine("Użytkownik z tym e-mailem już istnieje.");
                    return null;
                }
            }

            using (var insertCommand = connection.CreateCommand())
            {
                insertCommand.CommandText = @"INSERT INTO osoby (login, password, email, imie, nazwisko, nralbumu) VALUES (@login, @haslo, @email, @imie, @nazwisko, @numerAlbumu)";

                insertCommand.Parameters.AddWithValue("@login", login);
                insertCommand.Parameters.AddWithValue("@haslo", haslo);
                insertCommand.Parameters.AddWithValue("@email", email);
                insertCommand.Parameters.AddWithValue("@imie", imie);
                insertCommand.Parameters.AddWithValue("@nazwisko", nazwisko);
                insertCommand.Parameters.AddWithValue("@numerAlbumu", numerAlbumu);

                await insertCommand.ExecuteNonQueryAsync();
            }

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
