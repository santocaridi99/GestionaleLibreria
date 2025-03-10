using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;


namespace GestionaleLibreria.WPF
{
    public partial class LibriWindow : Window
    {
        private readonly LibroService _libroService;
        private List<Libro> _tuttiLibri;

        public LibriWindow()
        {
            InitializeComponent();
            var context = new LibraryContext();

            ILibroRepository libroRepository = new LibroRepository();
            IMagazzinoRepository magazzinoRepository = new MagazzinoRepository(context);

            MagazzinoService magazzinoService = new MagazzinoService(magazzinoRepository, libroRepository); 
            _libroService = new LibroService(libroRepository, magazzinoRepository);

            CaricaLibri();
        }
        private void CaricaLibri()
        {
            _tuttiLibri = _libroService.GetAllLibri();

            foreach (var libro in _tuttiLibri)
            {
                libro.Prezzo = libro.CalcolaPrezzo();
            }

            AggiornaDataGrid(_tuttiLibri);
        }


        private void AggiornaDataGrid(List<Libro> libri)
        {
            LibriDataGrid.ItemsSource = libri; 
        }

        private void FiltraLibri_Click(object sender, RoutedEventArgs e)
        {
            string filtro = FiltroTextBox.Text.Trim();
            string criterio = ((ComboBoxItem)FiltroCriterioComboBox.SelectedItem)?.Content.ToString();

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
        }




        private void AggiungiLibro_Click(object sender, RoutedEventArgs e)
        {
            var aggiungiFinestra = new AggiungiLibroWindow();
            aggiungiFinestra.ShowDialog();
            CaricaLibri();
        }

        private void ModificaLibro_Click(object sender, RoutedEventArgs e)
        {
            if (LibriDataGrid.SelectedItem is Libro libroSelezionato) 
            {
                var modificaFinestra = new ModificaLibroWindow(libroSelezionato);
                modificaFinestra.ShowDialog();
                CaricaLibri();
            }
            else
            {
                MessageBox.Show("Seleziona un libro per modificarlo.");
            }
        }


        private void EliminaLibro_Click(object sender, RoutedEventArgs e)
        {
            if (LibriDataGrid.SelectedItem is Libro libroSelezionato)
            {
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
                }
            }
            else
            {
                MessageBox.Show("Seleziona un libro per eliminarlo.");
            }
        }
    }
}
