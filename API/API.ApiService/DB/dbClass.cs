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
        public int ProjektorHDMI { get; set; }
        public int ProjektorVGA { get; set; }
        public int TablicaMultimedialna { get; set; }
        public int TablicaSuchoscieralna { get; set; }
        public int Klimatyzacja { get; set; }
        public int Komputerowa { get; set; }
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

}