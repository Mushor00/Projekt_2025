namespace API.Web
{
    public class Rezerwacja
    {
        public int Id { get; set; }
        public int Id_sale { get; set; }
        public int Id_osoby { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Godzina_rozpoczecia { get; set; }
        public TimeSpan Godzina_zakonczenia { get; set; }
    }

}
