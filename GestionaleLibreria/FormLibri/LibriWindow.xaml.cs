using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Logging;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class LibriWindow : Window
    {
        private readonly LibroService _libroService;
        private List<Libro> _tuttiLibri;
        private static readonly string NomeClasse = nameof(LibriWindow);

        public LibriWindow()
        {
            string nomeMetodo = nameof(LibriWindow);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Inizializzazione finestra di gestione libri.");

                InitializeComponent();
                var context = new LibraryContext();

                ILibroRepository libroRepository = new LibroRepository();
                IMagazzinoRepository magazzinoRepository = new MagazzinoRepository(context);

                MagazzinoService magazzinoService = new MagazzinoService(magazzinoRepository, libroRepository);
                _libroService = new LibroService(libroRepository, magazzinoRepository);

                CaricaLibri();

                Logger.LogInfo(NomeClasse, nomeMetodo, "Finestra inizializzata correttamente.");
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        private void CaricaLibri()
        {
            string nomeMetodo = nameof(CaricaLibri);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Caricamento libri in corso.");

                _tuttiLibri = _libroService.GetAllLibri();
                AggiornaDataGrid(_tuttiLibri);

                Logger.LogInfo(NomeClasse, nomeMetodo, $"Caricati {_tuttiLibri.Count} libri.");
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
            }
        }

        private void AggiornaDataGrid(List<Libro> libri)
        {
            LibriDataGrid.ItemsSource = libri;
        }

        private void FiltraLibri_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(FiltraLibri_Click);
            try
            {
                string filtro = FiltroTextBox.Text.Trim();
                string criterio = ((ComboBoxItem)FiltroCriterioComboBox.SelectedItem)?.Content.ToString();

                Logger.LogInfo(NomeClasse, nomeMetodo, $"Ricerca avviata con filtro: {filtro}, criterio: {criterio}");

                // Se il filtro è vuoto, mostra tutti i libri
                if (string.IsNullOrEmpty(filtro))
                {
                    AggiornaDataGrid(_tuttiLibri);
                    return;
                }

                var libriFiltrati = _tuttiLibri.Where(libro =>
                    (criterio == "Titolo" && libro.Titolo.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (criterio == "Autore" && libro.Autore.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (criterio == "Casa Editrice" && libro.CasaEditrice.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (criterio == "ISBN" && libro.ISBN == filtro) // ISBN deve essere univoco
                ).ToList();

                AggiornaDataGrid(libriFiltrati);

                Logger.LogInfo(NomeClasse, nomeMetodo, $"Trovati {libriFiltrati.Count} libri corrispondenti.");
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
            }
        }

        private void AggiungiLibro_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(AggiungiLibro_Click);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Apertura finestra per aggiungere un nuovo libro.");

                var aggiungiFinestra = new AggiungiLibroWindow();
                aggiungiFinestra.ShowDialog();

                CaricaLibri();
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
            }
        }

        private void ModificaLibro_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(ModificaLibro_Click);
            try
            {
                if (LibriDataGrid.SelectedItem is Libro libroSelezionato)
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Modifica libro: {libroSelezionato.Titolo} (ISBN: {libroSelezionato.ISBN})");

                    var modificaFinestra = new ModificaLibroWindow(libroSelezionato);
                    modificaFinestra.ShowDialog();

                    CaricaLibri();
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, "Nessun libro selezionato per la modifica.");
                    MessageBox.Show("Seleziona un libro per modificarlo.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
            }
        }

        private void EliminaLibro_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(EliminaLibro_Click);
            try
            {
                if (LibriDataGrid.SelectedItem is Libro libroSelezionato)
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Richiesta di eliminazione per il libro: {libroSelezionato.Titolo} (ISBN: {libroSelezionato.ISBN})");

                    MessageBoxResult result = MessageBox.Show(
                        $"Sei sicuro di voler eliminare \"{libroSelezionato.Titolo}\"?",
                        "Conferma Eliminazione",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning
                    );

                    if (result == MessageBoxResult.Yes)
                    {
                        _libroService.EliminaLibro(libroSelezionato.Id);
                        CaricaLibri();
                        Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro eliminato con successo: {libroSelezionato.Titolo}");
                    }
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, "Nessun libro selezionato per l'eliminazione.");
                    MessageBox.Show("Seleziona un libro per eliminarlo.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
            }
        }
    }
}
