using API.ApiService.Models;
using MySqlConnector;

namespace API.ApiService
{
    public class SalaRepo
    {
        private readonly MySqlDataSource _dataSource;

        public SalaRepo(MySqlDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public async Task<List<Sala>> PobierzSaleAsync()
        {
            var sale = new List<Sala>();

            await using var conn = await _dataSource.OpenConnectionAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM sale";

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                sale.Add(new Sala
                {
                    Id = reader.GetInt32("id"),
                    Numer = reader.GetInt32("numer"),
                    Budynek = reader.GetString("budynek"),
                    Nazwa = reader.GetString("nazwa"),
                    Pietro = reader.GetInt32("pietro"),
                    Pojemnosc = reader.GetInt32("pojemnosc"),
                    Dostepna = reader.GetBoolean("dostepna")
                });
            }

            return sale;
        }

        public async Task<bool> ZapiszSaleAsync(Sala sala)
        {
            await using var conn = await _dataSource.OpenConnectionAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE sale SET numer = @numer, budynek = @budynek, nazwa = @nazwa, pietro = @pietro, pojemnosc = @pojemnosc, dostepna = @dostepna WHERE id = @id";

            cmd.Parameters.AddWithValue("@numer", sala.Numer);
            cmd.Parameters.AddWithValue("@budynek", sala.Budynek);
            cmd.Parameters.AddWithValue("@nazwa", sala.Nazwa);
            cmd.Parameters.AddWithValue("@pietro", sala.Pietro);
            cmd.Parameters.AddWithValue("@pojemnosc", sala.Pojemnosc);
            cmd.Parameters.AddWithValue("@dostepna", sala.Dostepna);
            cmd.Parameters.AddWithValue("@id", sala.Id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> UpdateSalaAsync(Sala sala)
        {
            await using var conn = await _dataSource.OpenConnectionAsync();
            await using var cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE sale SET numer = @numer, budynek = @budynek, nazwa = @nazwa, pietro = @pietro, pojemnosc = @pojemnosc, dostepna = @dostepna WHERE id = @id";

            cmd.Parameters.AddWithValue("@numer", sala.Numer);
            cmd.Parameters.AddWithValue("@budynek", sala.Budynek);
            cmd.Parameters.AddWithValue("@nazwa", sala.Nazwa);
            cmd.Parameters.AddWithValue("@pietro", sala.Pietro);
            cmd.Parameters.AddWithValue("@pojemnosc", sala.Pojemnosc);
            cmd.Parameters.AddWithValue("@dostepna", sala.Dostepna);
            cmd.Parameters.AddWithValue("@id", sala.Id);

            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }

}
