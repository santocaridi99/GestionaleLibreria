using GestionaleLibreria.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionaleLibreria.Data
{
    public interface ILibroRepository
    {
        List<Libro> GetAllLibri();
        void AddLibro(Libro libro);
        void UpdateLibro(Libro libro);
        void DeleteLibro(int id);
    }

    public class LibroRepository : ILibroRepository
    {
        private readonly LibraryContext _context;

        public LibroRepository()
        {
            _context = new LibraryContext();
        }

        public List<Libro> GetAllLibri()
        {
            return _context.Libri.ToList();
        }

        public void AddLibro(Libro libro)
        {
            _context.Libri.Add(libro);
            _context.SaveChanges();
        }

        public void UpdateLibro(Libro libro)
        {
            var existing = _context.Libri.FirstOrDefault(l => l.Id == libro.Id);
            if (existing != null)
            {
                existing.Titolo = libro.Titolo;
                existing.Autore = libro.Autore;
                existing.ISBN = libro.ISBN;
                existing.Prezzo = libro.Prezzo;
                _context.SaveChanges();
            }
        }

        public void DeleteLibro(int id)
        {
            var libro = _context.Libri.FirstOrDefault(l => l.Id == id);
            if (libro != null)
            {
                _context.Libri.Remove(libro);
                _context.SaveChanges();
            }
        }
    }
}
