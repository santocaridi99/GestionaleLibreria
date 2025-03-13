using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace GestionaleLibreria.Business.Services
{
    public class MagazzinoService
    {
        private readonly IMagazzinoRepository _magazzinoRepository;
        private readonly ILibroRepository _libroRepository;

        public MagazzinoService(IMagazzinoRepository magazzinoRepository, ILibroRepository libroRepository) 
        {
            _magazzinoRepository = magazzinoRepository;
            _libroRepository = libroRepository; 
        }

        public List<LibroMagazzino> GetLibriMagazzino()
        {
            return _magazzinoRepository.GetAllLibriInMagazzino();
        }


        public List<LibroMagazzino> GetReportMagazzino()
        {
            return _magazzinoRepository.GetAllLibriInMagazzino()
                .OrderByDescending(lm => lm.Quantita)
                .ToList();
        }


        public void AggiungiScorte(int libroId, int quantita)
        {
            var libroMagazzino = _magazzinoRepository.GetLibroMagazzinoById(libroId);
            if (libroMagazzino != null)
            {
                libroMagazzino.AggiungiScorte(quantita);
                _magazzinoRepository.AggiornaLibroMagazzino(libroMagazzino);
            }
            else
            {
                var libro = _libroRepository.GetLibroById(libroId);
                if (libro != null)
                {
                    var nuovoLibroMagazzino = new LibroMagazzino(libro, quantita);
                    _magazzinoRepository.AggiungiLibroMagazzino(nuovoLibroMagazzino);
                }
            }
        }

        public bool RimuoviScorte(int libroId, int quantita)
        {
            var libroMagazzino = _magazzinoRepository.GetLibroMagazzinoById(libroId);
            if (libroMagazzino != null && libroMagazzino.Quantita >= quantita)
            {
                libroMagazzino.RimuoviScorte(quantita);
                _magazzinoRepository.AggiornaLibroMagazzino(libroMagazzino);
                return true;
            }
            return false;
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
