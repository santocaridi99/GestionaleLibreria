using System;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class AggiungiStockWindow : Window
    {
        private readonly MagazzinoService _magazzinoService;
        private readonly LibroMagazzino _libroMagazzino;

        public AggiungiStockWindow(MagazzinoService magazzinoService, LibroMagazzino libroMagazzino)
        {
            InitializeComponent();
            _magazzinoService = magazzinoService;
            _libroMagazzino = libroMagazzino;
        }

        private void AggiungiButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QuantitaTextBox.Text, out int quantita) && quantita > 0)
            {
                _magazzinoService.AggiungiScorte(_libroMagazzino.LibroId, quantita);
                MessageBox.Show($"Aggiunte {quantita} copie di \"{_libroMagazzino.Libro.Titolo}\"", "Successo");
                this.Close();
            }
            else
            {
                MessageBox.Show("Inserisci un numero valido.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AnnullaButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
