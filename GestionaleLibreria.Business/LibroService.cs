using GestionaleLibreria.Data.Logging;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestionaleLibreria.Business.Services
{
    public class LibroService
    {
        private readonly ILibroRepository _libroRepository;
        private readonly IMagazzinoRepository _magazzinoRepository;
        public  static string nomeClasse = nameof(LibroService);

        public LibroService(ILibroRepository libroRepository, IMagazzinoRepository magazzinoRepository)
        {
            _libroRepository = libroRepository;
            _magazzinoRepository = magazzinoRepository;
        }

        public List<Libro> GetAllLibri()
        {
            //return _libroRepository.GetAllLibri();
            using (var context = new LibraryContext())
            {
                return context.Libri
                    .Include("Categoria")
                    .Include("LibriMagazzino")
                    .ToList();
            }
        }

        public void AggiungiLibro(Libro libro)
        {
            string nameMetodo = nameof(AggiungiLibro);
            try
            {
                Logger.LogInfo(nomeClasse, nameMetodo , $"Inizio aggiunta libro: {libro.Titolo}");
                _libroRepository.AddLibro(libro);  
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
                    Logger.LogInfo(nomeClasse, nameMetodo, $"Libro aggiunto con successo: {libro.Titolo}");
                
            }
            catch (Exception ex)
            {
                Logger.LogError(nomeClasse, nameMetodo, ex);
                throw;
            }
           
        }



        public void ModificaLibro(Libro libro)
        {
            string nameMetodo = nameof(ModificaLibro);
            try
            {
                Logger.LogInfo(nomeClasse, nameMetodo, $"Modifica libro: {libro.Titolo}");
                _libroRepository.UpdateLibro(libro);
                Logger.LogInfo(nomeClasse, nameMetodo, $"Libro modificato con successo: {libro.Titolo}");

            }
            catch(Exception ex)
            {
                Logger.LogError(nomeClasse, nameMetodo, ex);
                throw;
            }
          
        }

        public List<Libro> CercaLibri(string filtro)
        {
            string nomeMetodo = nameof(CercaLibri);
            try
            {
                Logger.LogInfo(nomeClasse, nomeMetodo, $"Ricerca libri con filtro: {filtro}");

                if (string.IsNullOrWhiteSpace(filtro))
                {
                    return _libroRepository.GetAllLibri(); 
                }

                var libriFiltrati = _libroRepository.GetAllLibri().Where(libro =>
                    libro.Titolo.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    libro.Autore.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    libro.CasaEditrice.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    libro.ISBN.Equals(filtro, StringComparison.OrdinalIgnoreCase) ||
                    (libro.Categoria != null && libro.Categoria.Nome.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0) // Filtro per categoria
            
                ).ToList();

                Logger.LogInfo(nomeClasse, nomeMetodo, $"Trovati {libriFiltrati.Count} libri corrispondenti al filtro.");
                return libriFiltrati;
            }
            catch (Exception ex)
            {
                Logger.LogError(nomeClasse, nomeMetodo, ex);
                throw;
            }
        }


        public void EliminaLibro(int id)
        {
            _libroRepository.DeleteLibro(id);
        }
    }
}