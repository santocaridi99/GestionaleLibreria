using System;
using System.Windows;
using GestionaleLibreria.Business;
using System.Windows.Controls;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class RegisterWindow : Window
    {
        private readonly UtenteService _utenteService;

        public RegisterWindow()
        {
            InitializeComponent();
            IUtenteRepository utenteRepository = new UtenteRepository(new LibraryContext());
            _utenteService = new UtenteService(utenteRepository);
        }

        private void Registra_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string ruolo = (RuoloComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(ruolo))
            {
                ErroreTextBlock.Text = "Compila tutti i campi!";
                ErroreTextBlock.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                _utenteService.RegistraUtente(username, password, ruolo);
                MessageBox.Show("Registrazione completata!", "Successo");
                this.Close();
            }
            catch (Exception ex)
            {
                ErroreTextBlock.Text = ex.Message;
                ErroreTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}
