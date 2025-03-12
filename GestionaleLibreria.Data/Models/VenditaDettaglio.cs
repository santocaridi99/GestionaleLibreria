using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionaleLibreria.Data.Models
{
    public class VenditaDettaglio
    {
        [Key]
        public int Id { get; set; }

        public int VenditaId { get; set; }
        [ForeignKey("VenditaId")]
        public virtual Vendita Vendita { get; set; }

        public int LibroId { get; set; }
        [ForeignKey("LibroId")]
        public virtual Libro Libro { get; set; }

        public int Quantita { get; set; }

        public decimal PrezzoOriginale { get; set; } // Prezzo base senza sconto
        public decimal PrezzoUnitario { get; set; } // Prezzo scontato

        public decimal Totale => PrezzoUnitario * Quantita; 
    }
}
