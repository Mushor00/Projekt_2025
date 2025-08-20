public class RezerwacjaRequest // <-- Ta klasa powinna być wykorzystywana wszędzie
{
    public int NumerSali { get; set; }
    public string Login { get; set; } = string.Empty;
    public string NazwaPrzedmiotu { get; set; } = string.Empty;
    public DateOnly Data { get; set; }
    public TimeOnly DataOd { get; set; }
    public TimeOnly DataDo { get; set; }
}
