namespace API.ApiService.Models;

public class RezerwacjaRequest
{
    public int NumerSali { get; set; }
    public string Login { get; set; } = string.Empty;
    public DateTime DataOd { get; set; }
    public DateTime DataDo { get; set; }
}

public class EdytujRezerwacjeRequest
{
    public DateTime NowaDataOd { get; set; }
    public DateTime NowaDataDo { get; set; }
}
public class Sala
{
    public int Id { get; set; }
    public int Numer { get; set; }
    public string Budynek { get; set; }
    public string Nazwa { get; set; }
    public int Pietro { get; set; }
    public int Pojemnosc { get; set; }
    public bool Dostepna { get; set; }
}

public class RezerwacjaDto
{
    public int Id { get; set; }
    public int IdSali { get; set; }
    public int IdOsoby { get; set; }
    public DateTime Data { get; set; }
    public TimeSpan GodzinaRozpoczecia { get; set; }
    public TimeSpan GodzinaZakonczenia { get; set; }
}