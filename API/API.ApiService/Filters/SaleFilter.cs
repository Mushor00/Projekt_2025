namespace API.ApiService.Filters;

public class SaleFilter
{
    public string Numer { get; set; } = string.Empty;
    public string Budynek { get; set; } = string.Empty;
    public string Nazwa { get; set; } = string.Empty;
    public int? Pietro { get; set; }
    public int? Pojemnosc { get; set; }
    public string Projektor { get; set; } = string.Empty;
    public string Tablica { get; set; } = string.Empty;
    public int? Klimatyzacja { get; set; }
    public int? Komputerowa { get; set; }
    public string Ulica { get; set; } = string.Empty;
    public int? Niepelnosprawni { get; set; }
    public string Dostepnosc { get; set; } = string.Empty;
}