using GestionaleLibreria.Data.Models;
using GestionaleLibreria.Data.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private static readonly string NomeClasse = nameof(LibroRepository);

        public LibroRepository()
        {
            _context = new LibraryContext();
        }

        public List<Libro> GetAllLibri()
        {
            string nomeMetodo = nameof(GetAllLibri);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Recupero di tutti i libri dal database.");
                var libri = _context.Libri.ToList();
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Recuperati {libri.Count} libri.");
                return libri;
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public void AddLibro(Libro libro)
        {
            string nomeMetodo = nameof(AddLibro);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Tentativo di aggiunta libro: {libro.Titolo} (ISBN: {libro.ISBN})");

                if (_context.Libri.Any(l => l.ISBN == libro.ISBN))
                {
                    string errore = $"Esiste già un libro con ISBN: {libro.ISBN}";
                    Logger.LogError(NomeClasse, nomeMetodo, new Exception(errore));
                    throw new Exception(errore);
                }

                _context.Libri.Add(libro);
                _context.SaveChanges();
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro aggiunto con successo: {libro.Titolo}");
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public void UpdateLibro(Libro libro)
        {
            string nomeMetodo = nameof(UpdateLibro);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Tentativo di aggiornamento libro ID: {libro.Id}");

                var existing = _context.Libri.FirstOrDefault(l => l.Id == libro.Id);
                if (existing != null)
                {
                    // **Aggiorniamo i campi comuni**
                    existing.Titolo = libro.Titolo;
                    existing.Autore = libro.Autore;
                    existing.ISBN = libro.ISBN;
                    existing.Prezzo = libro.Prezzo;
                    existing.Sconto = libro.Sconto;

                    //  campi specifici per Ebook**
                    if (libro is Ebook ebook && existing is Ebook existingEbook)
                    {
                        existingEbook.Formato = ebook.Formato;
                        existingEbook.DimensioneFile = ebook.DimensioneFile;
                    }
                    // campi specifici per Audiobook**
                    else if (libro is Audiobook audiobook && existing is Audiobook existingAudiobook)
                    {
                        existingAudiobook.DurataOre = audiobook.DurataOre;
                        existingAudiobook.Narratore = audiobook.Narratore;
                    }

                    _context.SaveChanges();
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro aggiornato con successo: {libro.Titolo}");
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro con ID {libro.Id} non trovato per l'aggiornamento.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }


        public void DeleteLibro(int id)
        {
            string nomeMetodo = nameof(DeleteLibro);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Tentativo di eliminazione libro con ID: {id}");

                var libro = _context.Libri.FirstOrDefault(l => l.Id == id);
                if (libro != null)
                {
                    if (libro.QuantitaMagazzino > 0)
                    {
                        string messaggio = $"Impossibile eliminare il libro '{libro.Titolo}' con quantità in magazzino.";
                        Logger.LogInfo(NomeClasse, nomeMetodo, messaggio);
                        MessageBox.Show(messaggio, "Errore Eliminazione", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        _context.Libri.Remove(libro);
                        _context.SaveChanges();
                        Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro eliminato con successo: {libro.Titolo}");
                    }
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro con ID {id} non trovato per l'eliminazione.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public Libro GetLibroById(int id)
        {
            string nomeMetodo = nameof(GetLibroById);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Recupero libro con ID: {id}");
                var libro = _context.Libri.FirstOrDefault(l => l.Id == id);

                if (libro != null)
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro trovato: {libro.Titolo} (ISBN: {libro.ISBN})");
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Libro con ID {id} non trovato.");
                }

                return libro;
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }
    }
}
