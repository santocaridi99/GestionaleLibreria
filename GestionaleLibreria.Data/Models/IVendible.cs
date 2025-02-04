using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionaleLibreria.Data.Models
{
    //interfaccia per gli oggetti che possono essere venduti in modo che una funziona possa lavorare
    //con qualsiasi tipo di libro (base, ebook, audiobook) senza preoccuparsi dei dettagli interni.
    public interface IVendibile
    {
        decimal CalcolaPrezzo();

    }
}
