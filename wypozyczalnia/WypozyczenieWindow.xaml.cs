using System.Windows;

namespace wypozyczalnia
{
    public partial class WypozyczenieWindow : Window
    {
        public string OsobaWypozyczajaca { get; private set; } = string.Empty;

        public WypozyczenieWindow()
        {
            InitializeComponent();
            Loaded += (_, _) => txtOsoba.Focus();
        }

        private void Zatwierdz_Click(object sender, RoutedEventArgs e)
        {
            string osoba = txtOsoba.Text.Trim();

            if (string.IsNullOrWhiteSpace(osoba))
            {
                MessageBox.Show(
                    "Wpisz imię i nazwisko osoby wypożyczającej.",
                    "Brak danych",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                txtOsoba.Focus();
                return;
            }

            OsobaWypozyczajaca = osoba;
            DialogResult = true;
        }

        private void Anuluj_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
