using System.Linq;
using System.Windows;
using GestionaleLibreria.Business;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;

namespace GestionaleLibreria.WPF
{
    public partial class LoginWindow : Window
    {
        private readonly UtenteService _utenteService;

        public LoginWindow()
        {
            InitializeComponent();
            _utenteService = new UtenteService(new UtenteRepository(new LibraryContext()));
           
        }

        //private void VerificaRegistrazione()
        //{
        //    var context = new LibraryContext();
        //    if (!context.Utenti.Any(u => u.Ruolo == "Operatore"))
        //    {
        //        RegistratiButton.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        RegistratiButton.Visibility = Visibility.Collapsed;
        //    }
        //}

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            var utente = _utenteService.EffettuaLogin(username, password);

            if (utente != null)
            {
                MainWindow mainWindow = new MainWindow(utente.Ruolo, utente.Username);
                mainWindow.Show();
                Close();
            }
            else
            {
                ErroreTextBlock.Text = "Credenziali errate!";
                ErroreTextBlock.Visibility = Visibility.Visible;
            }
        }


        private void ApriFinestraRegistrazione(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.ShowDialog();
            //VerificaRegistrazione();
        }
    }
}
