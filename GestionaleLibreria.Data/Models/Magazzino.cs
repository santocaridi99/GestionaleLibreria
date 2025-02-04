using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionaleLibreria.Data.Models
{
    public class Magazzino
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "Magazzino Principale";
        // Un magazzino contiene molti record di LibroMagazzino
        public virtual ICollection<LibroMagazzino> LibriMagazzino { get; set; } = new List<LibroMagazzino>();
        public List<LibroMagazzino> OttieniLibriFisici() => new List<LibroMagazzino>(LibriMagazzino);
    }
}
