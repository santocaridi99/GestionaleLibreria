using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionaleLibreria.Data.Models
{
    public class Ebook : Libro
    {
        public string Formato { get; set; } // Es. PDF, EPUB
        public double DimensioneFile { get; set; } // In MB
        public double Sconto { get; set; } // Percentuale di sconto, ad esempio 0.1 = 10%
        public override decimal CalcolaPrezzo() => Prezzo - (Prezzo * (decimal)Sconto);
    }
}
