using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        [Required, StringLength(13)] // Max 13 caratteri
        [Index(IsUnique = true)] // Indice univoco
        public string ISBN { get; set; }

        [Required]
        public string CasaEditrice { get; set; } = string.Empty;
        [Required]
        public decimal Prezzo { get; set; }



        public double Sconto { get; set; } = 0;

        public virtual string Tipo => "Libro Cartaceo";


        public int QuantitaMagazzino => LibriMagazzino?.Sum(lm => lm.Quantita) ?? 0;
        // Relazione: un libro può essere presente in uno o più record di LibroMagazzino
        public virtual ICollection<LibroMagazzino> LibriMagazzino { get; set; } = new List<LibroMagazzino>();

        // Relazione con Categoria
        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }

        // Metodo virtual per consentire l'override nelle classi derivate
        public virtual decimal CalcolaPrezzo() => Prezzo - (Prezzo * (decimal)Sconto);


    }


}
