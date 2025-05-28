using MySqlConnector;

namespace API.ApiService.DB;

public class ReservationRepo(MySqlDataSource database)
{
    public async Task<bool> ZarezerwujSale(int numerSali, string login, DateTime dataOd, DateTime dataDo)
    {
        using var conn = await database.OpenConnectionAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            SELECT COUNT(*) FROM rezerwacje 
            WHERE numerSali = @numerSali 
              AND ((dataOd <= @dataDo AND dataDo >= @dataOd))
        """;
        cmd.Parameters.AddWithValue("@numerSali", numerSali);
        cmd.Parameters.AddWithValue("@dataOd", dataOd);
        cmd.Parameters.AddWithValue("@dataDo", dataDo);

        var kolizje = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        if (kolizje > 0) return false;

        cmd.CommandText = """
            INSERT INTO rezerwacje (numerSali, loginOsoby, dataOd, dataDo) 
            VALUES (@numerSali, @login, @dataOd, @dataDo)
        """;
        cmd.Parameters.AddWithValue("@login", login);
        await cmd.ExecuteNonQueryAsync();
        return true;
    }

    public async Task<bool> UsunRezerwacjeAsync(int id)
    {
        using var conn = await database.OpenConnectionAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM rezerwacje WHERE id = @id";
        cmd.Parameters.AddWithValue("@id", id);
        return await cmd.ExecuteNonQueryAsync() > 0;
    }

    public async Task<bool> EdytujRezerwacjeAsync(int id, DateTime nowaOd, DateTime nowaDo)
    {
        using var conn = await database.OpenConnectionAsync();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = """
            SELECT COUNT(*) FROM rezerwacje 
            WHERE id != @id 
              AND ((dataOd <= @nowaDo AND dataDo >= @nowaOd))
        """;
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@nowaOd", nowaOd);
        cmd.Parameters.AddWithValue("@nowaDo", nowaDo);
        var kolizje = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        if (kolizje > 0) return false;

        cmd.CommandText = """
            UPDATE rezerwacje SET dataOd = @nowaOd, dataDo = @nowaDo WHERE id = @id
        """;
        return await cmd.ExecuteNonQueryAsync() > 0;
    }
}
