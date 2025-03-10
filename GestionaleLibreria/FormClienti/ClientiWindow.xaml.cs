using System;
using System.Collections.Generic;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Logging;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class ClientiWindow : Window
    {
        private readonly ClienteService _clienteService;
        private List<Cliente> _clienti;
        private static readonly string NomeClasse = nameof(ClientiWindow);

        public ClientiWindow()
        {
            string nomeMetodo = nameof(ClientiWindow);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Apertura finestra gestione clienti.");
                InitializeComponent();

                IClienteRepository clienteRepository = new ClienteRepository();
                _clienteService = new ClienteService(clienteRepository);

                CaricaClienti();
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante il caricamento della finestra clienti.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void CaricaClienti()
        {
            string nomeMetodo = nameof(CaricaClienti);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Caricamento lista clienti.");
                _clienti = _clienteService.GetAllClienti();
                ClientiDataGrid.ItemsSource = _clienti;
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Caricati {_clienti.Count} clienti.");
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante il caricamento dei clienti.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AggiungiCliente_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(AggiungiCliente_Click);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Apertura finestra aggiunta cliente.");
                var finestraAggiungi = new AggiungiClienteWindow(_clienteService);

                if (finestraAggiungi.ShowDialog() == true)
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, "Cliente aggiunto con successo.");
                    CaricaClienti();
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, "Aggiunta cliente annullata.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante l'aggiunta del cliente.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ModificaCliente_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(ModificaCliente_Click);
            try
            {
                if (ClientiDataGrid.SelectedItem is Cliente clienteSelezionato)
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Apertura finestra modifica cliente: {clienteSelezionato.Nome} {clienteSelezionato.Cognome}.");
                    var finestraModifica = new ModificaClienteWindow(clienteSelezionato, _clienteService);

                    if (finestraModifica.ShowDialog() == true)
                    {
                        Logger.LogInfo(NomeClasse, nomeMetodo, $"Cliente modificato con successo: {clienteSelezionato.Nome} {clienteSelezionato.Cognome}.");
                        CaricaClienti();
                    }
                    else
                    {
                        Logger.LogInfo(NomeClasse, nomeMetodo, "Modifica cliente annullata.");
                    }
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, "Tentativo di modifica senza selezionare un cliente.");
                    MessageBox.Show("Seleziona un cliente da modificare.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante la modifica del cliente.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EliminaCliente_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(EliminaCliente_Click);
            try
            {
                if (ClientiDataGrid.SelectedItem is Cliente clienteSelezionato)
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Tentativo di eliminazione cliente: {clienteSelezionato.Nome} {clienteSelezionato.Cognome}.");

                    var result = MessageBox.Show($"Sei sicuro di voler eliminare {clienteSelezionato.Nome} {clienteSelezionato.Cognome}?",
                                                 "Conferma Eliminazione",
                                                 MessageBoxButton.YesNo,
                                                 MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        _clienteService.DeleteCliente(clienteSelezionato.Id);
                        Logger.LogInfo(NomeClasse, nomeMetodo, $"Cliente eliminato: {clienteSelezionato.Nome} {clienteSelezionato.Cognome}.");
                        MessageBox.Show("Cliente eliminato con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
                        CaricaClienti();
                    }
                    else
                    {
                        Logger.LogInfo(NomeClasse, nomeMetodo, "Eliminazione cliente annullata.");
                    }
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, "Tentativo di eliminazione senza selezionare un cliente.");
                    MessageBox.Show("Seleziona un cliente da eliminare.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante l'eliminazione del cliente.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
