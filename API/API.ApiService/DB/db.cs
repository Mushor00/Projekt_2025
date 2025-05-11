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

            if (filter.Projektor == "VGA")
            {
                query.Append(" AND Projektor = 1");
            }
            else if (filter.Projektor == "HDMI")
            {
                query.Append(" AND Projektor = 2");
            }
            else if (filter.Projektor == "HDMI/VGA")
            {
                query.Append(" AND Projektor = 3");
            }
            else if (filter.Projektor == "USB")
            {
                query.Append(" AND Projektor = 4");
            }
            else if (filter.Projektor == "USB/HDMI")
            {
                query.Append(" AND Projektor = 5");
            }
            else if (filter.Projektor == "USB/VGA")
            {
                query.Append(" AND Projektor = 6");
            }
            else if (filter.Projektor == "USB/HDMI/VGA")
            {
                query.Append(" AND Projektor = 7");
            }


            if (filter.Tablica == "Multimedialna")
            {
                query.Append(" AND Tablica = 1");
            }
            else if (filter.Tablica == "Suchoscierna")
            {
                query.Append(" AND Tablica = 2");
            }
            else if (filter.Tablica == "Multimedialna/Suchościerna")
            {
                query.Append(" AND Tablica = 3");
            }
            else if (filter.Tablica == "Interaktywna")
            {
                query.Append(" AND Tablica = 4");
            }
            else if (filter.Tablica == "Interaktywna/Multimedialna")
            {
                query.Append(" AND Tablica = 5");
            }
            else if (filter.Tablica == "Interaktywna/Suchościerna")
            {
                query.Append(" AND Tablica = 6");
            }
            else if (filter.Tablica == "Interaktywna/Multimedialna/Suchościerna")
            {
                query.Append(" AND Tablica = 7");
            }

            if (filter.Ulica == "Kilińskiego")
            {
                query.Append(" AND Ulica = Kilińskiego");
            }

            if (filter.Niepelnosprawni.HasValue)
            {
                query.Append(" AND Dla_niepelnosprawnych = @Niepelnosprawni");
                command.Parameters.AddWithValue("@Niepelnosprawni", filter.Niepelnosprawni.Value);
            }


            if (filter.Klimatyzacja.HasValue)
            {
                query.Append(" AND Klimatyzacja = @Klimatyzacja");
                command.Parameters.AddWithValue("@Klimatyzacja", filter.Klimatyzacja.Value);
            }

            if (filter.Komputerowa.HasValue)
            {
                query.Append(" AND Komputerowa = @Komputerowa");
                command.Parameters.AddWithValue("@Komputerowa", filter.Komputerowa.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Dostepnosc))
            {
                query.Append(" AND Status = @Status");
                command.Parameters.AddWithValue("@Status", filter.Dostepnosc);
            }



            //TODO: Add filtering for Ulica and Niepelnosprawni, zmiana w bazie danych, zmiana projektora i tablicy


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
                    Projektor = reader.GetInt32(reader.GetOrdinal("Projektor")),
                    Tablica = reader.GetInt32(reader.GetOrdinal("Tablica")),
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
                        Projektor = reader.GetInt32("Projektor"),
                        Tablica = reader.GetInt32("Tablica"),
                        Klimatyzacja = reader.GetInt32("Klimatyzacja"),
                        Komputerowa = reader.GetInt32("Komputerowa"),
                        Ulica = reader.GetString("Ulica"),
                        Niepelnosprawni = reader.GetInt32("Dla_niepelnosprawnych")
                    };
                    result.Add(sale);
                }
            }
            return result;
        }

        public async Task<(bool Success, string Message, Osoby osoba)> LoginAsync(string login, string haslo, MySqlDataSource database)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT imie, password FROM osoby WHERE email = @login";
            command.Parameters.AddWithValue("@login", login);
            var hashedPassword = Hashing.HashPassword(haslo);

            var result = await command.ExecuteReaderAsync();

            if (await result.ReadAsync())
            {
                var storedHashedPassword = result.GetString(result.GetOrdinal("password"));
                if (storedHashedPassword == hashedPassword)
                {
                    var imie = result.GetString(result.GetOrdinal("imie"));

                    var osoba = new Osoby
                    {
                        Imie = imie
                    };

                    result.Close();
                    return (true, "Zalogowano pomyślnie", osoba);
                }
            }

            result.Close();
            return (false, "Nieprawidłowy login lub hasło", null);
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


        public async Task<(bool Success, string Message)> AddRezerwacjaAsync(int email, string nazwa, DateOnly data, TimeOnly godzinaRozpoczecia, TimeOnly godzinaZakonczenia, MySqlDataSource database)
        {
            // Sprawdzenie, czy sala jest dostępna w danym terminie
            using var connection = await database.OpenConnectionAsync();
            using var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = @"SELECT COUNT(*) FROM rezerwacje WHERE ID_sale = @idSali AND Data = @data AND ((Godzina_Rozpoczecia = @godzinaRozpoczecia AND Godzina_Zakonczenia = @godzinaZakonczenia))";
            checkCommand.Parameters.AddWithValue("@idSali", nazwa);
            checkCommand.Parameters.AddWithValue("@data", data);
            checkCommand.Parameters.AddWithValue("@godzinaRozpoczecia", godzinaRozpoczecia);
            checkCommand.Parameters.AddWithValue("@godzinaZakonczenia", godzinaZakonczenia);
            var count = (long)await checkCommand.ExecuteScalarAsync();
            if (count > 0)
            {
                Console.WriteLine("Sala jest już zajęta w tym terminie.");
                return (false, "Sala jest już zajęta w tym terminie.");
            }




            int idOsoby;
            int idSali;
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT ID FROM osoby WHERE email = @email";
            command.Parameters.AddWithValue("@email", email);
            var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                idOsoby = reader.GetInt32(reader.GetOrdinal("ID"));
            }
            else
            {
                Console.WriteLine("Nie znaleziono użytkownika.");
                return (false, "Nie znaleziono użytkownika.");
            }
            reader.Close();


            command.CommandText = "SELECT ID FROM sale WHERE nazwa = @nazwa";
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@numer", nazwa);
            reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                idSali = reader.GetInt32(reader.GetOrdinal("ID"));
            }
            else
            {
                Console.WriteLine("Nie znaleziono sali.");
                return (false, "Nie znaleziono sali.");
            }
            reader.Close();


            command.CommandText = @"INSERT INTO rezerwacje (ID_osoby, ID_sale, Data, Godzina_Rozpoczecia, Godzina_Zakonczenia) VALUES (@idOsoby, @idSali, @data, @godzinaRozpoczecia, @godzinaZakonczenia)";
            command.Parameters.AddWithValue("@idOsoby", idOsoby);
            command.Parameters.AddWithValue("@idSali", idSali);
            command.Parameters.AddWithValue("@data", data);
            command.Parameters.AddWithValue("@godzinaRozpoczecia", godzinaRozpoczecia);
            command.Parameters.AddWithValue("@godzinaZakonczenia", godzinaZakonczenia);

            await command.ExecuteNonQueryAsync();

            Console.WriteLine("Rezerwacja została dodana.");
            return (true, "Rezerwacja została dodana.");
        }

        public async Task<(bool Success, string Message)> UpdateRezerwacjaAsync(Rezerwacje rezerwacja)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"UPDATE rezerwacje SET ID_osoby = @idOsoby, ID_sale = @idSali, Data = @data, Godzina_Rozpoczecia = @godzinaRozpoczecia, Godzina_Zakonczenia = @godzinaZakonczenia WHERE ID = @id";
            command.Parameters.AddWithValue("@id", rezerwacja.Id);
            command.Parameters.AddWithValue("@idOsoby", rezerwacja.ID_osoby);
            command.Parameters.AddWithValue("@idSali", rezerwacja.ID_sala);
            command.Parameters.AddWithValue("@data", rezerwacja.Data);
            command.Parameters.AddWithValue("@godzinaRozpoczecia", rezerwacja.GodzinaRozpoczecia);
            command.Parameters.AddWithValue("@godzinaZakonczenia", rezerwacja.GodzinaZakonczenia);

            await command.ExecuteNonQueryAsync();

            return (true, "Rezerwacja została zaktualizowana.");
        }

        public async Task<(bool Success, string Message)> DeleteRezerwacjaAsync(int id)
        {
            using var connection = await database.OpenConnectionAsync();
            using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM rezerwacje WHERE ID = @id";
            command.Parameters.AddWithValue("@id", id);

            await command.ExecuteNonQueryAsync();

            return (true, "Rezerwacja została usunięta.");
        }

    }
}
