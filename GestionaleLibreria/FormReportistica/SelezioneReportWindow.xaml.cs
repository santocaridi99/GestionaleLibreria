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
            GraficoVendite.Source = DisegnaGrafico(GetDatiVendite(), "Vendite Mensili");
           

            GraficoLibriVenduti.Source = DisegnaGrafico(GetDatiLibri(), "Libri più venduti");
            GraficoLibriVendutiTorta.Source = DisegnaGraficoTorta(GetDatiLibri(), "Distribuzione Libri");

            GraficoClientiAttivi.Source = DisegnaGrafico(GetDatiClienti(), "Clienti più attivi");
           
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


        private List<DatiGrafico> GetDatiVendite()
        {
            var vendite = _venditaService.GetVenditePerPeriodo(DateTime.Now.AddMonths(-6), DateTime.Now);
            return vendite.GroupBy(v => v.DataVendita.Month)
                .Select(g => new DatiGrafico { Nome = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key), Totale = g.Sum(v => v.Totale) })
                .ToList();
        }

        private List<DatiGrafico> GetDatiLibri()
        {
            return _venditaService.GetVendite()
                .SelectMany(v => v.DettagliVendita)
                .GroupBy(d => d.Libro.Titolo)
                .Select(g => new DatiGrafico { Nome = g.Key, Totale = g.Sum(d => d.Quantita) })
                .OrderByDescending(g => g.Totale)
                .Take(5)
                .ToList();
        }

        private List<DatiGrafico> GetDatiClienti()
        {
            return _venditaService.GetVendite()
                .Where(v => v.Cliente != null)
                .GroupBy(v => v.Cliente.Nome + " " + v.Cliente.Cognome)
                .Select(g => new DatiGrafico { Nome = g.Key, Totale = g.Count() })
                .OrderByDescending(g => g.Totale)
                .Take(5)
                .ToList();
        }



        private BitmapSource DisegnaGrafico(List<DatiGrafico> dati, string titolo)
        {
            int width = 500, height = 300;
            DrawingVisual dv = new DrawingVisual();

            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, new Pen(Brushes.Black, 2), new Rect(0, 0, width, height));

                if (dati == null || dati.Count == 0)
                {
                    FormattedText messaggio = new FormattedText(
                        "Nessun dato disponibile",
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        16,
                        Brushes.Red
                    );

                    dc.DrawText(messaggio, new Point(width / 4, height / 2));
                    return RenderVisual(dv, width, height);
                }

                double maxVal = (double)dati.Max(d => d.Totale);
                double scaleFactor = (height - 50) / maxVal;
                double barWidth = width / (dati.Count + 1);

                for (int i = 0; i < dati.Count; i++)
                {
                    double x = (i + 1) * barWidth;
                    double barHeight = (double)dati[i].Totale * scaleFactor;

                    dc.DrawRectangle(Brushes.DodgerBlue, null, new Rect(x, height - barHeight, barWidth - 15, barHeight));

                    FormattedText etichetta = new FormattedText(
                        dati[i].Nome,
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        12,
                        Brushes.Black
                    );

                    dc.DrawText(etichetta, new Point(x, height - 20));
                }
            }

            return RenderVisual(dv, width, height);
        }

        private BitmapSource DisegnaGraficoTorta(List<DatiGrafico> dati, string titolo)
        {
            int width = 400, height = 300;
            DrawingVisual dv = new DrawingVisual();

            using (DrawingContext dc = dv.RenderOpen())
            {
                dc.DrawRectangle(Brushes.White, new Pen(Brushes.Black, 2), new Rect(0, 0, width, height));

                if (dati == null || dati.Count == 0)
                {
                    FormattedText messaggio = new FormattedText(
                        "Nessun dato disponibile",
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface("Arial"),
                        16,
                        Brushes.Red
                    );

                    dc.DrawText(messaggio, new Point(width / 4, height / 2));
                    return RenderVisual(dv, width, height);
                }

                double totale = dati.Sum(d => (double)d.Totale);
                double angoloIniziale = 0;
                Brush[] colori = { Brushes.Blue, Brushes.Red, Brushes.Green, Brushes.Purple, Brushes.Orange };
                int coloriIndex = 0;

                foreach (var item in dati)
                {
                    double percentuale = (double)item.Totale / totale;
                    double angoloFinale = angoloIniziale + (percentuale * 360);
                    Brush colore = colori[coloriIndex % colori.Length];
                    coloriIndex++;

                    // Disegno il settore della torta
                    dc.DrawGeometry(colore, new Pen(Brushes.Black, 1), CreatePieSliceGeometry(width / 2, height / 2, 100, angoloIniziale, angoloFinale));

                    angoloIniziale = angoloFinale;
                }
            }

            return RenderVisual(dv, width, height);
        }

        private BitmapSource RenderVisual(DrawingVisual dv, int width, int height)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(dv);
            return bitmap;
        }

        private Geometry CreatePieSliceGeometry(double centerX, double centerY, double radius, double startAngle, double endAngle)
        {
            StreamGeometry geometry = new StreamGeometry();
            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(new Point(centerX, centerY), true, true);
                ctx.LineTo(new Point(centerX + radius * Math.Cos(startAngle * Math.PI / 180), centerY - radius * Math.Sin(startAngle * Math.PI / 180)), true, false);
                ctx.ArcTo(new Point(centerX + radius * Math.Cos(endAngle * Math.PI / 180), centerY - radius * Math.Sin(endAngle * Math.PI / 180)), new Size(radius, radius), 0, endAngle - startAngle > 180, SweepDirection.Counterclockwise, true, false);
                ctx.LineTo(new Point(centerX, centerY), true, false);
            }
            return geometry;
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


