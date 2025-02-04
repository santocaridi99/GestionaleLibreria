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
        private readonly List<Libro> _libri = new List<Libro>();

        public List<Libro> GetAllLibri()
        {
            return _libri;
        }

        public void AddLibro(Libro libro)
        {
            libro.Id = _libri.Count > 0 ? _libri.Max(l => l.Id) + 1 : 1;
            _libri.Add(libro);
        }

        public void UpdateLibro(Libro libro)
        {
            var existing = _libri.FirstOrDefault(l => l.Id == libro.Id);
            if (existing != null)
            {
                existing.Titolo = libro.Titolo;
                existing.Autore = libro.Autore;
                existing.ISBN = libro.ISBN;
                existing.Prezzo = libro.Prezzo;
            }
        }

        public void DeleteLibro(int id)
        {
            var libro = _libri.FirstOrDefault(l => l.Id == id);
            if (libro != null)
                _libri.Remove(libro);
        }
    }
}
