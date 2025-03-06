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
            VerificaRegistrazione();
        }

        private void VerificaRegistrazione()
        {
            using (var context = new LibraryContext())
            {
                if (!context.Utenti.Any(u => u.Ruolo == "Operatore"))
                {
                    RegistratiButton.Visibility = Visibility.Visible;
                }
                else
                {
                    RegistratiButton.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            var utente = _utenteService.EffettuaLogin(username, password);

            if (utente != null)
            {
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

        private void ApriFinestraRegistrazione(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
            VerificaRegistrazione();
        }
    }
}
