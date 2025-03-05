using System;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class AggiungiClienteWindow : Window
    {
        private readonly ClienteService _clienteService;

        public AggiungiClienteWindow(ClienteService clienteService)
        {
            InitializeComponent();
            _clienteService = clienteService;
        }

        private void AggiungiButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NomeTextBox.Text) ||
                string.IsNullOrWhiteSpace(CognomeTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(TelefonoTextBox.Text))
            {
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
            MessageBox.Show("Cliente aggiunto con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);

            DialogResult = true;
            Close();
        }

        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
