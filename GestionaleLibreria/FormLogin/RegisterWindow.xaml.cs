using System;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Logging;

namespace GestionaleLibreria.WPF
{
    public partial class RegisterWindow : Window
    {
        private readonly UtenteService _utenteService;
        private static readonly string NomeClasse = nameof(RegisterWindow);

        public RegisterWindow()
        {
            string nomeMetodo = nameof(RegisterWindow);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Avvio finestra di registrazione.");
                InitializeComponent();
                _utenteService = new UtenteService(new UtenteRepository(new LibraryContext()));
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante l'inizializzazione della finestra di registrazione.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Registra_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(Registra_Click);
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Tentativo di registrazione fallito: campi vuoti.");
                ErroreTextBlock.Text = "Compila tutti i campi!";
                ErroreTextBlock.Visibility = Visibility.Visible;
                return;
            }

            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Tentativo di registrazione per l'utente: {username}.");

                _utenteService.RegistraUtente(username, password, "Operatore");

                Logger.LogInfo(NomeClasse, nomeMetodo, $"Registrazione riuscita per l'utente: {username}.");
                MessageBox.Show("Registrazione completata!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                ErroreTextBlock.Text = "Errore nella registrazione. " + ex.Message;
                ErroreTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(Annulla_Click);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Chiusura finestra di registrazione.");
                Close();
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
            }
        }
    }
}
