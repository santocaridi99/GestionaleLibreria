using System;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Models;
using GestionaleLibreria.Data.Logging;

namespace GestionaleLibreria.WPF
{
    public partial class SelezionaClienteWindow : Window
    {
        private readonly ClienteService _clienteService;
        public Cliente ClienteSelezionato { get; private set; }
        private static readonly string NomeClasse = nameof(SelezionaClienteWindow);

        public SelezionaClienteWindow(ClienteService clienteService)
        {
            InitializeComponent();
            _clienteService = clienteService;
            try
            {
                ClientiDataGrid.ItemsSource = _clienteService.GetAllClienti();
                Logger.LogInfo(NomeClasse, nameof(SelezionaClienteWindow), "Caricati i clienti nella finestra di selezione.");
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nameof(SelezionaClienteWindow), ex);
                MessageBox.Show("Errore nel caricamento dei clienti.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Seleziona_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(Seleziona_Click);
            try
            {
                if (ClientiDataGrid.SelectedItem is Cliente cliente)
                {
                    ClienteSelezionato = cliente;
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Cliente selezionato: {cliente.Nome} {cliente.Cognome}");
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Seleziona un cliente prima di continuare.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore nella selezione del cliente.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
