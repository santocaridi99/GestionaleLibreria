using System.Collections.Generic;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class MainWindow : Window
    {
        private readonly LibroService _libroService;

        public MainWindow()
        {
            InitializeComponent();
            ILibroRepository libroRepository = new LibroRepository();
            _libroService = new LibroService(libroRepository);
            CaricaLibri();
        }

        private void CaricaLibri()
        {
            List<Libro> libri = _libroService.GetAllLibri();
            LibriDataGrid.ItemsSource = libri;
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
                _libroService.EliminaLibro(libroSelezionato.Id);
                CaricaLibri();
            }
            else
            {
                MessageBox.Show("Seleziona un libro per eliminarlo.");
            }
        }

        private void GestioneClienti_Click(object sender, RoutedEventArgs e)
        {
            var clientiWindow = new ClientiWindow();
            clientiWindow.ShowDialog();
        }

        private void RegistraVendita_Click(object sender, RoutedEventArgs e)
        {
            var venditaWindow = new VenditaWindow();
            venditaWindow.ShowDialog();
        }

        private void GeneraReport_Click(object sender, RoutedEventArgs e)
        {
            //var reportWindow = new ReportWindow();
            //reportWindow.ShowDialog();
        }

        private void GestioneMagazzino_Click(object sender, RoutedEventArgs e)
        {
            //var magazzinoWindow = new MagazzinoWindow();
            //magazzinoWindow.ShowDialog();
        }
    }
}
