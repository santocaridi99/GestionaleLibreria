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

        public ClientiWindow()
        {
            InitializeComponent();
            // Iniezione: crea l'istanza del repository per i clienti
            IClienteRepository clienteRepository = new ClienteRepository();
            _clienteService = new ClienteService(clienteRepository);
            CaricaClienti();
        }

        private void CaricaClienti()
        {
            List<Cliente> clienti = _clienteService.GetAllClienti();
            ClientiDataGrid.ItemsSource = clienti;
        }

        private void AggiungiCliente_Click(object sender, RoutedEventArgs e)
        {
            //// Apri una finestra per aggiungere un nuovo cliente (da implementare)
            //var aggiungiClienteWindow = new AggiungiClienteWindow();
            //aggiungiClienteWindow.ShowDialog();
            //CaricaClienti();
        }

        private void ModificaCliente_Click(object sender, RoutedEventArgs e)
        {
            //if (ClientiDataGrid.SelectedItem is Cliente clienteSelezionato)
            //{
            //    var modificaClienteWindow = new ModificaClienteWindow(clienteSelezionato);
            //    modificaClienteWindow.ShowDialog();
            //    CaricaClienti();
            //}
            //else
            //{
            //    MessageBox.Show("Seleziona un cliente da modificare.");
            //}
        }
    }
}
