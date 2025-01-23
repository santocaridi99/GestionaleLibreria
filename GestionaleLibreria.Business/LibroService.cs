using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace GestionaleLibreria.Business.Services
{
    public class LibroService
    {
        private readonly LibraryContext _context;

        public LibroService()
        {
            _context = new LibraryContext();
        }

        public List<Libro> GetAllLibri()
        {
            return _context.Libri.ToList();
        }

        public void AggiungiLibro(Libro libro)
        {
            _context.Libri.Add(libro);
            _context.SaveChanges();
        }

        public void ModificaLibro(Libro libro)
        {
            var existing = _context.Libri.Find(libro.Id);
            if (existing != null)
            {
                existing.Titolo = libro.Titolo;
                existing.Autore = libro.Autore;
                existing.Prezzo = libro.Prezzo;
                existing.Quantita = libro.Quantita;
                _context.SaveChanges();
            }
        }

        public void EliminaLibro(int id)
        {
            var libro = _context.Libri.Find(id);
            if (libro != null)
            {
                _context.Libri.Remove(libro);
                _context.SaveChanges();
            }
        }
    }
}
