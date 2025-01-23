using System.ComponentModel.DataAnnotations;

namespace GestionaleLibreria.Data.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Cognome { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Telefono { get; set; }
    }
}
