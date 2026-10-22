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
using Wypozyczalnia.Data;

namespace wypozyczalnia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
    }
}