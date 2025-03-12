using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GestionaleLibreria.Business;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Models;
using GestionaleLibreria.Data.Logging;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Windows.Markup;

namespace GestionaleLibreria.WPF
{
    public partial class VenditaWindow : Window
    {
        private readonly LibroService _libroService;
        private readonly ClienteService _clienteService;
        private readonly VenditaService _venditaService;
        private List<VenditaDettaglio> _carrello;
        private Cliente _clienteSelezionato;

        public VenditaWindow(LibroService libroService, ClienteService clienteService, VenditaService venditaService)
        {
            Logger.LogInfo(nameof(VenditaWindow), "Costruttore", "Apertura finestra Vendita");
            InitializeComponent();
            _libroService = libroService;
            _clienteService = clienteService;
            _venditaService = venditaService;
            _carrello = new List<VenditaDettaglio>();
            this.Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);

            CaricaLibri();
        }

        private void CaricaLibri()
        {
            LibriVenditaDataGrid.ItemsSource = _libroService.GetAllLibri()
                .Where(l => l.QuantitaMagazzino > 0)
                .ToList();
        }

        private void CercaLibro_Click(object sender, RoutedEventArgs e)
        {
            string filtro = RicercaLibroTextBox.Text.Trim().ToLower();
            LibriVenditaDataGrid.ItemsSource = _libroService.GetAllLibri()
                .Where(l => l.Titolo.ToLower().Contains(filtro) && l.QuantitaMagazzino > 0)
                .ToList();
        }
        private void AggiungiAlCarrello_Click(object sender, RoutedEventArgs e)
        {
            if (LibriVenditaDataGrid.SelectedItem is Libro libro)
            {
                if (libro.QuantitaMagazzino == 0)
                {
                    MessageBox.Show("Questo libro non è disponibile in magazzino!", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var esistente = _carrello.FirstOrDefault(c => c.LibroId == libro.Id);

                if (esistente != null)
                {
                    // Se il libro è già nel carrello, aumenta la quantità
                    if (esistente.Quantita < libro.QuantitaMagazzino)
                    {
                        esistente.Quantita++;
                    }
                    else
                    {
                        MessageBox.Show($"Non ci sono più copie disponibili di \"{libro.Titolo}\".", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                else
                {
                    // Aggiunge il libro con prezzo originale e prezzo scontato
                    _carrello.Add(new VenditaDettaglio
                    {
                        LibroId = libro.Id,
                        Libro = libro,
                        Quantita = 1,
                        PrezzoOriginale = libro.Prezzo, // Prezzo senza sconto
                        PrezzoUnitario = libro.CalcolaPrezzo(), // Prezzo con sconto
                    });
                }

                AggiornaCarrello();
            }
        }


        private void RimuoviDalCarrello_Click(object sender, RoutedEventArgs e)
        {
            if (CarrelloDataGrid.SelectedItem is VenditaDettaglio venditaDettaglio)
            {
                _carrello.Remove(venditaDettaglio);
                AggiornaCarrello();
            }
        }

        private void AggiornaCarrello()
        {
            CarrelloDataGrid.ItemsSource = null; // Forza l'aggiornamento UI

            CarrelloDataGrid.ItemsSource = _carrello.Select(item => new
            {
                Titolo = item.Libro.Titolo,
                Tipo = item.Libro.Tipo,
                Sconto = $"{item.Libro.Sconto * 100:0.##}%",
                Quantita = item.Quantita,
                PrezzoOriginale = item.Libro.Prezzo,
                PrezzoScontato = item.Libro.CalcolaPrezzo(), // Prezzo dopo sconto
                Totale = item.Quantita * item.Libro.CalcolaPrezzo() // Prezzo totale
            }).ToList();

            // Calcola il totale della vendita
            decimal totale = _carrello.Sum(item => item.Quantita * item.Libro.CalcolaPrezzo());
            TotaleVenditaTextBlock.Text = string.Format(new CultureInfo("it-IT"), "{0:C}", totale);
        }




        private void AumentaQuantita_Click(object sender, RoutedEventArgs e)
        {
            if (CarrelloDataGrid.SelectedItem is VenditaDettaglio dettaglio)
            {
                if (dettaglio.Quantita < dettaglio.Libro.QuantitaMagazzino)
                {
                    dettaglio.Quantita++;
                    AggiornaCarrello();
                }
                else
                {
                    MessageBox.Show("Quantità massima disponibile in magazzino raggiunta.", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void DiminuisciQuantita_Click(object sender, RoutedEventArgs e)
        {
            if (CarrelloDataGrid.SelectedItem is VenditaDettaglio dettaglio)
            {
                if (dettaglio.Quantita > 1)
                {
                    dettaglio.Quantita--;
                }
                else
                {
                    _carrello.Remove(dettaglio);
                }
                AggiornaCarrello();
            }
        }




        private void SelezionaCliente_Click(object sender, RoutedEventArgs e)
        {
            var finestraClienti = new SelezionaClienteWindow(_clienteService);
            if (finestraClienti.ShowDialog() == true)
            {
                _clienteSelezionato = finestraClienti.ClienteSelezionato;
                ClienteTextBox.Text = $"{_clienteSelezionato.Nome} {_clienteSelezionato.Cognome}";
            }
            else
            {
                _clienteSelezionato = new Cliente
                {
                    Id = -1,  // ID temporaneo per indicare un cliente anonimo
                    Nome = "Cliente Temporaneo",
                    Cognome = "",
                    Email = "",
                    Telefono = ""
                };
                ClienteTextBox.Text = "Cliente Temporaneo";
            }
        }

        private void RegistraVendita_Click(object sender, RoutedEventArgs e)
        {
            if (_carrello.Count == 0)
            {
                MessageBox.Show("Aggiungi almeno un libro al carrello.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string metodoPagamento = (MetodoPagamentoComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (string.IsNullOrEmpty(metodoPagamento))
            {
                MessageBox.Show("Seleziona un metodo di pagamento.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var nuovaVendita = new Vendita
            {
                ClienteId = _clienteSelezionato?.Id,
                MetodoPagamento = metodoPagamento,
                DataVendita = DateTime.Now
            };

            // Creiamo una lista separata per evitare di passare entità tracciate
            var dettagliVendita = _carrello.Select(d => new VenditaDettaglio
            {
                LibroId = d.LibroId,
                Quantita = d.Quantita,
                
               
            }).ToList();

            try
            {
                _venditaService.RegistraVendita(nuovaVendita, dettagliVendita);

                MessageBox.Show("Vendita registrata con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);

                // Opzione per stampare PDF
                if (MessageBox.Show("Vuoi stampare il riepilogo della vendita?", "Stampa PDF", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    StampaVenditaPDF(nuovaVendita);
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante la registrazione della vendita: {ex.Message}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void StampaVenditaPDF(Vendita vendita)
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, $"Vendita_{vendita.Id}.pdf");

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    Document document = new Document(PageSize.A4);
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    // **Titolo**
                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                    var title = new Paragraph("Riepilogo Vendita", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    document.Add(title);
                    document.Add(new Paragraph(" ")); // Spazio vuoto

                    // **Metodo di pagamento**
                    document.Add(new Paragraph($"Metodo di pagamento: {vendita.MetodoPagamento}", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                    document.Add(new Paragraph(" ")); // Spazio

                    // **Tabella dei libri venduti**
                    PdfPTable table = new PdfPTable(6);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 30, 15, 15, 15, 15, 10 });

                    table.AddCell(new PdfPCell(new Phrase("Titolo", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    table.AddCell(new PdfPCell(new Phrase("Tipo", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    table.AddCell(new PdfPCell(new Phrase("Prezzo Base", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    table.AddCell(new PdfPCell(new Phrase("Sconto", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    table.AddCell(new PdfPCell(new Phrase("Prezzo Scontato", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    table.AddCell(new PdfPCell(new Phrase("Quantità", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });

                    decimal totaleVendita = 0;

                    foreach (var item in _carrello)
                    {
                        decimal prezzoBase = item.Libro.Prezzo;
                        decimal prezzoScontato = item.Libro.CalcolaPrezzo();
                        string scontoDescrizione = $"{item.Libro.Sconto * 100:0.##}%";

                        if (item.Libro is Ebook)
                        {
                            scontoDescrizione += " + Sconto 50% (Ebook)";
                        }
                        else if (item.Libro is Audiobook)
                        {
                            scontoDescrizione += " + Supplemento 5€ (Audiobook)";
                        }

                        table.AddCell(new PdfPCell(new Phrase(item.Libro.Titolo, FontFactory.GetFont(FontFactory.HELVETICA, 12))));
                        table.AddCell(new PdfPCell(new Phrase(item.Libro.Tipo, FontFactory.GetFont(FontFactory.HELVETICA, 12))));
                        table.AddCell(new PdfPCell(new Phrase($"{prezzoBase:C}", FontFactory.GetFont(FontFactory.HELVETICA, 12))));
                        table.AddCell(new PdfPCell(new Phrase(scontoDescrizione, FontFactory.GetFont(FontFactory.HELVETICA, 12))));
                        table.AddCell(new PdfPCell(new Phrase($"{prezzoScontato:C}", FontFactory.GetFont(FontFactory.HELVETICA, 12))));
                        table.AddCell(new PdfPCell(new Phrase(item.Quantita.ToString(), FontFactory.GetFont(FontFactory.HELVETICA, 12))));

                        totaleVendita += prezzoScontato * item.Quantita;
                    }

                    document.Add(table);
                    document.Add(new Paragraph(" "));

                    var totalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                    document.Add(new Paragraph($"Totale: {totaleVendita:C}", totalFont));

                    document.Close();
                }

                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });

                MessageBox.Show("PDF generato con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante la generazione del PDF: {ex.Message}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
