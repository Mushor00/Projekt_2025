using MySqlConnector;

namespace API.ApiService.DB;

public class ReservationRepo(MySqlDataSource database)
{
    public async Task<(bool Success, string Message)> ZarezerwujSale(int idSali, string imie, string nazwisko, string nazwaPrzedmiotu, DateOnly data, TimeOnly godzinaOd, TimeOnly godzinaDo)
    {
        using var conn = await database.OpenConnectionAsync();

        // Sprawdź kolizje z istniejącymi rezerwacjami
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM sale_dostepnosc WHERE ID_sale = @idSali AND Data = @data AND ((Godzina_rozpoczecia <= @godzinaDo AND Godzina_zakonczenia >= @godzinaOd))";
        cmd.Parameters.AddWithValue("@idSali", idSali);
        cmd.Parameters.AddWithValue("@data", data);
        cmd.Parameters.AddWithValue("@godzinaOd", godzinaOd);
        cmd.Parameters.AddWithValue("@godzinaDo", godzinaDo);

        var kolizje = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        if (kolizje > 0) return (false, "Sala jest już zajęta w podanym terminie");

        // Pobierz ID osoby
        var idOsoby = await GetIdOsobyAsync(imie, nazwisko, conn);
        if (idOsoby == 0)
        {
            return (false, $"Nie znaleziono użytkownika: {imie} {nazwisko}");
        }
        else if (idOsoby == -1)
        {
            return (false, $"Znaleziono kilka osób o imieniu {imie}. Podaj pełne imię i nazwisko.");
        }

        // Dodaj rezerwację
        cmd.CommandText = "INSERT INTO sale_dostepnosc (ID_sale, ID_osoby, Nazwa_przedmiotu, Data, Godzina_rozpoczecia, Godzina_zakonczenia) VALUES (@idSali, @idOsoby, @nazwaPrzedmiotu, @data, @godzinaOd, @godzinaDo)";
        cmd.Parameters.Clear();
        cmd.Parameters.AddWithValue("@idSali", idSali);
        cmd.Parameters.AddWithValue("@idOsoby", idOsoby);
        cmd.Parameters.AddWithValue("@nazwaPrzedmiotu", nazwaPrzedmiotu);
        cmd.Parameters.AddWithValue("@data", data);
        cmd.Parameters.AddWithValue("@godzinaOd", godzinaOd);
        cmd.Parameters.AddWithValue("@godzinaDo", godzinaDo);
        await cmd.ExecuteNonQueryAsync();
        return (true, "Rezerwacja została dodana pomyślnie");
    }

    public async Task<int> GetIdOsobyAsync(string imie, string nazwisko, MySqlConnection conn)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT ID FROM osoby WHERE Imie = @imie AND Nazwisko = @nazwisko";
        cmd.Parameters.AddWithValue("@imie", imie);
        cmd.Parameters.AddWithValue("@nazwisko", nazwisko);
        var result = await cmd.ExecuteScalarAsync();

        // Jeśli nie znajdziemy dokładnego dopasowania, sprawdź czy jest tylko jedno imię
        if (result == null)
        {
            cmd.CommandText = "SELECT COUNT(*), ID FROM osoby WHERE Imie = @imie";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@imie", imie);
            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var count = reader.GetInt32(0);
                if (count == 1)
                {
                    return reader.GetInt32(1); // Zwróć ID jeśli jest tylko jedna osoba o tym imieniu
                }
                else if (count > 1)
                {
                    Console.WriteLine($"Znaleziono {count} osób o imieniu {imie}. Wymagane pełne imię i nazwisko.");
                    return -1; // Specjalna wartość oznaczająca niejednoznaczność
                }
            }
        }

        return result != null ? Convert.ToInt32(result) : 0;
    }

    public async Task<bool> UsunRezerwacjeAsync(int id)
    {
        using var conn = await database.OpenConnectionAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM sale_dostepnosc WHERE ID = @id";
        cmd.Parameters.AddWithValue("@id", id);
        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<(bool Success, string Message)> EdytujRezerwacjeAsync(int id, DateOnly date, TimeOnly nowaOd, TimeOnly nowaDo)
    {
        using var conn = await database.OpenConnectionAsync();
        using var cmd = conn.CreateCommand();

        // Sprawdź kolizje z innymi rezerwacjami (ale nie z tą samą)
        cmd.CommandText = "SELECT COUNT(*) FROM sale_dostepnosc WHERE ID != @id AND Data = @data AND ((Godzina_rozpoczecia <= @nowaDo AND Godzina_zakonczenia >= @nowaOd))";
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@data", date);
        cmd.Parameters.AddWithValue("@nowaOd", nowaOd);
        cmd.Parameters.AddWithValue("@nowaDo", nowaDo);

        var kolizje = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        if (kolizje > 0) return (false, "Sala jest już zajęta w podanym terminie");

        // Zaktualizuj rezerwację
        cmd.CommandText = "UPDATE sale_dostepnosc SET Data = @data, Godzina_rozpoczecia = @nowaOd, Godzina_zakonczenia = @nowaDo WHERE ID = @id";
        var updated = await cmd.ExecuteNonQueryAsync() > 0;

        return updated ? (true, "Rezerwacja została zaktualizowana pomyślnie") : (false, "Nie udało się zaktualizować rezerwacji");
    }
}
