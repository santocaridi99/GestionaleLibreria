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
            InitializeComponent();
            _libroService = libroService;
            _clienteService = clienteService;
            _venditaService = venditaService;
            _carrello = new List<VenditaDettaglio>();

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
                int quantitaNelCarrello = esistente?.Quantita ?? 0;

                // Verifica che la quantità totale nel carrello non superi la disponibilità
                if (quantitaNelCarrello >= libro.QuantitaMagazzino)
                {
                    MessageBox.Show($"Hai già selezionato tutte le copie disponibili di \"{libro.Titolo}\".", "Attenzione", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (esistente != null)
                {
                    esistente.Quantita++;
                }
                else
                {
                    _carrello.Add(new VenditaDettaglio
                    {
                        LibroId = libro.Id,
                        Libro = libro,
                        Quantita = 1,
                        PrezzoUnitario = libro.Prezzo
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
            CarrelloDataGrid.ItemsSource = null;
            CarrelloDataGrid.ItemsSource = _carrello;

            // Calcola il totale della vendita
            decimal totale = _carrello.Sum(item => item.Quantita * item.PrezzoUnitario);
            TotaleVenditaTextBlock.Text = $"{totale:0.00} €";
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
                PrezzoUnitario = d.PrezzoUnitario,
               
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

                    // **Dettagli Cliente**
                    var bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                    if (_clienteSelezionato != null)
                    {
                        document.Add(new Paragraph($"Cliente: {_clienteSelezionato.Nome} {_clienteSelezionato.Cognome}", bodyFont));
                        document.Add(new Paragraph($"Email: {_clienteSelezionato.Email}", bodyFont));
                        document.Add(new Paragraph(" "));
                    }
                    else
                    {
                        document.Add(new Paragraph("Cliente: NON REGISTRATO", bodyFont));
                    }

                    // **Metodo di pagamento**
                    document.Add(new Paragraph($"Metodo di pagamento: {vendita.MetodoPagamento}", bodyFont));
                    document.Add(new Paragraph(" ")); // Spazio

                    // **Tabella dei libri venduti**
                    PdfPTable table = new PdfPTable(4);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 40, 20, 20, 20 });

                    table.AddCell(new PdfPCell(new Phrase("Titolo", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    table.AddCell(new PdfPCell(new Phrase("Quantità", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    table.AddCell(new PdfPCell(new Phrase("Prezzo Unitario", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    table.AddCell(new PdfPCell(new Phrase("Totale", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });

                    decimal totaleVendita = 0;
                    foreach (var item in _carrello)
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Libro.Titolo, bodyFont)));
                        table.AddCell(new PdfPCell(new Phrase(item.Quantita.ToString(), bodyFont)));
                        table.AddCell(new PdfPCell(new Phrase($"{item.PrezzoUnitario:C}", bodyFont)));
                        table.AddCell(new PdfPCell(new Phrase($"{(item.PrezzoUnitario * item.Quantita):C}", bodyFont)));

                        totaleVendita += item.PrezzoUnitario * item.Quantita;
                    }

                    document.Add(table);
                    document.Add(new Paragraph(" ")); // Spazio

                    // **Totale della vendita**
                    var totalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                    document.Add(new Paragraph($"Totale: {totaleVendita:C}", totalFont));

                    document.Close();
                }

                // Apri automaticamente il PDF
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
