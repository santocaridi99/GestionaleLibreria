using System.Windows;

namespace GestionaleLibreria.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void GestioneLibri_Click(object sender, RoutedEventArgs e)
        {
            // Apri la finestra dedicata alla gestione dei libri
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
