using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Paragraph = iTextSharp.text.Paragraph;
using System.Windows.Controls;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Input;
using GestionaleLibreria.Data.Logging;

namespace GestionaleLibreria.WPF
{
    public partial class MagazzinoWindow : Window
    {
        private readonly MagazzinoService _magazzinoService;
        private List<LibroMagazzino> _libriSelezionati = new List<LibroMagazzino>();
        private List<LibroMagazzinoViewModel> _listaLibriMagazzino = new List<LibroMagazzinoViewModel>();
        private List<LibroMagazzinoViewModel> _tuttiLibriMagazzino = new List<LibroMagazzinoViewModel>();
        private static readonly string NomeClasse = nameof(MagazzinoWindow);


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
            string nomeMetodo = nameof(CaricaLibri);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Caricamento libri in magazzino.");
                var libriMagazzino = _magazzinoService.GetLibriMagazzino()
                    .Select(lm => new LibroMagazzinoViewModel { LibroMagazzino = lm, IsSelected = false })
                    .ToList();

                foreach (var libro in libriMagazzino)
                {
                    var libroEsistente = _listaLibriMagazzino.FirstOrDefault(l => l.LibroMagazzino.LibroId == libro.LibroMagazzino.LibroId);
                    if (libroEsistente != null)
                    {
                        libro.IsSelected = libroEsistente.IsSelected;
                    }
                }

