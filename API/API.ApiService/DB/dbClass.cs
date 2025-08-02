namespace API.ApiService.DB
{
    public class Sale
    {
        public int Numer { get; set; }
        public string? Budynek { get; set; }
        public string? Nazwa { get; set; }
        public int Pietro { get; set; }
        public int Pojemnosc { get; set; }
        public string? Dostepnosc { get; set; }
        public int Projektor { get; set; }
        public int Tablica { get; set; }
        public int Klimatyzacja { get; set; }
        public int Komputerowa { get; set; }
        public string? Ulica { get; set; }
        public int Niepelnosprawni { get; set; }
    }
    public class Osoby
    {
        public string? Login { get; set; }
        public string? Haslo { get; set; }
        public string? Email { get; set; }
        public string? Imie { get; set; }
        public string? Nazwisko { get; set; }
        public string? NumerAlbumu { get; set; }
    }
    public class Rezerwacje
    {
        public int Id { get; set; }
        public int ID_sala { get; set; }
        public int ID_osoby { get; set; }
        public DateOnly Data { get; set; }
        public TimeOnly GodzinaRozpoczecia { get; set; }
        public TimeOnly GodzinaZakonczenia { get; set; }
    }

}