using System.Collections.Generic;
using System.Collections.ObjectModel;
using wypozyczalnia.Models;

namespace Wypozyczalnia.Data
{
    public static class DaneTestowe
    {
        public static List<Sprzet> Sprzety = new List<Sprzet>
        {
            new Sprzet
            {
                Id = 1,
                Nazwa = "Laptop Dell",
                Kategoria = "Elektronika",
                Opis = "Laptop do nauki programowania",
                NumerSeryjny = "EL-001",
                StanTechniczny = "Dobry",
                CzyDostepny = true
            },

            new Sprzet
            {
                Id = 2,
                Nazwa = "Projektor Epson",
                Kategoria = "Elektronika",
                Opis = "Projektor do prezentacji",
                NumerSeryjny = "EL-002",
                StanTechniczny = "Dobry",
                CzyDostepny = true
            },

            new Sprzet
            {
                Id = 3,
                Nazwa = "Mikroskop",
                Kategoria = "Biologia",
                Opis = "Mikroskop do zajęć laboratoryjnych",
                NumerSeryjny = "BI-001",
                StanTechniczny = "Dobry",
                CzyDostepny = false
            },

            new Sprzet
            {
                Id = 4,
                Nazwa = "Kalkulator naukowy",
                Kategoria = "Matematyka",
                Opis = "Kalkulator do obliczeń matematycznych",
                NumerSeryjny = "MA-001",
                StanTechniczny = "Dobry",
                CzyDostepny = true
            }
        };

        public static ObservableCollection<HistoriaWpisu> Historia { get; set; } = new ObservableCollection<HistoriaWpisu>();
    }
}