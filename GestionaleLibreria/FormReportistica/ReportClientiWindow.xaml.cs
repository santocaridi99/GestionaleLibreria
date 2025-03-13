using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using GestionaleLibreria.Business;
using System.Threading;
using System.Windows.Markup;

namespace GestionaleLibreria.WPF.FormReportistica
{
    public partial class ReportClientiWindow : Window
    {
        private readonly ClienteService _clienteService;
        private readonly VenditaService _venditaService;

        public ReportClientiWindow(ClienteService clienteService, VenditaService venditaService)
        {
            InitializeComponent();
            _clienteService = clienteService;
            _venditaService = venditaService;
            this.Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);

            CaricaDatiClienti();
        }
        private void CaricaDatiClienti()
        {
            var clienti = _clienteService.GetAllClienti();

            var reportClienti = clienti.Select(cliente => new
            {
                Nome = cliente.Nome ?? "N/D",
                Cognome = cliente.Cognome ?? "N/D",
                Email = cliente.Email ?? "N/D",
                NumeroAcquisti = _venditaService.GetNumeroAcquistiCliente(cliente.Id),
                TotaleSpeso = _venditaService.GetTotaleSpesoCliente(cliente.Id)
            }).ToList();

            ClientiDataGrid.ItemsSource = reportClienti;

            Console.WriteLine("Dati Report Clienti:");
            foreach (var c in reportClienti)
            {
                Console.WriteLine($"Cliente: {c.Nome} {c.Cognome}, Acquisti: {c.NumeroAcquisti}, Totale: {c.TotaleSpeso}");
            }
        }


        private void EsportaPDF_Click(object sender, RoutedEventArgs e)
        {
            CaricaDatiClienti();
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, "Report_Clienti.pdf");

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    Document document = new Document(PageSize.A4);
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    // **Titolo**
                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                    var title = new Paragraph("Report Clienti", titleFont)
                    {
                        Alignment = Element.ALIGN_CENTER
                    };
                    document.Add(title);
                    document.Add(new Paragraph(" ")); // Spazio vuoto

                    // **Tabella**
                    PdfPTable table = new PdfPTable(5);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 20, 20, 30, 15, 15 });

                    table.AddCell(new PdfPCell(new Phrase("Nome", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    table.AddCell(new PdfPCell(new Phrase("Cognome", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    table.AddCell(new PdfPCell(new Phrase("Email", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    table.AddCell(new PdfPCell(new Phrase("Acquisti", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    table.AddCell(new PdfPCell(new Phrase("Totale Speso (€)", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });

                    // **Conversione sicura**
                    var listaClienti = ClientiDataGrid.ItemsSource.Cast<object>().ToList();

                    foreach (var item in listaClienti)
                    {
                        var nome = item.GetType().GetProperty("Nome")?.GetValue(item, null)?.ToString() ?? "N/D";
                        var cognome = item.GetType().GetProperty("Cognome")?.GetValue(item, null)?.ToString() ?? "N/D";
                        var email = item.GetType().GetProperty("Email")?.GetValue(item, null)?.ToString() ?? "N/D";
                        var numeroAcquisti = item.GetType().GetProperty("NumeroAcquisti")?.GetValue(item, null)?.ToString() ?? "0";
                        var totaleSpeso = item.GetType().GetProperty("TotaleSpeso")?.GetValue(item, null)?.ToString() ?? "0.00";

                        table.AddCell(new PdfPCell(new Phrase(nome, FontFactory.GetFont(FontFactory.HELVETICA, 12))));
                        table.AddCell(new PdfPCell(new Phrase(cognome, FontFactory.GetFont(FontFactory.HELVETICA, 12))));
                        table.AddCell(new PdfPCell(new Phrase(email, FontFactory.GetFont(FontFactory.HELVETICA, 12))));
                        table.AddCell(new PdfPCell(new Phrase(numeroAcquisti, FontFactory.GetFont(FontFactory.HELVETICA, 12))));
                        table.AddCell(new PdfPCell(new Phrase($"{decimal.Parse(totaleSpeso).ToString("N2", CultureInfo.GetCultureInfo("it-IT"))} €",
                                            FontFactory.GetFont(FontFactory.HELVETICA, 12))));
                    }

                    document.Add(table);
                    document.Close();
                }

                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                MessageBox.Show("PDF generato con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore nella generazione del PDF: {ex.Message}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void Chiudi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
