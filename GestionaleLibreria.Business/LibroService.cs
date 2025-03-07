using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace GestionaleLibreria.Business.Services
{
    public class LibroService
    {
        private readonly ILibroRepository _libroRepository;
        private readonly IMagazzinoRepository _magazzinoRepository;

        public LibroService(ILibroRepository libroRepository, IMagazzinoRepository magazzinoRepository)
        {
            _libroRepository = libroRepository;
            _magazzinoRepository = magazzinoRepository;
        }

        public List<Libro> GetAllLibri()
        {
            return _libroRepository.GetAllLibri();
        }

        public void AggiungiLibro(Libro libro)
        {
            _libroRepository.AddLibro(libro);

            // Se il libro è cartaceo, lo aggiunge in magazzino con quantità zero
            if (!(libro is Ebook) && !(libro is Audiobook))
            {
                // Verifica se esiste un magazzino, altrimenti ne crea uno
                var magazzino = _magazzinoRepository.GetMagazzinoPrincipale();
                if (magazzino == null)
                {
                    magazzino = new Magazzino { Nome = "Magazzino Principale" };
                    _magazzinoRepository.AggiungiMagazzino(magazzino);
                }

                // Ora crea il record in LibroMagazzino con il MagazzinoId corretto
                var libroMagazzino = new LibroMagazzino
                {
                    LibroId = libro.Id,
                    MagazzinoId = magazzino.Id, // Assicuriamoci che il MagazzinoId sia valido
                    Quantita = 0
                };
                _magazzinoRepository.AggiungiLibroMagazzino(libroMagazzino);
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