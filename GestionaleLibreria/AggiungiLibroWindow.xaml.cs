using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using System;
using System.Windows;

namespace GestionaleLibreria.WPF
{
    public partial class AggiungiLibroWindow : Window
    {
        private readonly LibroService _libroService;
        // Se vuoi gestire le scorte, potresti avere anche un'istanza di MagazzinoService
        // private readonly MagazzinoService _magazzinoService;

        public AggiungiLibroWindow()
        {
            InitializeComponent();
            // Iniezione manuale delle dipendenze
            ILibroRepository libroRepository = new LibroRepository();
            _libroService = new LibroService(libroRepository);

            // Se hai un magazzino e un magazzino service:
            // var magazzino = new Magazzino();
            // _magazzinoService = new MagazzinoService(magazzino);
        }

        private void AggiungiButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var nuovoLibro = new Libro
                {
                    Titolo = TitoloTextBox.Text,
                    Autore = AutoreTextBox.Text,
                    ISBN = ISBNTextBox.Text,
                    Prezzo = decimal.Parse(PrezzoTextBox.Text)
                    // Nota: La quantità per il magazzino verrà gestita tramite MagazzinoService
                };

                int quantita = int.Parse(QuantitaTextBox.Text);

                // Aggiunge il libro nel repository
                _libroService.AggiungiLibro(nuovoLibro);

                // Se vuoi aggiornare anche le scorte nel magazzino, potresti fare:
                // _magazzinoService.AggiungiLibroFisico(nuovoLibro, quantita);

                MessageBox.Show("Libro aggiunto con successo!");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante l'aggiunta del libro: " + ex.Message);
            }
        }

        private void AnnullaAggiungi_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
