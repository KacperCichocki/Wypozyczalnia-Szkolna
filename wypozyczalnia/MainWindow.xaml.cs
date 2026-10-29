using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wypozyczalnia.Models;
using Wypozyczalnia.Data;
using System.Linq;

namespace wypozyczalnia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool trybZwrotu;

        public MainWindow()
        {
            InitializeComponent();

            listaSprzetu.ItemsSource = DaneTestowe.Sprzety;
            txtSzukaj.Text = "";
        }
        private void chkZaznaczWszystkie_Checked(
    object sender, RoutedEventArgs e)
        {
            ZaznaczWidoczne(true);
        }

        private void chkZaznaczWszystkie_Unchecked(
            object sender, RoutedEventArgs e)
        {
            ZaznaczWidoczne(false);
        }

        private void ZaznaczWidoczne(bool zaznacz)
        {
            foreach (Sprzet sprzet in listaSprzetu.Items)
            {
                if (trybZwrotu)
                {
                    sprzet.CzyZaznaczony = zaznacz && !sprzet.CzyDostepny;
                }
                else
                {
                    sprzet.CzyZaznaczony = zaznacz && sprzet.CzyDostepny;
                }
            }

            listaSprzetu.Items.Refresh();
        }

        private void txtSzukaj_TextChanged(
            object sender, TextChangedEventArgs e)
        {
            foreach (Sprzet sprzet in DaneTestowe.Sprzety)
                sprzet.CzyZaznaczony = false;

            chkZaznaczWszystkie.IsChecked = false;

            OdswiezListe();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (trybZwrotu)
            {
                ZwrocSprzet();
                return;
            }

            var zaznaczone = DaneTestowe.Sprzety
                .Where(s => s.CzyZaznaczony && s.CzyDostepny)
                .ToList();

            if (zaznaczone.Count == 0)
            {
                if (listaSprzetu.SelectedItem is Sprzet wybrany
                    && wybrany.CzyDostepny)
                {
                    zaznaczone.Add(wybrany);
                }
                else
                {
                    MessageBox.Show(
                        "Zaznacz checkboxem lub wybierz dostępny sprzęt.",
                        "Brak wyboru",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    return;
                }
            }

            var oknoWypozyczenia = new WypozyczenieWindow
            {
                Owner = this
            };

            if (oknoWypozyczenia.ShowDialog() != true)
                return;

            string osoba = oknoWypozyczenia.OsobaWypozyczajaca;

            foreach (Sprzet sprzet in zaznaczone)
            {
                sprzet.OsobaWypozyczajaca = osoba;
                sprzet.CzyDostepny = false;
                sprzet.CzyZaznaczony = false;

                DaneTestowe.Historia.Add(new HistoriaWpisu
                {
                    Data = DateTime.Now,
                    NazwaSprzetu = sprzet.Nazwa,
                    NumerSeryjny = sprzet.NumerSeryjny,
                    Osoba = osoba,
                    TypAkcji = "Wypożyczenie"
                });
            }

            chkZaznaczWszystkie.IsChecked = false;
            OdswiezListe();
            
            string liczbaPrzedmiotow = zaznaczone.Count == 1
                ? "przedmiot"
                : zaznaczone.Count >= 2 && zaznaczone.Count <= 4
                    ? "przedmioty"
                    : "przedmiotów";

            MessageBox.Show(
                $"Wypożyczono {zaznaczone.Count} {liczbaPrzedmiotow} osobie: {osoba}.",
                "Wypożyczenie zapisane",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        private void ZwrocSprzet()
        {
            var zaznaczone = DaneTestowe.Sprzety
                .Where(s => s.CzyZaznaczony && !s.CzyDostepny)
                .ToList();

            if (zaznaczone.Count == 0 &&
                listaSprzetu.SelectedItem is Sprzet wybranySprzet &&
                !wybranySprzet.CzyDostepny)
            {
                zaznaczone.Add(wybranySprzet);
            }

            if (zaznaczone.Count == 0)
            {
                MessageBox.Show(
                    "Zaznacz przynajmniej jeden wypożyczony przedmiot.",
                    "Brak wyboru",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            foreach (Sprzet sprzet in zaznaczone)
            {
                string osoba = sprzet.OsobaWypozyczajaca ?? "Nieznana osoba";

                // ZAPIS DO HISTORII
                DaneTestowe.Historia.Add(new HistoriaWpisu
                {
                    Data = DateTime.Now,
                    NazwaSprzetu = sprzet.Nazwa,
                    NumerSeryjny = sprzet.NumerSeryjny,
                    Osoba = osoba,
                    TypAkcji = "Zwrot"
                });

                sprzet.OsobaWypozyczajaca = null;
                sprzet.CzyDostepny = true;
                sprzet.CzyZaznaczony = false;
            }

            chkZaznaczWszystkie.IsChecked = false;

            OdswiezListe();

            string liczbaPrzedmiotow = zaznaczone.Count == 1
                ? "przedmiot"
                : zaznaczone.Count >= 2 && zaznaczone.Count <= 4
                    ? "przedmioty"
                    : "przedmiotów";

            MessageBox.Show(
                $"Zwrócono {zaznaczone.Count} {liczbaPrzedmiotow}.",
                "Zwrot sprzętu",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        private void OdswiezListe()
        {
            string fraza = txtSzukaj.Text.ToLower();
            listaSprzetu.ItemsSource = DaneTestowe.Sprzety
                .Where(s => s.Nazwa.ToLower().Contains(fraza)
                         || s.Kategoria.ToLower().Contains(fraza)
                         || s.NumerSeryjny.ToLower().Contains(fraza))
                .ToList();
        }

        private void WypozyczSprzet_Click(object sender, RoutedEventArgs e)
        {
            trybZwrotu = false;
            btnAkcja.Content = "Wypożycz wybrany sprzęt";
            btnAkcja.Background = Brushes.Blue;

            // Powrót do widoku sprzętu
            txtNaglowek.Text = "Dostępny sprzęt";
            txtSzukaj.Visibility = Visibility.Visible;
            btnAkcja.Visibility = Visibility.Visible;
            listaHistorii.Visibility = Visibility.Collapsed;
            listaSprzetu.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            trybZwrotu = true;
            btnAkcja.Content = "Zwróć wybrany sprzęt";
            btnAkcja.Background = Brushes.Green;

            // Powrót do widoku sprzętu
            txtNaglowek.Text = "Dostępny sprzęt";
            txtSzukaj.Visibility = Visibility.Visible;
            btnAkcja.Visibility = Visibility.Visible;
            listaHistorii.Visibility = Visibility.Collapsed;
            listaSprzetu.Visibility = Visibility.Visible;
        }

        private void HistoriaWpozyczen_Click(object sender, RoutedEventArgs e)
        {
            // Przełączenie widoku na historię
            txtNaglowek.Text = "Historia wypożyczeń";
            txtSzukaj.Visibility = Visibility.Collapsed;
            btnAkcja.Visibility = Visibility.Collapsed;

            listaSprzetu.Visibility = Visibility.Collapsed;
            listaHistorii.ItemsSource = DaneTestowe.Historia;
            listaHistorii.Visibility = Visibility.Visible;
        }
    }
}