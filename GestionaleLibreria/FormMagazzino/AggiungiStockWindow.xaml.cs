using System;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Logging;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class AggiungiStockWindow : Window
    {
        private readonly MagazzinoService _magazzinoService;
        private readonly LibroMagazzino _libroMagazzino;
        private static readonly string NomeClasse = nameof(AggiungiStockWindow);

        public AggiungiStockWindow(MagazzinoService magazzinoService, LibroMagazzino libroMagazzino)
        {
            string nomeMetodo = nameof(AggiungiStockWindow);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Apertura finestra di aggiunta stock.");
                InitializeComponent();
                _magazzinoService = magazzinoService;
                _libroMagazzino = libroMagazzino;
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore nell'inizializzazione della finestra di aggiunta stock.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AggiungiButton_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(AggiungiButton_Click);

            if (!int.TryParse(QuantitaTextBox.Text, out int quantita) || quantita <= 0)
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Tentativo di aggiunta stock fallito: quantità non valida.");
                MessageBox.Show("Inserisci un numero valido.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Aggiunta di {quantita} copie a \"{_libroMagazzino.Libro.Titolo}\" (ID: {_libroMagazzino.LibroId}).");
                _magazzinoService.AggiungiScorte(_libroMagazzino.LibroId, quantita);

                Logger.LogInfo(NomeClasse, nomeMetodo, $"Stock aggiornato con successo per \"{_libroMagazzino.Libro.Titolo}\".");
                MessageBox.Show($"Aggiunte {quantita} copie di \"{_libroMagazzino.Libro.Titolo}\"", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante l'aggiornamento dello stock.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AnnullaButton_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(AnnullaButton_Click);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Chiusura finestra di aggiunta stock senza modifiche.");
                Close();
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
            }
        }
    }
}
