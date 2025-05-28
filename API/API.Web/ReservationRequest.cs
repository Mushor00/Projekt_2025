public class RezerwacjaRequest // <-- Ta klasa powinna być wykorzystywana wszędzie
{
    public int NumerSali { get; set; }
    public string Login { get; set; } = string.Empty;
    public DateTime DataOd { get; set; }
    public DateTime DataDo { get; set; }
}
