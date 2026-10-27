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

        private void txtSzukaj_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            string fraza = txtSzukaj.Text.ToLower();

            listaSprzetu.ItemsSource = DaneTestowe.Sprzety
                .Where(s => s.Nazwa.ToLower().Contains(fraza)
                         || s.Kategoria.ToLower().Contains(fraza)
                         || s.NumerSeryjny.ToLower().Contains(fraza))
                .ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (trybZwrotu)
            {
                ZwrocSprzet();
                return;
            }

            if (listaSprzetu.SelectedItem is not Sprzet wybranySprzet)
            {
                MessageBox.Show(
                    "Najpierw wybierz sprzęt z listy.",
                    "Brak wyboru",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            if (!wybranySprzet.CzyDostepny)
            {
                MessageBox.Show(
                    "Ten sprzęt jest już wypożyczony.",
                    "Sprzęt niedostępny",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            var oknoWypozyczenia = new WypozyczenieWindow
            {
                Owner = this
            };

            if (oknoWypozyczenia.ShowDialog() != true)
            {
                return;
            }

            wybranySprzet.OsobaWypozyczajaca = oknoWypozyczenia.OsobaWypozyczajaca;
            wybranySprzet.CzyDostepny = false;

            // ZAPIS DO HISTORII
            DaneTestowe.Historia.Add(new HistoriaWpisu
            {
                Data = DateTime.Now,
                NazwaSprzetu = wybranySprzet.Nazwa,
                NumerSeryjny = wybranySprzet.NumerSeryjny,
                Osoba = wybranySprzet.OsobaWypozyczajaca,
                TypAkcji = "Wypożyczenie"
            });

            OdswiezListe();

            MessageBox.Show(
                $"Sprzęt został wypożyczony osobie: {wybranySprzet.OsobaWypozyczajaca}.",
                "Wypożyczenie zapisane",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void ZwrocSprzet()
        {
            if (listaSprzetu.SelectedItem is not Sprzet wybranySprzet)
            {
                MessageBox.Show(
                    "Najpierw wybierz sprzęt z listy.",
                    "Brak wyboru",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            if (wybranySprzet.CzyDostepny)
            {
                MessageBox.Show(
                    "Ten sprzęt nie jest obecnie wypożyczony.",
                    "Sprzęt dostępny",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
                return;
            }

            string osoba = wybranySprzet.OsobaWypozyczajaca ?? "nieznanej osobie";

            // ZAPIS DO HISTORII
            DaneTestowe.Historia.Add(new HistoriaWpisu
            {
                Data = DateTime.Now,
                NazwaSprzetu = wybranySprzet.Nazwa,
                NumerSeryjny = wybranySprzet.NumerSeryjny,
                Osoba = osoba,
                TypAkcji = "Zwrot"
            });

            wybranySprzet.OsobaWypozyczajaca = null;
            wybranySprzet.CzyDostepny = true;
            OdswiezListe();

            MessageBox.Show(
                $"Sprzęt został zwrócony przez: {osoba}.",
                "Zwrot zapisany",
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