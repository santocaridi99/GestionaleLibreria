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

        public decimal PrezzoUnitario { get; set; }

        public decimal Totale { get { return Quantita * PrezzoUnitario; } }
    }
}
