using GestionaleLibreria.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace GestionaleLibreria.Data
{
    public interface IMagazzinoRepository
    {
        List<LibroMagazzino> GetAllLibriInMagazzino();
        LibroMagazzino GetLibroMagazzinoById(int libroId);
        void AggiungiLibroMagazzino(LibroMagazzino libroMagazzino);
        void AggiornaLibroMagazzino(LibroMagazzino libroMagazzino);
        void RimuoviLibroMagazzino(int libroId);
        void SaveChanges();

       
        Magazzino GetMagazzinoPrincipale();
        void AggiungiMagazzino(Magazzino magazzino);
    }


    public class MagazzinoRepository : IMagazzinoRepository
    {
        private readonly LibraryContext _context;

        public MagazzinoRepository(LibraryContext context)
        {
            _context = context;
        }

        public List<LibroMagazzino> GetAllLibriInMagazzino()
        {
            return _context.LibriMagazzino.Include("Libro").ToList();
        }


        public LibroMagazzino GetLibroMagazzinoById(int libroId)
        {
            return _context.LibriMagazzino.FirstOrDefault(lm => lm.LibroId == libroId);
        }

        public void AggiungiLibroMagazzino(LibroMagazzino libroMagazzino)
        {
            _context.LibriMagazzino.Add(libroMagazzino);
            SaveChanges();
        }

        public void AggiornaLibroMagazzino(LibroMagazzino libroMagazzino)
        {
            var entity = _context.LibriMagazzino.FirstOrDefault(lm => lm.Id == libroMagazzino.Id);
            if (entity != null)
            {
                _context.Entry(entity).CurrentValues.SetValues(libroMagazzino);
                SaveChanges();
            }
        }

        public Magazzino GetMagazzinoPrincipale()
        {
            return _context.Magazzini.FirstOrDefault(m => m.Nome == "Magazzino Principale");
        }

        public void AggiungiMagazzino(Magazzino magazzino)
        {
            _context.Magazzini.Add(magazzino);
            _context.SaveChanges();
        }



        public void RimuoviLibroMagazzino(int libroId)
        {
            var libroMagazzino = GetLibroMagazzinoById(libroId);
            if (libroMagazzino != null)
            {
                _context.LibriMagazzino.Remove(libroMagazzino);
                SaveChanges();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
