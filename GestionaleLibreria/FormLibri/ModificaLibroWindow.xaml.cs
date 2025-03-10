
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Logging;
using GestionaleLibreria.Data.Models;
using System;
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

                // Iniezione delle dipendenze
                ILibroRepository libroRepository = new LibroRepository();
                var _context = new LibraryContext();
                IMagazzinoRepository magazzinoRepository = new MagazzinoRepository(_context);

                _libroService = new LibroService(libroRepository, magazzinoRepository);
                _libro = libro;

                // Popola i campi con i dati del libro
                TitoloTextBox.Text = _libro.Titolo;
                AutoreTextBox.Text = _libro.Autore;
                ISBNTextBox.Text = _libro.ISBN;
                PrezzoTextBox.Text = _libro.Prezzo.ToString();

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

                _libro.Titolo = TitoloTextBox.Text;
                _libro.Autore = AutoreTextBox.Text;
                _libro.ISBN = ISBNTextBox.Text;
                _libro.Prezzo = decimal.Parse(PrezzoTextBox.Text);

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
    }
}
