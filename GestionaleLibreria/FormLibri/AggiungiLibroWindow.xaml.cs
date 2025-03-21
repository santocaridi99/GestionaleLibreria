﻿
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Logging;
using GestionaleLibreria.Data.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GestionaleLibreria.WPF
{
    public partial class AggiungiLibroWindow : Window
    {
        private readonly LibroService _libroService;
        private static readonly string NomeClasse = nameof(AggiungiLibroWindow);

        public AggiungiLibroWindow()
        {
            string nomeMetodo = nameof(AggiungiLibroWindow);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Inizializzazione finestra di aggiunta libro.");

                InitializeComponent();
                var context = new LibraryContext();

                ILibroRepository libroRepository = new LibroRepository();
                IMagazzinoRepository magazzinoRepository = new MagazzinoRepository(context);

                MagazzinoService magazzinoService = new MagazzinoService(magazzinoRepository, libroRepository);
                _libroService = new LibroService(libroRepository, magazzinoRepository);

                CaricaCategorie();

                Logger.LogInfo(NomeClasse, nomeMetodo, "Inizializzazione completata.");
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        private void CaricaCategorie()
        {
            using (var context = new LibraryContext())
            {
                var categorie = context.Categorie.OrderBy(c => c.Nome).ToList();
                CategoriaComboBox.ItemsSource = categorie;
                CategoriaComboBox.DisplayMemberPath = "Nome";  // Mostra il nome
                CategoriaComboBox.SelectedValuePath = "Id";   // Seleziona l'ID
            }
        }


        private void TipoLibroComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string nomeMetodo = nameof(TipoLibroComboBox_SelectionChanged);
            try
            {
                if (!IsLoaded) return;

                if (TipoLibroComboBox.SelectedItem is ComboBoxItem selectedItem)
                {
                    string tipoSelezionato = selectedItem.Content.ToString();
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Tipo di libro selezionato: {tipoSelezionato}");

                    if (EbookFields != null && AudiobookFields != null)
                    {
                        EbookFields.Visibility = (tipoSelezionato == "Ebook") ? Visibility.Visible : Visibility.Collapsed;
                        AudiobookFields.Visibility = (tipoSelezionato == "Audiobook") ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
            }
        }

        private void AggiungiButton_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(AggiungiButton_Click);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Tentativo di aggiunta di un nuovo libro.");

                if (string.IsNullOrWhiteSpace(TitoloTextBox.Text) || string.IsNullOrWhiteSpace(AutoreTextBox.Text))
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, "Errore: alcuni campi obbligatori sono vuoti.");
                    MessageBox.Show("Inserisci tutti i dati obbligatori.");
                    return;
                }

                if (!decimal.TryParse(PrezzoTextBox.Text, out decimal prezzo))
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, "Errore: il prezzo inserito non è valido.");
                    MessageBox.Show("Prezzo non valido.");
                    return;
                }

                double sconto = 0;
                if (!string.IsNullOrWhiteSpace(ScontoTextBox.Text) && !double.TryParse(ScontoTextBox.Text, out sconto))
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, "Errore: lo sconto inserito non è valido.");
                    MessageBox.Show("Sconto non valido. Inserire un numero.");
                    return;
                }

                sconto = sconto / 100;  // Convertiamo lo sconto in percentuale

                string tipoSelezionato = (TipoLibroComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                int categoriaId = (int)CategoriaComboBox.SelectedValue;

                if (categoriaId == null)
                {
                    MessageBox.Show("Seleziona una categoria.");
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
                        Sconto = sconto,
                        Formato = FormatoEbookTextBox.Text,
                        DimensioneFile = double.TryParse(DimensioneEbookTextBox.Text, out double dimensione) ? dimensione : 0,
                        CategoriaId = categoriaId
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
                        Sconto = sconto,
                        DurataOre = double.TryParse(DurataAudiobookTextBox.Text, out double durata) ? durata : 0,
                        Narratore = NarratoreAudiobookTextBox.Text,
                        CategoriaId = categoriaId
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
                        Prezzo = prezzo,
                        Sconto = sconto,
                        CategoriaId = categoriaId
                    };
                }

                _libroService.AggiungiLibro(nuovoLibro);

                Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro aggiunto con successo: {nuovoLibro.Titolo}, ISBN: {nuovoLibro.ISBN}");

                MessageBox.Show("Libro aggiunto con successo!");
                Close();
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore: " + ex.Message);
            }
        }



        private void AnnullaAggiungi_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(AnnullaAggiungi_Click);
            Logger.LogInfo(NomeClasse, nomeMetodo, "Operazione annullata dall'utente.");
            Close();
        }
    }
}
