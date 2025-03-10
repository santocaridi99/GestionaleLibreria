﻿using GestionaleLibreria.Data.Logging;
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
            return _libroRepository.GetAllLibri();
        }

        public void AggiungiLibro(Libro libro)
        {
            string nameMetodo = nameof(AggiungiLibro);
            try
            {
                Logger.LogInfo(nomeClasse, nameMetodo , $"Inizio aggiunta libro: {libro.Titolo}");
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
                    Logger.LogInfo(nomeClasse, nameMetodo, $"Libro aggiunto con successo: {libro.Titolo}");
                }
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

        public void EliminaLibro(int id)
        {
            _libroRepository.DeleteLibro(id);
        }
    }
}