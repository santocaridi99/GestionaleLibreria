using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Models;
using System.Collections.Generic;
using System.Windows;

namespace GestionaleLibreria
{
    public partial class MainWindow : Window
    {
        private readonly LibroService _libroService;

        public MainWindow()
        {
            InitializeComponent();
            _libroService = new LibroService();
            CaricaLibri();
        }

        private void CaricaLibri()
        {
            var libri = _libroService.GetAllLibri();
            LibriDataGrid.ItemsSource = libri;
        }

        private void AggiungiLibro_Click(object sender, RoutedEventArgs e)
        {
            // Mostra una finestra di dialogo per aggiungere un nuovo libro
            var aggiungiFinestra = new AggiungiLibroWindow();
            aggiungiFinestra.ShowDialog();
            CaricaLibri();  // Ricarica i libri dopo aver aggiunto uno nuovo
        }

        private void ModificaLibro_Click(object sender, RoutedEventArgs e)
        {
            if (LibriDataGrid.SelectedItem is Libro libroSelezionato)
            {
                var modificaFinestra = new ModificaLibroWindow(libroSelezionato);
                modificaFinestra.ShowDialog();
                CaricaLibri();  // Ricarica i libri dopo la modifica
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
                CaricaLibri();  // Ricarica i libri dopo aver eliminato uno
            }
            else
            {
                MessageBox.Show("Seleziona un libro per eliminarlo.");
            }
        }
    }
}
