using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Business;
using GestionaleLibreria.Data;
using System.Windows;
using GestionaleLibreria.WPF.FormReportistica;
using System.Runtime.Remoting.Contexts;

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
            var context = new LibraryContext();
            var libroRepository = new LibroRepository();
            var clienteRepository = new ClienteRepository();
            var magazzinoRepository = new MagazzinoRepository(context);
            var venditaRepository = new VenditaRepository(); 

            var libroService = new LibroService(libroRepository, magazzinoRepository);
            var clienteService = new ClienteService(clienteRepository);
            var venditaService = new VenditaService(venditaRepository, magazzinoRepository, libroRepository);
          

            var venditaWindow = new VenditaWindow(libroService, clienteService, venditaService);
            venditaWindow.ShowDialog();
        }


        private void GeneraReport_Click(object sender, RoutedEventArgs e)
        {
            var context = new LibraryContext();
            var libroRepository = new LibroRepository();
            var magazzinoRepository = new MagazzinoRepository(context);
            var venditaRepository = new VenditaRepository();

            var venditaService = new VenditaService(venditaRepository, magazzinoRepository, libroRepository);
            var magazzinoService = new MagazzinoService(magazzinoRepository, libroRepository);

            new SelezioneReportWindow(venditaService, magazzinoService).ShowDialog();
        }


        private void GestioneMagazzino_Click(object sender, RoutedEventArgs e)
        {
            new MagazzinoWindow().ShowDialog();
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
