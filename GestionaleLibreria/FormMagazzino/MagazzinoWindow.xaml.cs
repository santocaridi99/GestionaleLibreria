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

namespace GestionaleLibreria.WPF
{
    public partial class MagazzinoWindow : Window
    {
        private readonly MagazzinoService _magazzinoService;
        private List<LibroMagazzino> _libriSelezionati = new List<LibroMagazzino>();
        private List<LibroMagazzinoViewModel> _listaLibriMagazzino = new List<LibroMagazzinoViewModel>();
        private List<LibroMagazzinoViewModel> _tuttiLibriMagazzino = new List<LibroMagazzinoViewModel>();



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
            var libriMagazzino = _magazzinoService.GetLibriMagazzino()
                .Select(lm => new LibroMagazzinoViewModel { LibroMagazzino = lm, IsSelected = false })
                .ToList();

            // Mantieni i valori selezionati
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

        private void FiltraLibri_Click(object sender, RoutedEventArgs e)
        {
            string filtro = FiltroLibroTextBox.Text.ToLower().Trim();

           
            var risultati = _tuttiLibriMagazzino
                .Where(lm => lm.LibroMagazzino.Libro.Titolo.ToLower().Contains(filtro)
                          || lm.LibroMagazzino.Libro.ISBN.ToLower().Contains(filtro))
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

        private  void GeneraPDF_Click(object sender, RoutedEventArgs e)
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

            try
            {
                // Percorso del file PDF
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Lista_Libri_Selezionati.pdf");

                // Usa un blocco using per garantire che il file venga correttamente chiuso
                using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    using (Document pdfDoc = new Document(PageSize.A4))
                    {
                        PdfWriter.GetInstance(pdfDoc, stream);

                       
                        pdfDoc.Open();

                        var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                        var title = new Paragraph("Lista Libri Selezionati", titleFont)
                        {
                            Alignment = Element.ALIGN_CENTER,
                            SpacingAfter = 10
                        };
                        pdfDoc.Add(title);

                        pdfDoc.Add(new Paragraph(" "));

                        // Creazione tabella
                        PdfPTable table = new PdfPTable(3)
                        {
                            WidthPercentage = 100
                        };

                        table.SetWidths(new float[] { 40, 30, 30 });

                        // Intestazioni tabella
                        PdfPCell cell = new PdfPCell(new Phrase("Titolo", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)))
                        {
                            BackgroundColor = BaseColor.LIGHT_GRAY,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        };
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("ISBN", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)))
                        {
                            BackgroundColor = BaseColor.LIGHT_GRAY,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        };
                        table.AddCell(cell);

                        cell = new PdfPCell(new Phrase("Quantità", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)))
                        {
                            BackgroundColor = BaseColor.LIGHT_GRAY,
                            HorizontalAlignment = Element.ALIGN_CENTER
                        };
                        table.AddCell(cell);

                        // Inserimento dati
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

                            table.AddCell(new PdfPCell(new Phrase(libro.Quantita.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 11)))
                            {
                                HorizontalAlignment = Element.ALIGN_CENTER
                            });
                        }

                        pdfDoc.Add(table);
                        pdfDoc.Close();
                    }
                }

                MessageBox.Show($"PDF generato con successo!\nFile salvato in: {filePath}");

                // Apri il PDF appena creato
                System.Diagnostics.Process.Start(filePath);
            }
            catch (IOException ioEx)
            {
                MessageBox.Show("Errore: il file potrebbe essere già aperto in un altro programma.\nChiudilo e riprova.\n\nDettagli: " + ioEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore nella generazione del PDF: " + ex.Message);
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

   

}
