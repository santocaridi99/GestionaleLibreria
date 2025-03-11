using System.ComponentModel.DataAnnotations;

namespace GestionaleLibreria.Data.Models
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il nome è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Il nome non può superare i 50 caratteri.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Il cognome è obbligatorio.")]
        [MaxLength(50, ErrorMessage = "Il cognome non può superare i 50 caratteri.")]
        public string Cognome { get; set; }

        [Required(ErrorMessage = "L'email è obbligatoria.")]
        [EmailAddress(ErrorMessage = "Inserisci un'email valida.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Il numero di telefono è obbligatorio.")]
        [RegularExpression(@"^\d{8,15}$", ErrorMessage = "Il telefono deve contenere solo numeri (8-15 cifre).")]
        public string Telefono { get; set; }
    }
}
