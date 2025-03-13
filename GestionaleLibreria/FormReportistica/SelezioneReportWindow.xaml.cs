using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using GestionaleLibreria.Business;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using Microsoft.Xaml.Behaviors.Media;

namespace GestionaleLibreria.WPF.FormReportistica
{
    public partial class SelezioneReportWindow : Window
    {
        private readonly VenditaService _venditaService;
        private readonly MagazzinoService _magazzinoService;

        public SelezioneReportWindow(VenditaService venditaService, MagazzinoService magazzinoService)
        {
            InitializeComponent();
            _venditaService = venditaService;
            _magazzinoService = magazzinoService;
            GeneraGrafici();
        }
        private void GeneraGrafici()
        {
            GraficoVendite.Source = GeneraGraficoVendite();
            GraficoLibriVenduti.Source = GeneraGraficoLibri();
            GraficoClientiAttivi.Source = GeneraGraficoClienti();
        }

        private void ApriReportMagazzino_Click(object sender, RoutedEventArgs e)
        {
            new ReportMagazzinoWindow(_magazzinoService).ShowDialog();
        }

        private void ApriReportVendite_Click(object sender, RoutedEventArgs e)
        {
            new ReportVenditeWindow(_venditaService).ShowDialog();
        }

        private void ApriReportClienti_Click(object sender, RoutedEventArgs e)
        {
            
            var clienteService = new ClienteService(new ClienteRepository());
           

            new ReportClientiWindow(clienteService, _venditaService).ShowDialog();
        }

        private void GeneraGrafici_Click(object sender, RoutedEventArgs e)
        {
            GeneraGrafici();
        }

        private BitmapSource GeneraGraficoVendite()
        {
            var vendite = _venditaService.GetVenditePerPeriodo(DateTime.Now.AddMonths(-6), DateTime.Now);
            var datiGrafico = vendite
                .GroupBy(v => v.DataVendita.Month)
                .Select(g => new DatiGrafico { Mese = g.Key, Totale = (decimal)(double)g.Sum(v => v.Totale) })
                .OrderBy(g => g.Mese)
                .ToList();

            return DisegnaGrafico(datiGrafico, "Vendite");
        }

        private BitmapSource GeneraGraficoLibri()
        {
            var libriVenduti = _venditaService.GetVendite()
                .SelectMany(v => v.DettagliVendita)
                .GroupBy(d => d.Libro.Titolo)
                .Select(g => new DatiGrafico { Mese = 0, Totale = (decimal)g.Sum(d => (double)d.Quantita), Nome = g.Key })
                .OrderByDescending(g => g.Totale)
                .Take(5)
                .ToList();

            return DisegnaGrafico(libriVenduti, "Libri Venduti");
        }

        private BitmapSource GeneraGraficoClienti()
        {
            var clienti = _venditaService.GetVendite()
                .Where(v => v.Cliente != null)
                .GroupBy(v => v.Cliente.Nome + " " + v.Cliente.Cognome)
                .Select(g => new DatiGrafico { Mese = 0, Totale = g.Count(), Nome = g.Key })
                .OrderByDescending(g => g.Totale)
                .Take(5)
                .ToList();

            return DisegnaGrafico(clienti, "Clienti Attivi");
        }

        private BitmapSource DisegnaGrafico(List<DatiGrafico> dati, string titolo)
        {
            int width = 400, height = 250;
            DrawingVisual dv = new DrawingVisual();

            // Se dati è vuoto, ritorna un'immagine vuota con messaggio
            if (dati == null || dati.Count == 0)
            {
                using (DrawingContext dc = dv.RenderOpen())
                {
                    dc.DrawRectangle(Brushes.White, new Pen(Brushes.Black, 2), new Rect(0, 0, width, height));

                    FormattedText messaggio = new FormattedText(
                        "Nessun dato disponibile",
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        16,
                        Brushes.Red
                    );

                    dc.DrawText(messaggio, new Point(width / 4, height / 2));
                }

                RenderTargetBitmap bitmapVuoto = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
                bitmapVuoto.Render(dv);
                return bitmapVuoto;
            }

            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, new Pen(Brushes.Black, 2), new Rect(0, 0, width, height));

                double maxVal = (double)dati.Max(d => d.Totale);
                if (maxVal == 0) maxVal = 1; // Evita divisione per zero

                double scaleFactor = (height - 50) / maxVal;
                double barWidth = width / (dati.Count + 1);

                for (int i = 0; i < dati.Count; i++)
                {
                    double x = (i + 1) * barWidth;
                    double barHeight = (double)dati[i].Totale * scaleFactor;

                    dc.DrawRectangle(Brushes.Blue, null, new Rect(x, height - barHeight, barWidth - 10, barHeight));

                    // Aggiungi etichetta sotto ogni barra
                    FormattedText mese = new FormattedText(
                        dati[i].Nome ?? CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dati[i].Mese),
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        12,
                        Brushes.Black
                    );

                    dc.DrawText(mese, new Point(x, height - 20));
                }
            }

            RenderTargetBitmap bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
            return bitmap;
        }

    }



    public class DatiGrafico
    {
        public string Nome { get; set; }
        public int Mese { get; set; }
        public decimal Totale { get; set; }
        public int Quantita { get; set; }
        
    }



}


