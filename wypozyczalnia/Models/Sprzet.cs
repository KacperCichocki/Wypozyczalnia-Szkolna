using System;
using System.Collections.Generic;
using System.Text;

namespace wypozyczalnia.Models
{
    public class Sprzet
    {
        public int Id { get; set; }
        public string Nazwa { get; set; } = string.Empty;
        public string Kategoria { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public string NumerSeryjny { get; set; } = string.Empty;
        public string StanTechniczny { get; set; } = "Dobry";
        public bool CzyDostepny { get; set; } = true;
        public string? OsobaWypozyczajaca { get; set; }
        public string Status => CzyDostepny ? "Dostępny" : "Wypożyczony";
        public bool CzyZaznaczony { get; set; }
    }
}
