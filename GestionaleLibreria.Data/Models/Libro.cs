using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GestionaleLibreria.Data.Models
{
    public class Libro : IVendibile
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Titolo { get; set; } = string.Empty;

        [Required]
        public string Autore { get; set; } = string.Empty;

        [Required]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        public decimal Prezzo { get; set; }

        [Required]
        public string CasaEditrice { get; set; } = string.Empty;

        public string Tipo => this is Ebook ? "Ebook" : this is Audiobook ? "Audiobook" : "Libro Cartaceo";
    

        // Relazione: un libro può essere presente in uno o più record di LibroMagazzino
        public virtual ICollection<LibroMagazzino> LibriMagazzino { get; set; } = new List<LibroMagazzino>();

        // Metodo virtual per consentire l'override nelle classi derivate
        public virtual decimal CalcolaPrezzo() => Prezzo;
    }


}
