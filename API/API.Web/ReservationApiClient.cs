using API.ApiService.DB;
using API.ApiService.Models;
using API.Web;
using System.Net.Http.Json;
using MySqlConnector;
using API.ApiService;

public class ReservationApiClient : IReservationApiClient
{

    private readonly MySqlDataSource _dataSource;

    public ReservationApiClient(MySqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<(bool Success, string Message)> ZarezerwujSaleAsync(RezerwacjaRequest request)
    {
        var repo = new ReservationRepo(_dataSource);
        var result = await repo.ZarezerwujSale(
            request.IdSali,
            request.Imie,
            request.Nazwisko,
            request.NazwaPrzedmiotu,
            request.Data,
            request.DataOd,
            request.DataDo
        );

        return result;
    }

    public async Task<List<Rezerwacja>> GetRezerwacjeBySalaAndMonth(int salaId, DateOnly miesiac)
    {
        // Ta metoda nie jest używana, zwracamy pustą listę
        return new List<Rezerwacja>();
    }
    public async Task<List<RezerwacjaDto>?> GetRezerwacjeBySalaAndDateRange(int idSali, DateOnly dataOd, DateOnly dataDo)
    {
        try
        {
            using var conn = await _dataSource.OpenConnectionAsync();
            using var cmd = conn.CreateCommand();

            // Używamy bezpośrednio ID sali
            cmd.CommandText = @"SELECT sd.ID, sd.ID_sale as IdSali, sd.ID_osoby as IdOsoby, 
                                       sd.Data, sd.Godzina_rozpoczecia as GodzinaRozpoczecia, 
                                       sd.Godzina_zakonczenia as GodzinaZakonczenia 
                                FROM sale_dostepnosc sd 
                                WHERE sd.ID_sale = @idSali AND sd.Data BETWEEN @dataOd AND @dataDo";
            cmd.Parameters.AddWithValue("@idSali", idSali);
            cmd.Parameters.AddWithValue("@dataOd", dataOd);
            cmd.Parameters.AddWithValue("@dataDo", dataDo);

            var rezerwacje = new List<RezerwacjaDto>();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                rezerwacje.Add(new RezerwacjaDto
                {
                    Id = reader.GetInt32("ID"),
                    IdSali = reader.GetInt32("IdSali"),
                    IdOsoby = reader.GetInt32("IdOsoby"),
                    Data = DateOnly.FromDateTime(reader.GetDateTime("Data")),
                    GodzinaRozpoczecia = TimeOnly.FromTimeSpan(reader.GetTimeSpan("GodzinaRozpoczecia")),
                    GodzinaZakonczenia = TimeOnly.FromTimeSpan(reader.GetTimeSpan("GodzinaZakonczenia"))
                });
            }

            return rezerwacje;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd pobierania rezerwacji: {ex.Message}");
            return new List<RezerwacjaDto>();
        }
    }
    public async Task<List<Sala>?> GetSaleAsync()
    {
        try
        {
            using var conn = await _dataSource.OpenConnectionAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM sale";

            var sale = new List<Sala>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                sale.Add(new Sala
                {
                    Id = reader.GetInt32("ID"),
                    Numer = reader.GetInt32("Numer"),
                    Budynek = reader.GetString("Budynek"),
                    Nazwa = reader.GetString("Nazwa"),
                    Pietro = reader.GetInt32("Piętro"),
                    Pojemnosc = reader.GetInt32("Pojemność"),
                    Dostepna = reader.GetString("Dostępność") == "Dostępna"
                });
            }
            return sale;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd pobierania sal: {ex.Message}");
            return new List<Sala>();
        }
    }

    public async Task<List<RezerwacjaDto>?> GetAllRezerwacjeByDate(DateOnly data)
    {
        try
        {
            using var conn = await _dataSource.OpenConnectionAsync();
            using var cmd = conn.CreateCommand();

            // Pobierz wszystkie rezerwacje na dany dzień z informacjami o salach i osobach
            cmd.CommandText = @"
                SELECT sd.ID, sd.ID_sale, sd.ID_osoby, sd.Data, 
                       sd.Godzina_rozpoczecia, sd.Godzina_zakonczenia,
                       sd.Nazwa_przedmiotu, s.Nazwa as NazwaSali, s.Budynek,
                       o.Imie, o.Nazwisko
                FROM sale_dostepnosc sd 
                JOIN sale s ON sd.ID_sale = s.ID
                JOIN osoby o ON sd.ID_osoby = o.ID
                WHERE sd.Data = @data
                ORDER BY s.Nazwa, sd.Godzina_rozpoczecia";

            cmd.Parameters.AddWithValue("@data", data);

            var rezerwacje = new List<RezerwacjaDto>();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var rezerwacja = new RezerwacjaDto
                {
                    Id = reader.GetInt32("ID"),
                    IdSali = reader.GetInt32("ID_sale"),
                    IdOsoby = reader.GetInt32("ID_osoby"),
                    Data = DateOnly.FromDateTime(reader.GetDateTime("Data")),
                    GodzinaRozpoczecia = TimeOnly.FromTimeSpan(reader.GetTimeSpan("Godzina_rozpoczecia")),
                    GodzinaZakonczenia = TimeOnly.FromTimeSpan(reader.GetTimeSpan("Godzina_zakonczenia"))
                };

                rezerwacje.Add(rezerwacja);
            }

            return rezerwacje;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd pobierania rezerwacji: {ex.Message}");
            return new List<RezerwacjaDto>();
        }
    }

    public async Task<List<RezerwacjaKalendarzDto>?> GetRezerwacjeKalendarzByDate(DateOnly data)
    {
        try
        {
            using var conn = await _dataSource.OpenConnectionAsync();
            using var cmd = conn.CreateCommand();

            // Pobierz wszystkie rezerwacje na dany dzień z dodatkowymi informacjami dla kalendarza
            cmd.CommandText = @"
                SELECT sd.ID, sd.ID_sale, sd.ID_osoby, sd.Data, 
                       sd.Godzina_rozpoczecia, sd.Godzina_zakonczenia,
                       sd.Nazwa_przedmiotu, s.Nazwa as NazwaSali, s.Budynek,
                       o.Imie, o.Nazwisko
                FROM sale_dostepnosc sd 
                JOIN sale s ON sd.ID_sale = s.ID
                JOIN osoby o ON sd.ID_osoby = o.ID
                WHERE sd.Data = @data
                ORDER BY s.Nazwa, sd.Godzina_rozpoczecia";

            cmd.Parameters.AddWithValue("@data", data);

            var rezerwacje = new List<RezerwacjaKalendarzDto>();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var rezerwacja = new RezerwacjaKalendarzDto
                {
                    Id = reader.GetInt32("ID"),
                    IdSali = reader.GetInt32("ID_sale"),
                    IdOsoby = reader.GetInt32("ID_osoby"),
                    Data = DateOnly.FromDateTime(reader.GetDateTime("Data")),
                    GodzinaRozpoczecia = TimeOnly.FromTimeSpan(reader.GetTimeSpan("Godzina_rozpoczecia")),
                    GodzinaZakonczenia = TimeOnly.FromTimeSpan(reader.GetTimeSpan("Godzina_zakonczenia")),
                    NazwaPrzedmiotu = reader.GetString("Nazwa_przedmiotu"),
                    NazwaSali = reader.GetString("NazwaSali"),
                    Budynek = reader.GetString("Budynek"),
                    ImieOsoby = reader.GetString("Imie"),
                    NazwiskoOsoby = reader.GetString("Nazwisko")
                };

                rezerwacje.Add(rezerwacja);
            }

            return rezerwacje;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd pobierania rezerwacji kalendarza: {ex.Message}");
            return new List<RezerwacjaKalendarzDto>();
        }
    }

    public async Task<List<RezerwacjaKalendarzDto>?> GetAllRezerwacje()
    {
        try
        {
            using var conn = await _dataSource.OpenConnectionAsync();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
                SELECT sd.ID, sd.ID_sale, sd.ID_osoby, sd.Data, 
                       sd.Godzina_rozpoczecia, sd.Godzina_zakonczenia,
                       sd.Nazwa_przedmiotu, s.Nazwa as NazwaSali, s.Budynek,
                       o.Imie, o.Nazwisko
                FROM sale_dostepnosc sd 
                JOIN sale s ON sd.ID_sale = s.ID
                JOIN osoby o ON sd.ID_osoby = o.ID
                ORDER BY sd.Data DESC, s.Nazwa, sd.Godzina_rozpoczecia";

            var rezerwacje = new List<RezerwacjaKalendarzDto>();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var rezerwacja = new RezerwacjaKalendarzDto
                {
                    Id = reader.GetInt32("ID"),
                    IdSali = reader.GetInt32("ID_sale"),
                    IdOsoby = reader.GetInt32("ID_osoby"),
                    Data = DateOnly.FromDateTime(reader.GetDateTime("Data")),
                    GodzinaRozpoczecia = TimeOnly.FromTimeSpan(reader.GetTimeSpan("Godzina_rozpoczecia")),
                    GodzinaZakonczenia = TimeOnly.FromTimeSpan(reader.GetTimeSpan("Godzina_zakonczenia")),
                    NazwaPrzedmiotu = reader.GetString("Nazwa_przedmiotu"),
                    NazwaSali = reader.GetString("NazwaSali"),
                    Budynek = reader.GetString("Budynek"),
                    ImieOsoby = reader.GetString("Imie"),
                    NazwiskoOsoby = reader.GetString("Nazwisko")
                };

                rezerwacje.Add(rezerwacja);
            }

            return rezerwacje;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd pobierania wszystkich rezerwacji: {ex.Message}");
            return new List<RezerwacjaKalendarzDto>();
        }
    }

    public async Task<RezerwacjaKalendarzDto?> GetRezerwacjaById(int id)
    {
        try
        {
            using var conn = await _dataSource.OpenConnectionAsync();
            using var cmd = conn.CreateCommand();

            cmd.CommandText = @"
                SELECT sd.ID, sd.ID_sale, sd.ID_osoby, sd.Data, 
                       sd.Godzina_rozpoczecia, sd.Godzina_zakonczenia,
                       sd.Nazwa_przedmiotu, s.Nazwa as NazwaSali, s.Budynek,
                       o.Imie, o.Nazwisko
                FROM sale_dostepnosc sd 
                JOIN sale s ON sd.ID_sale = s.ID
                JOIN osoby o ON sd.ID_osoby = o.ID
                WHERE sd.ID = @id";

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new RezerwacjaKalendarzDto
                {
                    Id = reader.GetInt32("ID"),
                    IdSali = reader.GetInt32("ID_sale"),
                    IdOsoby = reader.GetInt32("ID_osoby"),
                    Data = DateOnly.FromDateTime(reader.GetDateTime("Data")),
                    GodzinaRozpoczecia = TimeOnly.FromTimeSpan(reader.GetTimeSpan("Godzina_rozpoczecia")),
                    GodzinaZakonczenia = TimeOnly.FromTimeSpan(reader.GetTimeSpan("Godzina_zakonczenia")),
                    NazwaPrzedmiotu = reader.GetString("Nazwa_przedmiotu"),
                    NazwaSali = reader.GetString("NazwaSali"),
                    Budynek = reader.GetString("Budynek"),
                    ImieOsoby = reader.GetString("Imie"),
                    NazwiskoOsoby = reader.GetString("Nazwisko")
                };
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd pobierania rezerwacji: {ex.Message}");
            return null;
        }
    }

    public async Task<(bool Success, string Message)> UpdateRezerwacjaAsync(int id, RezerwacjaRequest request)
    {
        try
        {
            var repo = new ReservationRepo(_dataSource);
            return await repo.EdytujRezerwacjeAsync(id, request.Data, request.DataOd, request.DataDo);
        }
        catch (Exception ex)
        {
            return (false, $"Błąd aktualizacji rezerwacji: {ex.Message}");
        }
    }

    public async Task<bool> DeleteRezerwacjaAsync(int id)
    {
        try
        {
            var repo = new ReservationRepo(_dataSource);
            return await repo.UsunRezerwacjeAsync(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd usuwania rezerwacji: {ex.Message}");
            return false;
        }
    }

}
