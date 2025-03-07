using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GestionaleLibreria.WPF
{
    public partial class AggiungiLibroWindow : Window
    {
        private readonly LibroService _libroService;

        public AggiungiLibroWindow()
        {
            InitializeComponent();
            var context = new LibraryContext();

            ILibroRepository libroRepository = new LibroRepository();
            IMagazzinoRepository magazzinoRepository = new MagazzinoRepository(context);

            MagazzinoService magazzinoService = new MagazzinoService(magazzinoRepository, libroRepository); 
            _libroService = new LibroService(libroRepository, magazzinoRepository);
        }


        private void TipoLibroComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Se la finestra non è ancora inizializzata, esci
            if (!IsLoaded) return;

            if (TipoLibroComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string tipoSelezionato = selectedItem.Content.ToString();

                // Controlla che i campi non siano null prima di modificarne la visibilità
                if (EbookFields != null && AudiobookFields != null)
                {
                    EbookFields.Visibility = (tipoSelezionato == "Ebook") ? Visibility.Visible : Visibility.Collapsed;
                    AudiobookFields.Visibility = (tipoSelezionato == "Audiobook") ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }


        private void AggiungiButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string tipoSelezionato = (TipoLibroComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (string.IsNullOrWhiteSpace(TitoloTextBox.Text) || string.IsNullOrWhiteSpace(AutoreTextBox.Text))
                {
                    MessageBox.Show("Inserisci tutti i dati obbligatori.");
                    return;
                }

                if (!decimal.TryParse(PrezzoTextBox.Text, out decimal prezzo))
                {
                    MessageBox.Show("Prezzo non valido.");
                    return;
                }

                Libro nuovoLibro;

                if (tipoSelezionato == "Ebook")
                {
                    nuovoLibro = new Ebook
                    {
                        Titolo = TitoloTextBox.Text,
                        Autore = AutoreTextBox.Text,
                        CasaEditrice = CasaEditriceTextBox.Text,
                        ISBN = ISBNTextBox.Text,
                        Prezzo = prezzo,
                        Formato = FormatoEbookTextBox.Text,
                        DimensioneFile = double.Parse(DimensioneEbookTextBox.Text),
                        Sconto = double.Parse(ScontoEbookTextBox.Text) / 100
                    };
                }
                else if (tipoSelezionato == "Audiobook")
                {
                    nuovoLibro = new Audiobook
                    {
                        Titolo = TitoloTextBox.Text,
                        Autore = AutoreTextBox.Text,
                        CasaEditrice = CasaEditriceTextBox.Text,
                        ISBN = ISBNTextBox.Text,
                        Prezzo = prezzo,
                        DurataOre = double.Parse(DurataAudiobookTextBox.Text),
                        Narratore = NarratoreAudiobookTextBox.Text
                    };
                }
                else
                {
                    nuovoLibro = new Libro
                    {
                        Titolo = TitoloTextBox.Text,
                        Autore = AutoreTextBox.Text,
                        CasaEditrice = CasaEditriceTextBox.Text,
                        ISBN = ISBNTextBox.Text,
                        Prezzo = prezzo
                    };
                }

                _libroService.AggiungiLibro(nuovoLibro);
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
