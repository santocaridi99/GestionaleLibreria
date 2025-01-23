using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Models;
using System.Windows;

namespace GestionaleLibreria
{
    public partial class ModificaLibroWindow : Window
    {
        private readonly LibroService _libroService;
        private readonly Libro _libro;

        public ModificaLibroWindow(Libro libro)
        {
            InitializeComponent();
            _libroService = new LibroService();
            _libro = libro;

            // Popola i campi con i dati del libro
            TitoloTextBox.Text = _libro.Titolo;
            AutoreTextBox.Text = _libro.Autore;
            PrezzoTextBox.Text = _libro.Prezzo.ToString();
            QuantitaTextBox.Text = _libro.Quantita.ToString();
        }

        private void Modifica_Click(object sender, RoutedEventArgs e)
        {
            _libro.Titolo = TitoloTextBox.Text;
            _libro.Autore = AutoreTextBox.Text;
            _libro.Prezzo = decimal.Parse(PrezzoTextBox.Text);
            _libro.Quantita = int.Parse(QuantitaTextBox.Text);

            _libroService.ModificaLibro(_libro);
            MessageBox.Show("Libro modificato con successo!");
            Close(); // Chiudi la finestra
        }

        // Metodo per il salvataggio delle modifiche
        private void Salva_Click(object sender, RoutedEventArgs e)
        {
            _libro.Titolo = TitoloTextBox.Text;
            _libro.Autore = AutoreTextBox.Text;
            _libro.Prezzo = decimal.Parse(PrezzoTextBox.Text);
            _libro.Quantita = int.Parse(QuantitaTextBox.Text);

            _libroService.ModificaLibro(_libro);
            MessageBox.Show("Libro modificato con successo!");
            Close(); // Chiudi la finestra
        }

        // Metodo per annullare l'operazione e chiudere la finestra senza salvare
        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Chiudi la finestra senza salvare
        }
    }
}