                _tuttiLibriMagazzino = libriMagazzino;
                _listaLibriMagazzino = libriMagazzino;
                MagazzinoDataGrid.ItemsSource = _listaLibriMagazzino;
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante il caricamento dei libri.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void FiltraLibriMagazzino_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(FiltraLibriMagazzino_Click);
            try
            {
                string filtro = FiltroMagazzinoTextBox.Text.Trim();
                string criterio = ((ComboBoxItem)FiltroCriterioMagazzinoComboBox.SelectedItem)?.Content.ToString();

                if (string.IsNullOrEmpty(filtro))
                {
                    MagazzinoDataGrid.ItemsSource = _tuttiLibriMagazzino;
                    return;
                }

                int filtroQuantita = 0;

                if (criterio == "Quantità" && !int.TryParse(filtro, out filtroQuantita))
                {
                    MessageBox.Show("Inserisci solo numeri interi per la quantità.", "Errore di input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var risultati = _tuttiLibriMagazzino
                    .Where(lm =>
                        (criterio == "Titolo" && lm.LibroMagazzino.Libro.Titolo.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (criterio == "Autore" && lm.LibroMagazzino.Libro.Autore.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (criterio == "ISBN" && lm.LibroMagazzino.Libro.ISBN == filtro) ||
                        (criterio == "Quantità" && lm.LibroMagazzino.Quantita == filtroQuantita))
                    .ToList();

                MagazzinoDataGrid.ItemsSource = risultati;
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante il filtraggio dei libri.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void FiltroMagazzinoTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            string criterio = ((ComboBoxItem)FiltroCriterioMagazzinoComboBox.SelectedItem)?.Content.ToString();

            // Se il criterio è "Quantità", consenti solo numeri
            if (criterio == "Quantità")
            {
                e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
            }
            else
            {
                e.Handled = false; // Permette lettere per Titolo, Autore e ISBN
            }
        }



        private void AggiungiStock_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(AggiungiStock_Click);
            try
            {
                if (MagazzinoDataGrid.SelectedItem is LibroMagazzinoViewModel libroMagazzinoViewModel)
                {
                    var libroMagazzinoSelezionato = libroMagazzinoViewModel.LibroMagazzino;

                    if (libroMagazzinoSelezionato != null)
                    {
                        Logger.LogInfo(NomeClasse, nomeMetodo, $"Apertura finestra per aggiungere stock a '{libroMagazzinoSelezionato.Libro.Titolo}' (ID: {libroMagazzinoSelezionato.LibroId}).");
                        var aggiungiStockWindow = new AggiungiStockWindow(_magazzinoService, libroMagazzinoSelezionato);
                        aggiungiStockWindow.ShowDialog();
                        CaricaLibri();
                        return;
                    }
                }

                MessageBox.Show("Seleziona un libro per aggiungere quantità.");
                Logger.LogInfo(NomeClasse, nomeMetodo, "Tentativo di aggiungere stock senza selezionare un libro.");
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante l'aggiunta dello stock.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RimuoviStock_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(RimuoviStock_Click);
            try
            {
                if (MagazzinoDataGrid.SelectedItem is LibroMagazzinoViewModel libroMagazzinoViewModel)
                {
                    var libroMagazzinoSelezionato = libroMagazzinoViewModel.LibroMagazzino;

                    if (libroMagazzinoSelezionato != null)
                    {
                        Logger.LogInfo(NomeClasse, nomeMetodo, $"Apertura finestra per rimuovere stock da '{libroMagazzinoSelezionato.Libro.Titolo}' (ID: {libroMagazzinoSelezionato.LibroId}).");
                        var rimuoviStockWindow = new RimuoviStockWindow(_magazzinoService, libroMagazzinoSelezionato);
                        rimuoviStockWindow.ShowDialog();
                        CaricaLibri();
                        return;
                    }
                }

                MessageBox.Show("Seleziona un libro per rimuovere quantità.");
                Logger.LogInfo(NomeClasse, nomeMetodo, "Tentativo di rimuovere stock senza selezionare un libro.");
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore durante la rimozione dello stock.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void GeneraPDF_Click(object sender, RoutedEventArgs e)
        {
            string nomeMetodo = nameof(GeneraPDF_Click);
            try
            {
                _libriSelezionati = _listaLibriMagazzino
                                    .Where(l => l.IsSelected)
                                    .Select(l => l.LibroMagazzino)
                                    .ToList();

                if (_libriSelezionati.Count == 0)
                {
                    MessageBox.Show("Seleziona almeno un libro per generare il PDF.");
                    return;
                }

                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Lista_Libri_Selezionati.pdf");

                using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (Document pdfDoc = new Document(PageSize.A4))
                    {
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();

                        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                        pdfDoc.Add(new Paragraph("Lista Libri Selezionati", titleFont) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 10 });
                        pdfDoc.Add(new Paragraph(" "));

                        PdfPTable table = new PdfPTable(2) { WidthPercentage = 100 };
                        table.SetWidths(new float[] { 40, 30});
                        table.AddCell(new PdfPCell(new Phrase("Titolo", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)))
                        {
                            BackgroundColor = BaseColor.LIGHT_GRAY,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });

                        table.AddCell(new PdfPCell(new Phrase("ISBN", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)))
                        {
                            BackgroundColor = BaseColor.LIGHT_GRAY,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });


                        foreach (var libro in _libriSelezionati)
                        {
                            table.AddCell(new PdfPCell(new Phrase(libro.Libro.Titolo, FontFactory.GetFont(FontFactory.HELVETICA, 11)))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });

                            table.AddCell(new PdfPCell(new Phrase(libro.Libro.ISBN, FontFactory.GetFont(FontFactory.HELVETICA, 11)))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });
                        }

                        pdfDoc.Add(table);
                        pdfDoc.Close();
                    }
                }

                Logger.LogInfo(NomeClasse, nomeMetodo, $"PDF generato: {filePath}");
                MessageBox.Show($"PDF generato con successo!\nFile salvato in: {filePath}");
                System.Diagnostics.Process.Start(filePath);
            }
            catch (IOException ioEx)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ioEx);
                MessageBox.Show("Errore: il file potrebbe essere già aperto in un altro programma.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                MessageBox.Show("Errore nella generazione del PDF.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }






    public class LibroMagazzinoViewModel : INotifyPropertyChanged
        {
            private bool _isSelected;

            public LibroMagazzino LibroMagazzino { get; set; }

            public bool IsSelected
            {
                get => _isSelected;
                set
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

   


