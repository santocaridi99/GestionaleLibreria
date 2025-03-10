using System.Linq;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Logging;

namespace GestionaleLibreria.WPF
{
    public partial class LoginWindow : Window
    {
        private readonly UtenteService _utenteService;
        private static readonly string NomeClasse = nameof(LoginWindow);

        public LoginWindow()
        {
            string nomeMetodo = nameof(LoginWindow);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Avvio finestra di login.");
                InitializeComponent();
                _utenteService = new UtenteService(new UtenteRepository(new LibraryContext()));
            }
            catch (System.Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante l'inizializzazione della finestra di login.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(Login_Click);
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Tentativo di login per l'utente: {username}");

                var utente = _utenteService.EffettuaLogin(username, password);

                if (utente != null)
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Login riuscito per l'utente: {username} (Ruolo: {utente.Ruolo})");

                    MainWindow mainWindow = new MainWindow(utente.Ruolo, utente.Username);
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Tentativo di login fallito per l'utente: {username}");
                    ErroreTextBlock.Text = "Credenziali errate!";
                    ErroreTextBlock.Visibility = Visibility.Visible;
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante il login.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApriFinestraRegistrazione(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(ApriFinestraRegistrazione);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Apertura finestra registrazione utente.");
                var registerWindow = new RegisterWindow();
                registerWindow.ShowDialog();
            }
            catch (System.Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante l'apertura della finestra di registrazione.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
