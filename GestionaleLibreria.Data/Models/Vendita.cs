using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;


namespace GestionaleLibreria.Data.Models
{
    public class Vendita
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Libro")]
        public int LibroId { get; set; }
        public Libro Libro { get; set; }

        [ForeignKey("Cliente")]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public DateTime DataVendita { get; set; }

        public int QuantitaVenduta { get; set; }

        public decimal Totale { get; set; }
    }
}
