using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionaleLibreria.Data.Models
{
    public class Audiobook : Libro
    {
        public double DurataOre { get; set; }
        public string Narratore { get; set; } = string.Empty;

        public override decimal CalcolaPrezzo()
        {
            // Esempio: gli audiolibri hanno un costo aggiuntivo di 5€
            return Prezzo + 5.00m;
        }
    }
}
