using System;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class RimuoviStockWindow : Window
    {
        private readonly MagazzinoService _magazzinoService;
        private readonly LibroMagazzino _libroMagazzino;

        public RimuoviStockWindow(MagazzinoService magazzinoService, LibroMagazzino libroMagazzino)
        {
            InitializeComponent();
            _magazzinoService = magazzinoService;
            _libroMagazzino = libroMagazzino;
        }

        private void RimuoviButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(QuantitaTextBox.Text, out int quantita) && quantita > 0)
            {
                bool success = _magazzinoService.RimuoviScorte(_libroMagazzino.LibroId, quantita);
                if (success)
                {
                    MessageBox.Show($"Rimosse {quantita} copie di \"{_libroMagazzino.Libro.Titolo}\"", "Successo");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Quantità insufficiente nel magazzino.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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
