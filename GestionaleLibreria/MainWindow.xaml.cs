using System.Windows;

namespace GestionaleLibreria.WPF
{
    public partial class MainWindow : Window
    {
        private string ruoloUtente;

        // Costruttore senza parametri richiesto da WPF
        public MainWindow() : this("Admin") { }  // Imposta "Admin" come valore di default

        // Costruttore principale che riceve il ruolo dell'utente
        public MainWindow(string ruolo)
        {
            InitializeComponent();
            ruoloUtente = ruolo;
            ConfiguraInterfaccia();
        }

        private void ConfiguraInterfaccia()
        {
            if (ruoloUtente == "Operatore")
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
