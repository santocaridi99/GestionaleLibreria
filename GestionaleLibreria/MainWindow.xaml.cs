using System.Windows;

namespace GestionaleLibreria.WPF
{
    public partial class MainWindow : Window
    {
        private readonly string _ruoloUtente;
        private readonly string _nomeUtente;

        public MainWindow(string ruoloUtente, string nomeUtente)
        {
            InitializeComponent();
            _ruoloUtente = ruoloUtente;
            _nomeUtente = nomeUtente;

            UtenteTextBlock.Text = $"👤 {_nomeUtente} ({_ruoloUtente})";
            ConfiguraInterfaccia();
        }

        private void ConfiguraInterfaccia()
        {
            bool isOperatore = _ruoloUtente == "Operatore";

            GestioneLibriButton.Visibility = isOperatore ? Visibility.Collapsed : Visibility.Visible;
            GestioneMagazzinoButton.Visibility = isOperatore ? Visibility.Collapsed : Visibility.Visible;
        }

        private void GestioneLibri_Click(object sender, RoutedEventArgs e)
        {
            new LibriWindow().ShowDialog();
        }

        private void GestioneClienti_Click(object sender, RoutedEventArgs e)
        {
            new ClientiWindow().ShowDialog();
        }

        private void RegistraVendita_Click(object sender, RoutedEventArgs e)
        {
            new VenditaWindow().ShowDialog();
        }

        private void GeneraReport_Click(object sender, RoutedEventArgs e)
        {
            // implementazione futura
        }

        private void GestioneMagazzino_Click(object sender, RoutedEventArgs e)
        {
            // implementazione futura
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            Application.Current.MainWindow = loginWindow;
            loginWindow.Show();
            Close();
        }
    }
}
