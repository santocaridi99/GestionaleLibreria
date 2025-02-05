using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using System;
using System.Windows;

namespace GestionaleLibreria.WPF
{
    public partial class ModificaLibroWindow : Window
    {
        private readonly LibroService _libroService;
        private readonly Libro _libro;

        public ModificaLibroWindow(Libro libro)
        {
            InitializeComponent();
            // Iniezione delle dipendenze
            ILibroRepository libroRepository = new LibroRepository();
            _libroService = new LibroService(libroRepository);

            _libro = libro;
            // Popola i campi con i dati del libro
            TitoloTextBox.Text = _libro.Titolo;
            AutoreTextBox.Text = _libro.Autore;
            ISBNTextBox.Text = _libro.ISBN;
            PrezzoTextBox.Text = _libro.Prezzo.ToString();
          
        }

        private void Salva_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _libro.Titolo = TitoloTextBox.Text;
                _libro.Autore = AutoreTextBox.Text;
                _libro.ISBN = ISBNTextBox.Text;
                _libro.Prezzo = decimal.Parse(PrezzoTextBox.Text);
              
                _libroService.ModificaLibro(_libro);
                MessageBox.Show("Libro modificato con successo!");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante la modifica: " + ex.Message);
            }
        }

        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
