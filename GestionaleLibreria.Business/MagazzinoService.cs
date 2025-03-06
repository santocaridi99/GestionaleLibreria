using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace GestionaleLibreria.Business.Services
{
    public class MagazzinoService
    {
        private readonly IMagazzinoRepository _magazzinoRepository;

        public MagazzinoService(IMagazzinoRepository magazzinoRepository)
        {
            _magazzinoRepository = magazzinoRepository;
        }

        public List<LibroMagazzino> GetLibriMagazzino()
        {
            return _magazzinoRepository.GetAllLibriInMagazzino();
        }

        public void AggiungiLibroFisico(Libro libro, int quantita)
        {
            var libroMagazzino = _magazzinoRepository.GetLibroMagazzinoById(libro.Id);
            if (libroMagazzino != null)
            {
                libroMagazzino.AggiungiScorte(quantita);
                _magazzinoRepository.AggiornaLibroMagazzino(libroMagazzino);
            }
            else
            {
                _magazzinoRepository.AggiungiLibroMagazzino(new LibroMagazzino(libro, quantita));
            }
        }

        public void RimuoviLibroFisico(int libroId, int quantita)
        {
            var libroMagazzino = _magazzinoRepository.GetLibroMagazzinoById(libroId);
            if (libroMagazzino != null && libroMagazzino.Quantita >= quantita)
            {
                libroMagazzino.RimuoviScorte(quantita);
                _magazzinoRepository.AggiornaLibroMagazzino(libroMagazzino);
            }
        }

        public int OttieniQuantitaLibro(int libroId)
        {
            var libroMagazzino = _magazzinoRepository.GetLibroMagazzinoById(libroId);
            return libroMagazzino != null ? libroMagazzino.Quantita : 0;
        }
    }
}
