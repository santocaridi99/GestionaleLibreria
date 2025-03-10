using System;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Logging;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class AggiungiClienteWindow : Window
    {
        private readonly ClienteService _clienteService;
        private static readonly string NomeClasse = nameof(AggiungiClienteWindow);

        public AggiungiClienteWindow(ClienteService clienteService)
        {
            string nomeMetodo = nameof(AggiungiClienteWindow);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Apertura finestra aggiunta cliente.");
                InitializeComponent();
                _clienteService = clienteService;
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante il caricamento della finestra.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void AggiungiButton_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(AggiungiButton_Click);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Avvio processo di aggiunta cliente.");

                if (string.IsNullOrWhiteSpace(NomeTextBox.Text) ||
                    string.IsNullOrWhiteSpace(CognomeTextBox.Text) ||
                    string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                    string.IsNullOrWhiteSpace(TelefonoTextBox.Text))
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, "Tentativo di aggiunta fallito: campi obbligatori vuoti.");
                    MessageBox.Show("Tutti i campi sono obbligatori!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Cliente nuovoCliente = new Cliente
                {
                    Nome = NomeTextBox.Text,
                    Cognome = CognomeTextBox.Text,
                    Email = EmailTextBox.Text,
                    Telefono = TelefonoTextBox.Text
                };

                _clienteService.AddCliente(nuovoCliente);
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Cliente aggiunto con successo");

                MessageBox.Show("Cliente aggiunto con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante l'aggiunta del cliente.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(Annulla_Click);
            Logger.LogInfo(NomeClasse, nomeMetodo, "Aggiunta cliente annullata dall'utente.");
            DialogResult = false;
            Close();
        }
    }
}
