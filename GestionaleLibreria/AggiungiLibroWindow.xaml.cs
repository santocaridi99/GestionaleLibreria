using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Models;
using System.Windows;

namespace GestionaleLibreria
{
    public partial class AggiungiLibroWindow : Window
    {
        private readonly LibroService _libroService;

        public AggiungiLibroWindow()
        {
            InitializeComponent();
            _libroService = new LibroService();
        }

        //"Aggiungi Libro"
        private void AggiungiButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                var nuovoLibro = new Libro
                {
                    Titolo = TitoloTextBox.Text,
                    Autore = AutoreTextBox.Text,
                    Prezzo = decimal.Parse(PrezzoTextBox.Text),
                    Quantita = int.Parse(QuantitaTextBox.Text)
                };

               
                _libroService.AggiungiLibro(nuovoLibro);

                // Mostra un messaggio di conferma
                MessageBox.Show("Libro aggiunto con successo!");

                // Chiudi la finestra
                DialogResult = true;
                Close();
            }
            catch (System.Exception ex)
            {
                // Mostra un messaggio di errore in caso di problemi
                MessageBox.Show("Errore durante l'aggiunta del libro: " + ex.Message);
            }
        }

        // Metodo per il pulsante "Annulla"
        private void AnnullaAggiungi_Click(object sender, RoutedEventArgs e)
        {
            // Chiudi la finestra senza fare nulla
            Close();
        }
    }
}
