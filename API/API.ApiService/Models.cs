namespace API.ApiService.Models;

public class RezerwacjaRequest
{
    public int IdSali { get; set; }
    public string Imie { get; set; } = string.Empty;
    public string Nazwisko { get; set; } = string.Empty;
    public string NazwaPrzedmiotu { get; set; } = string.Empty;
    public DateOnly Data { get; set; }
    public TimeOnly DataOd { get; set; }
    public TimeOnly DataDo { get; set; }
}

public class EdytujRezerwacjeRequest
{
    public string NazwaPrzedmiotu { get; set; } = string.Empty;
    public DateOnly Data { get; set; }
    public TimeOnly NowaGodzinaOd { get; set; }
    public TimeOnly NowaGodzinaDo { get; set; }
}
public class Sala
{
    public int Id { get; set; }
    public int Numer { get; set; }
    public string Budynek { get; set; } = string.Empty;
    public string Nazwa { get; set; } = string.Empty;
    public int Pietro { get; set; }
    public int Pojemnosc { get; set; }
    public bool Dostepna { get; set; }
}

public class RezerwacjaDto
{
    public int Id { get; set; }
    public int IdSali { get; set; }
    public int IdOsoby { get; set; }
    public DateOnly Data { get; set; }
    public TimeOnly GodzinaRozpoczecia { get; set; }
    public TimeOnly GodzinaZakonczenia { get; set; }
}