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
            _libroService = new LibroService(libroRepository, magazzinoService);

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
            string filtro = FiltroTextBox.Text.ToLower();
            string tipoFiltro = ((ComboBoxItem)FiltroTipoComboBox.SelectedItem).Content.ToString();

            var libriFiltrati = _tuttiLibri.Where(libro =>
                (string.IsNullOrEmpty(filtro) ||
                 libro.Titolo.ToLower().Contains(filtro) ||
                 libro.Autore.ToLower().Contains(filtro) ||
                 libro.ISBN.ToLower().Contains(filtro) ||
                 libro.CasaEditrice.ToLower().Contains(filtro)) &&
                (tipoFiltro == "Tutti" ||
                 (tipoFiltro == "Ebook" && libro is Ebook) ||
                 (tipoFiltro == "Audiobook" && libro is Audiobook))
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
