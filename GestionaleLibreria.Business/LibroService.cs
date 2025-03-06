using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace GestionaleLibreria.Business.Services
{
    public class LibroService
    {
        private readonly ILibroRepository _libroRepository;
        private readonly MagazzinoService _magazzinoService;

        // Ora il MagazzinoService viene iniettato correttamente.
        public LibroService(ILibroRepository libroRepository, MagazzinoService magazzinoService)
        {
            _libroRepository = libroRepository;
            _magazzinoService = magazzinoService;
        }

        public List<Libro> GetAllLibri()
        {
            return _libroRepository.GetAllLibri();
        }

        public void AggiungiLibro(Libro libro)
        {
            _libroRepository.AddLibro(libro);

            // Se il libro non è un Ebook o Audiobook, lo aggiunge automaticamente al magazzino.
            if (!(libro is Ebook) && !(libro is Audiobook))
            {
                _magazzinoService.AggiungiLibroFisico(libro, 0);
            }
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