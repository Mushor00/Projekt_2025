namespace API.ApiService.DB
{
    public class Sale
    {
        public int Numer { get; set; }
        public string? Budynek { get; set; }
        public string? Nazwa { get; set; }
        public int Pietro { get; set; }
        public int Pojemosc { get; set; }
        public string? Dostepnosc { get; set; }
    }
    public class Osoby
    {
        public string? Login { get; set; }
        public string? Haslo { get; set; }
        public string? Email { get; set; }
        public string? Imie { get; set; }
        public string? Nazwisko { get; set; }
    }

}