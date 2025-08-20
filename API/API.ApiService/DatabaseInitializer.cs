using MySqlConnector;
using System.Text;

namespace API.ApiService;

public class DatabaseInitializer
{
    private readonly MySqlDataSource _dataSource;

    public DatabaseInitializer(MySqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task InitializeAsync()
    {
        try
        {
            using var connection = await _dataSource.OpenConnectionAsync();

            // Sprawdź czy baza danych już istnieje i ma tabele
            var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = "SHOW TABLES LIKE 'sale'";
            var result = await checkCommand.ExecuteScalarAsync();

            if (result != null)
            {
                Console.WriteLine("Baza danych już została zainicjalizowana.");
                return;
            }

            Console.WriteLine("Inicjalizacja bazy danych...");

            // Odczytaj plik SQL i wykonaj polecenia
            var sqlScript = await File.ReadAllTextAsync("san.sql");

            // Usuń komentarze i przetwórz skrypt
            var cleanedScript = CleanSqlScript(sqlScript);

            // Wykonaj polecenie po poleceniu
            await ExecuteCommandByCommand(connection, cleanedScript);

            Console.WriteLine("Baza danych została zainicjalizowana pomyślnie.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd inicjalizacji bazy danych: {ex.Message}");
            throw;
        }
    }

    private static string CleanSqlScript(string script)
    {
        var lines = script.Split('\n');
        var cleanedLines = new List<string>();

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();

            // Pomiń komentarze i puste linie
            if (string.IsNullOrEmpty(trimmedLine) ||
                trimmedLine.StartsWith("--") ||
                trimmedLine.StartsWith("/*") ||
                trimmedLine.StartsWith("/*!") ||
                trimmedLine.Contains("phpMyAdmin") ||
                trimmedLine.StartsWith("SET SQL_MODE") ||
                trimmedLine.StartsWith("START TRANSACTION") ||
                trimmedLine.StartsWith("SET time_zone") ||
                trimmedLine.StartsWith("CREATE DATABASE") ||
                trimmedLine.StartsWith("CREATE USER") ||
                trimmedLine.StartsWith("GRANT ALL") ||
                trimmedLine.StartsWith("USE `") ||
                trimmedLine == "COMMIT;")
            {
                continue;
            }

            cleanedLines.Add(line); // Zachowaj oryginalne formatowanie
        }

        return string.Join("\n", cleanedLines);
    }

    private static async Task ExecuteCommandByCommand(MySqlConnection connection, string script)
    {
        // Podział na polecenia - ale zachowuj wieloliniowe INSERT
        var commands = SplitSqlCommands(script);

        foreach (var commandText in commands)
        {
            if (string.IsNullOrWhiteSpace(commandText)) continue;

            try
            {
                var command = connection.CreateCommand();
                command.CommandText = commandText.Trim();
                await command.ExecuteNonQueryAsync();

                var preview = commandText.Trim().Replace("\n", " ").Replace("\r", "");
                Console.WriteLine($"Wykonano: {preview.Substring(0, Math.Min(80, preview.Length))}...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd wykonywania polecenia: {ex.Message}");
                Console.WriteLine($"Polecenie: {commandText.Substring(0, Math.Min(200, commandText.Length))}...");
            }
        }
    }

    private static List<string> SplitSqlCommands(string script)
    {
        var commands = new List<string>();
        var currentCommand = new StringBuilder();
        var inString = false;
        var stringChar = '\0';

        for (int i = 0; i < script.Length; i++)
        {
            char c = script[i];

            // Obsługa stringów w SQL (pojedyncze i podwójne cudzysłowy)
            if (!inString && (c == '\'' || c == '"'))
            {
                inString = true;
                stringChar = c;
                currentCommand.Append(c);
            }
            else if (inString && c == stringChar)
            {
                // Sprawdź czy to nie jest escaped quote
                if (i + 1 < script.Length && script[i + 1] == stringChar)
                {
                    // Escaped quote - dodaj oba znaki
                    currentCommand.Append(c);
                    currentCommand.Append(script[++i]);
                }
                else
                {
                    // Koniec stringa
                    inString = false;
                    stringChar = '\0';
                    currentCommand.Append(c);
                }
            }
            else if (!inString && c == ';')
            {
                // Koniec polecenia SQL
                currentCommand.Append(c);
                var command = currentCommand.ToString().Trim();
                if (!string.IsNullOrEmpty(command))
                {
                    commands.Add(command);
                }
                currentCommand.Clear();
            }
            else
            {
                currentCommand.Append(c);
            }
        }

        // Dodaj ostatnie polecenie jeśli istnieje
        var lastCommand = currentCommand.ToString().Trim();
        if (!string.IsNullOrEmpty(lastCommand))
        {
            commands.Add(lastCommand);
        }

        return commands;
    }
}
