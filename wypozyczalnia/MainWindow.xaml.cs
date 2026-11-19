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
        private readonly PostgresRepository repository = null!;
        private List<Sprzet> sprzety = new();

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                repository = new PostgresRepository();
                repository.Initialize();
                OdswiezDane();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Nie można połączyć się z bazą PostgreSQL. Sprawdź konfigurację SCHOOL_RENTAL_DB i dostępność serwera.\n\nSzczegóły: {ex.Message}",
                    "Błąd połączenia z bazą", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return;
            }
            listaSprzetu.ItemsSource = sprzety;
            txtSzukaj.Text = "";
        }

        private void OdswiezDane()
        {
            sprzety = repository.GetEquipment();
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
            foreach (Sprzet sprzet in sprzety)
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

            var zaznaczone = sprzety
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

            try
            {
                repository.Borrow(zaznaczone, osoba);
                OdswiezDane();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie udało się zapisać wypożyczenia: {ex.Message}", "Błąd bazy danych",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                OdswiezDane();
                return;
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
            var zaznaczone = sprzety
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

            try
            {
                repository.Return(zaznaczone);
                OdswiezDane();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie udało się zapisać zwrotu: {ex.Message}", "Błąd bazy danych",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                OdswiezDane();
                return;
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
            listaSprzetu.ItemsSource = sprzety
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
            try
            {
                listaHistorii.ItemsSource = repository.GetHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie udało się pobrać historii: {ex.Message}", "Błąd bazy danych",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            listaHistorii.Visibility = Visibility.Visible;
        }
    }
}
