using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Logging;
using GestionaleLibreria.Data.Models;
using System;
using System.Linq;
using System.Windows;

namespace GestionaleLibreria.WPF
{
    public partial class ModificaLibroWindow : Window
    {
        private readonly LibroService _libroService;
        private readonly Libro _libro;
        private static readonly string NomeClasse = nameof(ModificaLibroWindow);

        public ModificaLibroWindow(Libro libro)
        {
            string nomeMetodo = nameof(ModificaLibroWindow);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Apertura finestra di modifica libro.");

                InitializeComponent();
                ILibroRepository libroRepository = new LibroRepository();
                var _context = new LibraryContext();
                IMagazzinoRepository magazzinoRepository = new MagazzinoRepository(_context);

                _libroService = new LibroService(libroRepository, magazzinoRepository);
                _libro = libro;

                CaricaCategorie();
                // **Popoliamo i campi con i dati del libro**
                TitoloTextBox.Text = _libro.Titolo;
                AutoreTextBox.Text = _libro.Autore;
                ISBNTextBox.Text = _libro.ISBN;
                PrezzoTextBox.Text = _libro.Prezzo.ToString();
                ScontoTextBox.Text = (_libro.Sconto * 100).ToString(); // Convertiamo lo sconto in percentuale (es. 0.10 -> 10)
                CasaEditriceTextBox.Text = _libro.CasaEditrice;
                if (_libro.CategoriaId != null)
                {
                    CategoriaComboBox.SelectedValue = _libro.CategoriaId;
                }
                // **Gestione interfaccia per Ebook o Audiobook**
                if (_libro is Ebook ebook)
                {
                    FormatoEbookPanel.Visibility = Visibility.Visible;
                    DimensioneEbookPanel.Visibility = Visibility.Visible;
                    DurataPanel.Visibility = Visibility.Collapsed;
                    NarratorePanel.Visibility = Visibility.Collapsed;

                    FormatoEbookTextBox.Text = ebook.Formato;
                    DimensioneEbookTextBox.Text = ebook.DimensioneFile.ToString();
                }
                else if (_libro is Audiobook audiobook)
                {
                    FormatoEbookPanel.Visibility = Visibility.Collapsed;
                    DimensioneEbookPanel.Visibility = Visibility.Collapsed;
                    DurataPanel.Visibility = Visibility.Visible;
                    NarratorePanel.Visibility = Visibility.Visible;

                    DurataTextBox.Text = audiobook.DurataOre.ToString();
                    NarratoreTextBox.Text = audiobook.Narratore;
                }
                else
                {
                    // **È un libro cartaceo**
                    FormatoEbookPanel.Visibility = Visibility.Collapsed;
                    DimensioneEbookPanel.Visibility = Visibility.Collapsed;
                    DurataPanel.Visibility = Visibility.Collapsed;
                    NarratorePanel.Visibility = Visibility.Collapsed;
                }

                Logger.LogInfo(NomeClasse, nomeMetodo, $"Caricati dati del libro: {_libro.Titolo} (ISBN: {_libro.ISBN})");
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante il caricamento della finestra.");
                Close();
            }
        }

        private void Salva_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(Salva_Click);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Modifica libro avviata per ISBN: {_libro.ISBN}");

                // Aggiorniamo i campi comuni
                _libro.Titolo = TitoloTextBox.Text;
                _libro.Autore = AutoreTextBox.Text;
                _libro.ISBN = ISBNTextBox.Text;
                _libro.Prezzo = decimal.Parse(PrezzoTextBox.Text);
                _libro.CasaEditrice = CasaEditriceTextBox.Text;

                if (double.TryParse(ScontoTextBox.Text, out double sconto))
                {
                    _libro.Sconto = sconto / 100;
                }

                // Assegna la categoria selezionata
                if (CategoriaComboBox.SelectedValue != null)
                {
                    _libro.CategoriaId = (int)CategoriaComboBox.SelectedValue;
                }

                // Salviamo le modifiche
                _libroService.ModificaLibro(_libro);
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro modificato con successo: {_libro.Titolo} (ISBN: {_libro.ISBN})");

                MessageBox.Show("Libro modificato con successo!");
                Close();
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante la modifica: " + ex.Message);
            }
        }

        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(Annulla_Click);
            Logger.LogInfo(NomeClasse, nomeMetodo, "Modifica annullata.");
            Close();
        }

        private void CaricaCategorie()
        {
            using (var context = new LibraryContext())
            {
                var categorie = context.Categorie.ToList();
                CategoriaComboBox.ItemsSource = categorie;
            }
        }

    }
}
