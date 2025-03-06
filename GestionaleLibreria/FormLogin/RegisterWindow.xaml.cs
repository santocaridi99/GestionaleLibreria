using System;
using System.Windows;
using GestionaleLibreria.Business;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;

namespace GestionaleLibreria.WPF
{
    public partial class RegisterWindow : Window
    {
        private readonly UtenteService _utenteService;

        public RegisterWindow()
        {
            InitializeComponent();
            _utenteService = new UtenteService(new UtenteRepository(new LibraryContext()));
        }

        private void Registra_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ErroreTextBlock.Text = "Compila tutti i campi!";
                ErroreTextBlock.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                _utenteService.RegistraUtente(username, password, "Operatore");
                MessageBox.Show("Registrazione completata!", "Successo");
                Close();
            }
            catch (Exception ex)
            {
                ErroreTextBlock.Text = ex.Message;
                ErroreTextBlock.Visibility = Visibility.Visible;
            }
        }


        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
