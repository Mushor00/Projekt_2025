using MySqlConnector;
namespace API.ApiService.DB
{
    public class ReservationRepo
    {
        private readonly MySqlDataSource _database;

        // Konstruktor – wstrzyknięcie MySqlDataSource (działa dzięki Program.cs)
        public ReservationRepo(MySqlDataSource database)
        {
            _database = database;
        }

        // Główna metoda rezerwacji sali
        public async Task<bool> ZarezerwujSale(int numerSali, string login, DateTime dataOd, DateTime dataDo)
        {
            try
            {
                using var connection = await _database.OpenConnectionAsync();

                // 1. Sprawdzenie kolizji
                using var checkCommand = connection.CreateCommand();
                checkCommand.CommandText = @"
                    SELECT COUNT(*) FROM rezerwacje 
                    WHERE numerSali = @numerSali 
                      AND (
                          (dataOd <= @dataDo AND dataDo >= @dataOd)
                      )";
                checkCommand.Parameters.AddWithValue("@numerSali", numerSali);
                checkCommand.Parameters.AddWithValue("@dataOd", dataOd);
                checkCommand.Parameters.AddWithValue("@dataDo", dataDo);

                var kolizje = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());

                if (kolizje > 0)
                    return false; // Kolizja – nie rezerwuj

                // 2. Wstawienie nowej rezerwacji
                using var insertCommand = connection.CreateCommand();
                insertCommand.CommandText = @"
                    INSERT INTO rezerwacje (numerSali, loginOsoby, dataOd, dataDo) 
                    VALUES (@numerSali, @login, @dataOd, @dataDo)";
                insertCommand.Parameters.AddWithValue("@numerSali", numerSali);
                insertCommand.Parameters.AddWithValue("@login", login);
                insertCommand.Parameters.AddWithValue("@dataOd", dataOd);
                insertCommand.Parameters.AddWithValue("@dataDo", dataDo);

                await insertCommand.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy rezerwacji: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> UsunRezerwacjeAsync(int idRezerwacji)
        {
            try
            {
                using var connection = await _database.OpenConnectionAsync();
                using var command = connection.CreateCommand();

                command.CommandText = "DELETE FROM rezerwacje WHERE id = @id";
                command.Parameters.AddWithValue("@id", idRezerwacji);

                int rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy usuwaniu rezerwacji: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> EdytujRezerwacjeAsync(int idRezerwacji, DateTime nowaDataOd, DateTime nowaDataDo)
        {
            try
            {
                using var connection = await _database.OpenConnectionAsync();

                // Sprawdź kolizje z innymi rezerwacjami
                using var checkCommand = connection.CreateCommand();
                checkCommand.CommandText = @"
            SELECT COUNT(*) FROM rezerwacje 
            WHERE id != @idRezerwacji
              AND (
                  (dataOd <= @nowaDataDo AND dataDo >= @nowaDataOd)
              )";
                checkCommand.Parameters.AddWithValue("@idRezerwacji", idRezerwacji);
                checkCommand.Parameters.AddWithValue("@nowaDataOd", nowaDataOd);
                checkCommand.Parameters.AddWithValue("@nowaDataDo", nowaDataDo);

                var kolizje = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());

                if (kolizje > 0)
                    return false;

                // Aktualizacja
                using var updateCommand = connection.CreateCommand();
                updateCommand.CommandText = @"
            UPDATE rezerwacje 
            SET dataOd = @nowaDataOd, dataDo = @nowaDataDo 
            WHERE id = @idRezerwacji";
                updateCommand.Parameters.AddWithValue("@nowaDataOd", nowaDataOd);
                updateCommand.Parameters.AddWithValue("@nowaDataDo", nowaDataDo);
                updateCommand.Parameters.AddWithValue("@idRezerwacji", idRezerwacji);

                int rowsAffected = await updateCommand.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy edytowaniu rezerwacji: {ex.Message}");
                return false;
            }
        }

    }
}
