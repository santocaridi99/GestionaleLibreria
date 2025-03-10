using System;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Models;
using GestionaleLibreria.Data.Logging;

namespace GestionaleLibreria.WPF
{
    public partial class RimuoviStockWindow : Window
    {
        private readonly MagazzinoService _magazzinoService;
        private readonly LibroMagazzino _libroMagazzino;
        private static readonly string NomeClasse = nameof(RimuoviStockWindow);

        public RimuoviStockWindow(MagazzinoService magazzinoService, LibroMagazzino libroMagazzino)
        {
            string nomeMetodo = nameof(RimuoviStockWindow);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Inizializzazione finestra Rimuovi Stock.");
                InitializeComponent();
                _magazzinoService = magazzinoService;
                _libroMagazzino = libroMagazzino;
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante l'apertura della finestra.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RimuoviButton_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(RimuoviButton_Click);
            try
            {
                if (int.TryParse(QuantitaTextBox.Text, out int quantita) && quantita > 0)
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Tentativo di rimozione di {quantita} copie di \"{_libroMagazzino.Libro.Titolo}\".");

                    bool success = _magazzinoService.RimuoviScorte(_libroMagazzino.LibroId, quantita);
                    if (success)
                    {
                        Logger.LogInfo(NomeClasse, nomeMetodo, $"Rimosse {quantita} copie di \"{_libroMagazzino.Libro.Titolo}\" con successo.");
                        MessageBox.Show($"Rimosse {quantita} copie di \"{_libroMagazzino.Libro.Titolo}\"", "Successo");
                        this.Close();
                    }
                    else
                    {
                        Logger.LogInfo(NomeClasse, nomeMetodo, "Tentativo di rimozione fallito: quantità insufficiente nel magazzino.");
                        MessageBox.Show("Quantità insufficiente nel magazzino.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, "Input quantità non valido.");
                    MessageBox.Show("Inserisci un numero valido.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante la rimozione delle scorte.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AnnullaButton_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(AnnullaButton_Click);
            Logger.LogInfo(NomeClasse, nomeMetodo, "Chiusura finestra senza modifiche.");
            this.Close();
        }
    }
}
