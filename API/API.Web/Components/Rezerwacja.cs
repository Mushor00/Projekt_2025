namespace API.Web
{
    public class Rezerwacja
    {
        public int Id { get; set; }
        public int Id_sale { get; set; }
        public int Id_osoby { get; set; }
        public string Nazwa_przedmiotu { get; set; } = string.Empty;
        public DateOnly Data { get; set; }
        public TimeOnly Godzina_rozpoczecia { get; set; }
        public TimeOnly Godzina_zakonczenia { get; set; }
    }

}
