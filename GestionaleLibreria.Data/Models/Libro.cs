using System.ComponentModel.DataAnnotations;

namespace GestionaleLibreria.Data.Models
{
    public class Libro
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Titolo { get; set; }

        [Required]
        public string Autore { get; set; }

        [Required]
        public decimal Prezzo { get; set; }

        public int Quantita { get; set; }

        //poi aggiungere altre caratteristiche
    }
}
