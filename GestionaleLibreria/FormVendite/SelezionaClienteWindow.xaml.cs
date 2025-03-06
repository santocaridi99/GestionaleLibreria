using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class SelezionaClienteWindow : Window
    {
        private readonly ClienteService _clienteService;
        public Cliente ClienteSelezionato { get; private set; }

        public SelezionaClienteWindow(ClienteService clienteService)
        {
            InitializeComponent();
            _clienteService = clienteService;
            ClientiDataGrid.ItemsSource = _clienteService.GetAllClienti();
        }

        private void Seleziona_Click(object sender, RoutedEventArgs e)
        {
            if (ClientiDataGrid.SelectedItem is Cliente cliente)
            {
                ClienteSelezionato = cliente;
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Seleziona un cliente prima di continuare.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
