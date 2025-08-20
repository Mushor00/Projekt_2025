using MySqlConnector;

namespace API.ApiService.DB;

public class ReservationRepo(MySqlDataSource database)
{
    public async Task<bool> ZarezerwujSale(int numerSali, string login, string nazwaPrzedmiotu, DateOnly data, TimeOnly godzinaOd, TimeOnly godzinaDo)
    {
        using var conn = await database.OpenConnectionAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM sale_dostepnosc WHERE numerSali = @numerSali AND ((Godzina_rozpoczecia <= @godzinaDo AND Godzina_zakonczenia >= @godzinaOd))";
        cmd.Parameters.AddWithValue("@numerSali", numerSali);
        cmd.Parameters.AddWithValue("@data", data);
        cmd.Parameters.AddWithValue("@godzinaOd", godzinaOd);
        cmd.Parameters.AddWithValue("@godzinaDo", godzinaDo);

        var kolizje = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        if (kolizje > 0) return false;

        cmd.CommandText = "INSERT INTO sale_dostepnosc (ID_sale, ID_osoby, Nazwa_przedmiotu, Data, Godzina_rozpoczecia, Godzina_zakonczenia) VALUES (@numerSali, @idOsoby, @data, @godzinaOd, @godzinaDo)";

        var idOsoby = await GetIdOsobyAsync(login, conn);

        cmd.Parameters.AddWithValue("@idOsoby", idOsoby);
        await cmd.ExecuteNonQueryAsync();
        return true;
    }

    public async Task<int> GetIdOsobyAsync(string login, MySqlConnection conn)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT ID FROM osoby WHERE Login = @login";
        cmd.Parameters.AddWithValue("@login", login);
        var result = await cmd.ExecuteScalarAsync();
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

    public async Task<bool> EdytujRezerwacjeAsync(int id, DateOnly date, TimeOnly nowaOd, TimeOnly nowaDo)
    {
        using var conn = await database.OpenConnectionAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM sale_dostepnosc WHERE ID != @id AND ((Godzina_rozpoczecia <= @nowaDo AND Godzina_zakonczenia >= @nowaOd)) AND Data = @data";
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@nowaOd", nowaOd);
        cmd.Parameters.AddWithValue("@nowaDo", nowaDo);
        cmd.Parameters.AddWithValue("@data", date);
        var kolizje = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        if (kolizje > 0) return false;

        cmd.CommandText = "UPDATE sale_dostepnosc SET Godzina_rozpoczecia = @nowaOd, Godzina_zakonczenia = @nowaDo WHERE ID = @id";
        return await cmd.ExecuteNonQueryAsync() > 0;
    }
}
