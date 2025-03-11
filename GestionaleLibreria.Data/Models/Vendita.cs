using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;


namespace GestionaleLibreria.Data.Models
{
    public class Vendita
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Cliente")]
        public int? ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public DateTime DataVendita { get; set; }

        public int QuantitaVenduta { get; set; }
        public string MetodoPagamento { get; set; }

        public decimal Totale { get; set; }
        public virtual ICollection<VenditaDettaglio> DettagliVendita { get; set; } = new List<VenditaDettaglio>();
    }
}
