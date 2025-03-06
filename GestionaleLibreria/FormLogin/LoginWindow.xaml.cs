using System.Linq;
using System.Windows;
using GestionaleLibreria.Business;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class LoginWindow : Window
    {
        private readonly UtenteService _utenteService;

        public LoginWindow()
        {
            InitializeComponent();
            IUtenteRepository utenteRepository = new UtenteRepository(new LibraryContext());
            _utenteService = new UtenteService(utenteRepository);

            // Se il database non ha utenti, chiedi di registrare uno
            if (!HaUtentiRegistrati())
            {
                MessageBox.Show("Non ci sono utenti registrati. Creiamo un nuovo account.", "Registrazione");
                ApriRegistrazione();
            }
        }

        private bool HaUtentiRegistrati()
        {
            return _utenteService.EffettuaLogin("admin", "admin") != null || new LibraryContext().Utenti.Any();
        }

        private void ApriRegistrazione()
        {
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            var utente = _utenteService.EffettuaLogin(username, password);

            if (utente != null)
            {
                // Passa il ruolo dell'utente alla MainWindow
                MainWindow mainWindow = new MainWindow(utente.Ruolo);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ErroreTextBlock.Text = "Credenziali errate!";
                ErroreTextBlock.Visibility = Visibility.Visible;
            }
        }

    }
}
