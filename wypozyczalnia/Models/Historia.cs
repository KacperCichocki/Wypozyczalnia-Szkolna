namespace wypozyczalnia.Models
{
    public class HistoriaWpisu
    {
        public DateTime Data { get; set; }
        public string NazwaSprzetu { get; set; } = string.Empty;
        public string NumerSeryjny { get; set; } = string.Empty;
        public string Osoba { get; set; } = string.Empty;
        public string TypAkcji { get; set; } = string.Empty;
    }
}