using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionaleLibreria.Data.Models
{
    public class Ebook : Libro
    {
        public string Formato { get; set; } // Es. PDF, EPUB
        public double DimensioneFile { get; set; } // In MB
        public override string Tipo => "Ebook";
        public override decimal CalcolaPrezzo() => (Prezzo - (Prezzo * (decimal)Sconto))/2;
    }
}
