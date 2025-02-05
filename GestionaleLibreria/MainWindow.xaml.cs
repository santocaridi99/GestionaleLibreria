using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using System.Collections.Generic;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class MainWindow : Window
    {
        // Iniettiamo il servizio tramite l'interfaccia del repository (DIP)
        private readonly LibroService _libroService;

        public MainWindow()
        {
            InitializeComponent();
            // Creiamo l'istanza del repository e la passiamo al servizio
            ILibroRepository libroRepository = new LibroRepository();
            _libroService = new LibroService(libroRepository);
            CaricaLibri();
        }

        private void CaricaLibri()
        {
            // Recupera la lista dei libri
            List<Libro> libri = _libroService.GetAllLibri();
            LibriDataGrid.ItemsSource = libri;
        }

        private void AggiungiLibro_Click(object sender, RoutedEventArgs e)
        {
            // Mostra la finestra per aggiungere un nuovo libro
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
                _libroService.EliminaLibro(libroSelezionato.Id);
                CaricaLibri();
            }
            else
            {
                MessageBox.Show("Seleziona un libro per eliminarlo.");
            }
        }
    }
}
