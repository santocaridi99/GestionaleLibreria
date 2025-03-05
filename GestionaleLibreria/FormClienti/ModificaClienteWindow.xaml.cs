using System;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class ModificaClienteWindow : Window
    {
        private readonly ClienteService _clienteService;
        private readonly Cliente _clienteOriginale;

        public ModificaClienteWindow(Cliente cliente, ClienteService clienteService)
        {
            InitializeComponent();
            _clienteService = clienteService;
            _clienteOriginale = cliente;

            if (cliente != null)
            {
                NomeTextBox.Text = cliente.Nome;
                CognomeTextBox.Text = cliente.Cognome;
                EmailTextBox.Text = cliente.Email;
                TelefonoTextBox.Text = cliente.Telefono;
            }
        }

        private void Salva_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NomeTextBox.Text) ||
                string.IsNullOrWhiteSpace(CognomeTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                string.IsNullOrWhiteSpace(TelefonoTextBox.Text))
            {
                MessageBox.Show("Tutti i campi sono obbligatori!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _clienteOriginale.Nome = NomeTextBox.Text;
            _clienteOriginale.Cognome = CognomeTextBox.Text;
            _clienteOriginale.Email = EmailTextBox.Text;
            _clienteOriginale.Telefono = TelefonoTextBox.Text;

            _clienteService.UpdateCliente(_clienteOriginale);
            MessageBox.Show("Cliente modificato con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);

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
