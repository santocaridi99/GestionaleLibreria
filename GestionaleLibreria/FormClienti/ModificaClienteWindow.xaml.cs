using System;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Logging;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class ModificaClienteWindow : Window
    {
        private readonly ClienteService _clienteService;
        private readonly Cliente _clienteOriginale;
        private static readonly string NomeClasse = nameof(ModificaClienteWindow);

        public ModificaClienteWindow(Cliente cliente, ClienteService clienteService)
        {
            string nomeMetodo = nameof(ModificaClienteWindow);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Apertura finestra modifica cliente.");
                InitializeComponent();

                _clienteService = clienteService;
                _clienteOriginale = cliente;

                if (cliente != null)
                {
                    NomeTextBox.Text = cliente.Nome;
                    CognomeTextBox.Text = cliente.Cognome;
                    EmailTextBox.Text = cliente.Email;
                    TelefonoTextBox.Text = cliente.Telefono;
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Dati caricati per il cliente: {cliente.Nome} {cliente.Cognome}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante il caricamento del cliente.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void Salva_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(Salva_Click);
            try
            {
                if (string.IsNullOrWhiteSpace(NomeTextBox.Text) ||
                    string.IsNullOrWhiteSpace(CognomeTextBox.Text) ||
                    string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                    string.IsNullOrWhiteSpace(TelefonoTextBox.Text))
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, "Tentativo di salvataggio con campi vuoti.");
                    MessageBox.Show("Tutti i campi sono obbligatori!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _clienteOriginale.Nome = NomeTextBox.Text;
                _clienteOriginale.Cognome = CognomeTextBox.Text;
                _clienteOriginale.Email = EmailTextBox.Text;
                _clienteOriginale.Telefono = TelefonoTextBox.Text;

                Logger.LogInfo(NomeClasse, nomeMetodo, $"Salvataggio modifiche cliente: {_clienteOriginale.Nome} {_clienteOriginale.Cognome}");

                _clienteService.UpdateCliente(_clienteOriginale);

                MessageBox.Show("Cliente modificato con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
                Logger.LogInfo(NomeClasse, nomeMetodo, "Cliente modificato con successo!");

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante la modifica del cliente.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(Annulla_Click);
            Logger.LogInfo(NomeClasse, nomeMetodo, "Modifica cliente annullata.");
            DialogResult = false;
            Close();
        }
    }
}
