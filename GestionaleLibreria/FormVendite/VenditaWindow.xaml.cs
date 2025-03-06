using System;
using System.Collections.Generic;
using System.Windows;
using GestionaleLibreria.Business;
using System.Windows.Controls;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class VenditaWindow : Window
    {
        private readonly LibroService _libroService;
        private readonly ClienteService _clienteService;
        private readonly VenditaService _venditaService;

        private Cliente _clienteSelezionato;
        private Libro _libroSelezionato;

        public VenditaWindow()
        {
            InitializeComponent();
         
        }

        private void CercaLibro_Click(object sender, RoutedEventArgs e)
        {
            //string ricerca = RicercaLibroTextBox.Text.Trim();
            //if (!string.IsNullOrEmpty(ricerca))
            //{
            //    LibriVenditaDataGrid.ItemsSource = _libroService.CercaLibri(ricerca);
            //}
            //else
            //{
            //    MessageBox.Show("Inserisci un titolo per la ricerca.", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
            //}
        }

        private void SelezionaLibro_Click(object sender, RoutedEventArgs e)
        {
            if (LibriVenditaDataGrid.SelectedItem is Libro libro)
            {
                _libroSelezionato = libro;
                MessageBox.Show($"Libro selezionato: {libro.Titolo}", "Libro Selezionato", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Seleziona un libro prima di procedere.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void SelezionaCliente_Click(object sender, RoutedEventArgs e)
        {
            var finestraClienti = new SelezionaClienteWindow(_clienteService);
            if (finestraClienti.ShowDialog() == true)
            {
                _clienteSelezionato = finestraClienti.ClienteSelezionato;
                ClienteTextBox.Text = $"{_clienteSelezionato.Nome} {_clienteSelezionato.Cognome}";
            }
        }

        private void RegistraVendita_Click(object sender, RoutedEventArgs e)
        {
            if (_libroSelezionato == null)
            {
                MessageBox.Show("Seleziona un libro prima di registrare la vendita.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string metodoPagamento = (MetodoPagamentoComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (string.IsNullOrEmpty(metodoPagamento))
            {
                MessageBox.Show("Seleziona un metodo di pagamento.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var nuovaVendita = new Vendita
            {
                LibroId = _libroSelezionato.Id,
                ClienteId = _clienteSelezionato.Id,
                DataVendita = DateTime.Now,
                MetodoPagamento = metodoPagamento
            };

            _venditaService.RegistraVendita(nuovaVendita);
            MessageBox.Show("Vendita registrata con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }

        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
