using System;
using System.Windows;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Logging;
using GestionaleLibreria.Data.Models;
using System.Collections.Generic;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using GestionaleLibreria.Business;

namespace GestionaleLibreria.WPF.FormReportistica
{
    public partial class ReportVenditeWindow : Window
    {
        private readonly VenditaService _venditaService;

        public ReportVenditeWindow(VenditaService venditaService)
        {
            InitializeComponent();
            _venditaService = venditaService;
        }

        private void GeneraReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!DataInizioPicker.SelectedDate.HasValue || !DataFinePicker.SelectedDate.HasValue)
                {
                    MessageBox.Show("Seleziona un intervallo di tempo valido.", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Data inizio (00:00:00) e Data fine (23:59:59)
                DateTime dataInizio = DataInizioPicker.SelectedDate.Value.Date; // 12/03/2025 00:00:00
                DateTime dataFine = DataFinePicker.SelectedDate.Value.Date.AddDays(1).AddSeconds(-1); // 12/03/2025 23:59:59

                if (dataInizio > dataFine)
                {
                    MessageBox.Show("La data di inizio non può essere successiva alla data di fine.", "Errore", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Stampiamo i valori per debugging
                Console.WriteLine($"Cerco vendite dal {dataInizio:yyyy-MM-dd HH:mm:ss} al {dataFine:yyyy-MM-dd HH:mm:ss}");

                var vendite = _venditaService.GetVenditePerPeriodo(dataInizio, dataFine);

                if (!vendite.Any())
                {
                    MessageBox.Show("Nessuna vendita trovata per il periodo selezionato.", "Informazione", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                GeneraPDFReport(vendite, dataInizio, dataFine);
                MessageBox.Show("Report generato con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Logger.LogError(nameof(ReportVenditeWindow), nameof(GeneraReport_Click), ex);
                MessageBox.Show("Errore durante la generazione del report.", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void GeneraPDFReport(List<Vendita> vendite, DateTime dataInizio, DateTime dataFine)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, $"Report_Vendite_{dataInizio:yyyyMMdd}_{dataFine:yyyyMMdd}.pdf");

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                Document document = new Document(PageSize.A4);
                PdfWriter.GetInstance(document, stream);
                document.Open();

                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                var bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);

                document.Add(new Paragraph("📊 Report Vendite", titleFont) { Alignment = Element.ALIGN_CENTER });
                document.Add(new Paragraph($"Periodo: {dataInizio:dd/MM/yyyy} - {dataFine:dd/MM/yyyy}", bodyFont));
                document.Add(new Paragraph(" "));

                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 30, 20, 25, 25 });

                table.AddCell(new PdfPCell(new Phrase("Data Vendita", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Cliente", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Metodo Pagamento", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Totale Vendita", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });

                decimal totaleVendite = 0;
                foreach (var vendita in vendite)
                {
                    table.AddCell(new PdfPCell(new Phrase(vendita.DataVendita.ToString("dd/MM/yyyy"), bodyFont)));
                    table.AddCell(new PdfPCell(new Phrase(vendita.Cliente?.Nome ?? "Anonimo", bodyFont)));
                    table.AddCell(new PdfPCell(new Phrase(vendita.MetodoPagamento, bodyFont)));
                    table.AddCell(new PdfPCell(new Phrase($"{vendita.Totale:C}", bodyFont)));

                    totaleVendite += vendita.Totale;
                }

                document.Add(table);
                document.Add(new Paragraph(" "));
                document.Add(new Paragraph($"Totale Vendite: {totaleVendite:C}", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14)));

                document.Close();
            }

            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
        }

        private void Annulla_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
