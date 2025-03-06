using System.Windows;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class MainWindow : Window
    {
        private readonly string _ruoloUtente;

        public MainWindow() : this("Operatore") { }

        public MainWindow(string ruoloUtente)
        {
            InitializeComponent();
            _ruoloUtente = ruoloUtente;
            ConfiguraInterfaccia();
        }

        private void ConfiguraInterfaccia()
        {
            if (_ruoloUtente == "Operatore")
            {
                GestioneLibriButton.IsEnabled = false;
                GestioneLibriButton.Visibility = Visibility.Collapsed;

                GestioneMagazzinoButton.IsEnabled = false;
                GestioneMagazzinoButton.Visibility = Visibility.Collapsed;
            }
        }

        private void GestioneLibri_Click(object sender, RoutedEventArgs e)
        {
            LibriWindow libriWindow = new LibriWindow();
            libriWindow.ShowDialog();
        }

        private void GestioneClienti_Click(object sender, RoutedEventArgs e)
        {
            var clientiWindow = new ClientiWindow();
            clientiWindow.ShowDialog();
        }

        private void RegistraVendita_Click(object sender, RoutedEventArgs e)
        {
            var venditaWindow = new VenditaWindow();
            venditaWindow.ShowDialog();
        }

        private void GeneraReport_Click(object sender, RoutedEventArgs e)
        {
            //var reportWindow = new ReportWindow();
            //reportWindow.ShowDialog();
        }

        private void GestioneMagazzino_Click(object sender, RoutedEventArgs e)
        {
            //var magazzinoWindow = new MagazzinoWindow();
            //magazzinoWindow.ShowDialog();
        }
    }
}
