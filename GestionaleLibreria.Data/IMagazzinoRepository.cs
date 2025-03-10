using GestionaleLibreria.Data.Logging;
using GestionaleLibreria.Data.Models;
using System;
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
        private static readonly string NomeClasse = nameof(MagazzinoRepository);

        public MagazzinoRepository(LibraryContext context)
        {
            _context = context;
        }

        public List<LibroMagazzino> GetAllLibriInMagazzino()
        {
            string nomeMetodo = nameof(GetAllLibriInMagazzino);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Recupero di tutti i libri in magazzino.");
                var libriMagazzino = _context.LibriMagazzino.Include("Libro").ToList();
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Recuperati {libriMagazzino.Count} libri in magazzino.");
                return libriMagazzino;
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public LibroMagazzino GetLibroMagazzinoById(int libroId)
        {
            string nomeMetodo = nameof(GetLibroMagazzinoById);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Recupero libro in magazzino con ID: {libroId}");
                var libroMagazzino = _context.LibriMagazzino.FirstOrDefault(lm => lm.LibroId == libroId);

                if (libroMagazzino != null)
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro trovato in magazzino: {libroMagazzino.Libro.Titolo}");
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Nessun libro in magazzino trovato con ID: {libroId}");
                }

                return libroMagazzino;
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public void AggiungiLibroMagazzino(LibroMagazzino libroMagazzino)
        {
            string nomeMetodo = nameof(AggiungiLibroMagazzino);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Aggiunta libro in magazzino)");
                _context.LibriMagazzino.Add(libroMagazzino);
                SaveChanges();
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro aggiunto con successo in magazzino.");
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public void AggiornaLibroMagazzino(LibroMagazzino libroMagazzino)
        {
            string nomeMetodo = nameof(AggiornaLibroMagazzino);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Aggiornamento libro in magazzino ID: {libroMagazzino.Id}");
                var entity = _context.LibriMagazzino.FirstOrDefault(lm => lm.Id == libroMagazzino.Id);
                if (entity != null)
                {
                    _context.Entry(entity).CurrentValues.SetValues(libroMagazzino);
                    SaveChanges();
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro aggiornato in magazzino con successo.");
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro con ID {libroMagazzino.Id} non trovato in magazzino.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public Magazzino GetMagazzinoPrincipale()
        {
            string nomeMetodo = nameof(GetMagazzinoPrincipale);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Recupero del Magazzino Principale.");
                var magazzino = _context.Magazzini.FirstOrDefault(m => m.Nome == "Magazzino Principale");

                if (magazzino != null)
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, "Magazzino Principale trovato.");
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, "Magazzino Principale non trovato.");
                }

                return magazzino;
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public void AggiungiMagazzino(Magazzino magazzino)
        {
            string nomeMetodo = nameof(AggiungiMagazzino);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Aggiunta nuovo magazzino: {magazzino.Nome}");
                _context.Magazzini.Add(magazzino);
                _context.SaveChanges();
                Logger.LogInfo(NomeClasse, nomeMetodo, "Magazzino aggiunto con successo.");
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public void RimuoviLibroMagazzino(int libroId)
        {
            string nomeMetodo = nameof(RimuoviLibroMagazzino);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Tentativo di rimozione libro ID: {libroId} dal magazzino.");

                var libroMagazzino = GetLibroMagazzinoById(libroId);
                if (libroMagazzino != null)
                {
                    _context.LibriMagazzino.Remove(libroMagazzino);
                    SaveChanges();
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro {libroMagazzino.Libro.Titolo} rimosso dal magazzino.");
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro con ID {libroId} non trovato in magazzino per la rimozione.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public void SaveChanges()
        {
            string nomeMetodo = nameof(SaveChanges);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Salvataggio delle modifiche nel database.");
                _context.SaveChanges();
                Logger.LogInfo(NomeClasse, nomeMetodo, "Salvataggio completato con successo.");
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }
    }
}
