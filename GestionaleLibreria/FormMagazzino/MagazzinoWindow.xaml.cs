﻿using System.Linq;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class MagazzinoWindow : Window
    {
        private readonly MagazzinoService _magazzinoService;

        public MagazzinoWindow()
        {
            InitializeComponent();

            var context = new LibraryContext();
            var magazzinoRepository = new MagazzinoRepository(context);
            var libroRepository = new LibroRepository();
            _magazzinoService = new MagazzinoService(magazzinoRepository, libroRepository);

            CaricaLibri();
        }


        private void CaricaLibri()
        {
            var libriMagazzino = _magazzinoService.GetLibriMagazzino();
            MagazzinoDataGrid.ItemsSource = libriMagazzino;
        }

        private void FiltraLibri_Click(object sender, RoutedEventArgs e)
        {
            string filtro = FiltroLibroTextBox.Text.ToLower();
            var risultati = _magazzinoService.GetLibriMagazzino()
                .Where(lm => lm.Libro.Titolo.ToLower().Contains(filtro)
                          || lm.Libro.ISBN.ToLower().Contains(filtro))
                .ToList();

            MagazzinoDataGrid.ItemsSource = risultati;
        }

        private void AggiungiStock_Click(object sender, RoutedEventArgs e)
        {
            if (MagazzinoDataGrid.SelectedItem is LibroMagazzino libroMagazzinoSelezionato)
            {
                var aggiungiStockWindow = new AggiungiStockWindow(_magazzinoService, libroMagazzinoSelezionato);
                aggiungiStockWindow.ShowDialog();
                CaricaLibri();
            }
            else
            {
                MessageBox.Show("Seleziona un libro per aggiungere quantità.");
            }
        }


        private void RimuoviStock_Click(object sender, RoutedEventArgs e)
        {
            if (MagazzinoDataGrid.SelectedItem is LibroMagazzino libroMagazzinoSelezionato)
            {
                var rimuoviStockWindow = new RimuoviStockWindow(_magazzinoService, libroMagazzinoSelezionato);
                rimuoviStockWindow.ShowDialog();
                CaricaLibri();
            }
            else
            {
                MessageBox.Show("Seleziona un libro per rimuovere quantità.");
            }
        }
    }
}
