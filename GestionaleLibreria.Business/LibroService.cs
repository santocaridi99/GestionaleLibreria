using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace GestionaleLibreria.Business.Services
{
    public class LibroService
    {
        private readonly ILibroRepository _libroRepository;

        public LibroService(ILibroRepository libroRepository)
        {
            _libroRepository = libroRepository;
        }

        public List<Libro> GetAllLibri()
        {
            return _libroRepository.GetAllLibri();
        }

        public void AggiungiLibro(Libro libro)
        {
            _libroRepository.AddLibro(libro);
        }

        public void ModificaLibro(Libro libro)
        {
            _libroRepository.UpdateLibro(libro);
        }

        public void EliminaLibro(int id)
        {
            _libroRepository.DeleteLibro(id);
        }
    }
}
