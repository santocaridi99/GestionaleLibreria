using GestionaleLibreria.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestionaleLibreria.Data
{
    public interface ILibroRepository
    {
        List<Libro> GetAllLibri();
        void AddLibro(Libro libro);
        void UpdateLibro(Libro libro);
        void DeleteLibro(int id);
        Libro GetLibroById(int id);
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
            if (_context.Libri.Any(l => l.ISBN == libro.ISBN))
            {
                throw new Exception($"Esiste già un libro con ISBN: {libro.ISBN}");
            }
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
                if(libro.QuantitaMagazzino > 0)
                {
                   
                    MessageBox.Show("Impossibile eliminare un libro con quantità in magazzino, gestisci in magazzino");
                }
                else
                {
                    _context.Libri.Remove(libro);
                    _context.SaveChanges();
                }
               
              
            }
        }

        public Libro GetLibroById(int id)
        {
            return _context.Libri.FirstOrDefault(l => l.Id == id);
        }

    }
}
