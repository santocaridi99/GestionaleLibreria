using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;

namespace GestionaleLibreria.WPF.FormReportistica
{
    public partial class ReportMagazzinoWindow : Window
    {
        private readonly MagazzinoService _magazzinoService;

        public ReportMagazzinoWindow(MagazzinoService magazzinoService)
        {
            InitializeComponent();
            _magazzinoService = magazzinoService;
            CaricaDati();
        }

        private void CaricaDati()
        {
            MagazzinoDataGrid.ItemsSource = _magazzinoService.GetReportMagazzino();
        }

        private void EsportaPDF_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string filePath = Path.Combine(desktopPath, "ReportMagazzino.pdf");

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    Document document = new Document(PageSize.A4);
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                    document.Add(new Paragraph("📦 Report Magazzino", titleFont));
                    document.Add(new Paragraph(" "));

                    PdfPTable table = new PdfPTable(3);
                    table.WidthPercentage = 100;
                    table.SetWidths(new float[] { 40, 30, 30 });

                    table.AddCell(new PdfPCell(new Phrase("Titolo", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    table.AddCell(new PdfPCell(new Phrase("Autore", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    table.AddCell(new PdfPCell(new Phrase("Quantità", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12))) { BackgroundColor = BaseColor.LIGHT_GRAY });

                    foreach (var item in _magazzinoService.GetReportMagazzino())
                    {
                        table.AddCell(new PdfPCell(new Phrase(item.Libro.Titolo)));
                        table.AddCell(new PdfPCell(new Phrase(item.Libro.Autore)));
                        table.AddCell(new PdfPCell(new Phrase(item.Quantita.ToString())));
                    }

                    document.Add(table);
                    document.Close();
                }

                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });

                MessageBox.Show("Report esportato con successo!", "Successo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errore durante l'esportazione del PDF: {ex.Message}", "Errore", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Chiudi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
