using System;
using System.Collections.Generic;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class ClientiWindow : Window
    {
        private readonly ClienteService _clienteService;
        private List<Cliente> _clienti;

        public ClientiWindow()
        {
            InitializeComponent();
            IClienteRepository clienteRepository = new ClienteRepository();
            _clienteService = new ClienteService(clienteRepository);
            CaricaClienti();
        }

        private void CaricaClienti()
        {
            _clienti = _clienteService.GetAllClienti();
            ClientiDataGrid.ItemsSource = _clienti;
        }

        private void AggiungiCliente_Click(object sender, RoutedEventArgs e)
        {
            var finestraAggiungi = new AggiungiClienteWindow(_clienteService);
            if (finestraAggiungi.ShowDialog() == true)
            {
                CaricaClienti(); // Ricarichiamo la lista dopo l'aggiunta
            }
        }

        private void ModificaCliente_Click(object sender, RoutedEventArgs e)
        {
            if (ClientiDataGrid.SelectedItem is Cliente clienteSelezionato)
            {
                var finestraModifica = new ModificaClienteWindow(clienteSelezionato, _clienteService);
                if (finestraModifica.ShowDialog() == true)
                {
                    CaricaClienti(); // Ricarichiamo la lista dopo la modifica
                }
            }
            else
            {
                MessageBox.Show("Seleziona un cliente da modificare.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void EliminaCliente_Click(object sender, RoutedEventArgs e)
        {
            if (ClientiDataGrid.SelectedItem is Cliente clienteSelezionato)
            {
                var result = MessageBox.Show($"Sei sicuro di voler eliminare {clienteSelezionato.Nome} {clienteSelezionato.Cognome}?",
                                             "Conferma Eliminazione",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _clienteService.DeleteCliente(clienteSelezionato.Id);
                    MessageBox.Show("Cliente eliminato con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
                    CaricaClienti(); // Aggiorniamo la lista
                }
            }
            else
            {
                MessageBox.Show("Seleziona un cliente da eliminare.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
