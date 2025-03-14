﻿using GestionaleLibreria.Data.Models;
using GestionaleLibreria.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestionaleLibreria.Data.Logging;

namespace GestionaleLibreria.Business
{
    public class VenditaService
    {
        private readonly IMagazzinoRepository _magazzinoRepository;
        private readonly IVenditaRepository _venditaRepository;
        private readonly ILibroRepository _libroRepository;

        public VenditaService(IVenditaRepository venditaRepository, IMagazzinoRepository magazzinoRepository, ILibroRepository libroRepository)
        {
            _venditaRepository = venditaRepository ?? throw new ArgumentNullException(nameof(venditaRepository));
            _magazzinoRepository = magazzinoRepository ?? throw new ArgumentNullException(nameof(magazzinoRepository));
            _libroRepository = libroRepository ?? throw new ArgumentNullException(nameof(libroRepository));
        }


        public VenditaService(IVenditaRepository venditaRepository)
        {
            _venditaRepository = venditaRepository;
        }
        public void RegistraVendita(Vendita vendita, List<VenditaDettaglio> dettagliVendita)
        {
            string nomeMetodo = nameof(RegistraVendita);
            try
            {
                Logger.LogInfo(nameof(VenditaService), nomeMetodo, "Registrazione vendita iniziata");

                using (var context = new LibraryContext())
                {
                    vendita.DataVendita = DateTime.Now;
                    vendita.Totale = dettagliVendita.Sum(d => d.Quantita * d.PrezzoUnitario);
                    vendita.QuantitaVenduta = dettagliVendita.Sum(d => d.Quantita);

                    foreach (var dettaglio in dettagliVendita)
                    {
                      
                        var libro = context.Libri.SingleOrDefault(l => l.Id == dettaglio.LibroId);
                        if (libro == null)
                        {
                            throw new Exception($"Errore: Il libro con ID {dettaglio.LibroId} non esiste nel database.");
                        }

                        dettaglio.Quantita = dettaglio.Quantita;

                        var libroMagazzino = context.LibriMagazzino.SingleOrDefault(lm => lm.LibroId == dettaglio.LibroId);
                        if (libroMagazzino != null)
                        {
                            if (libroMagazzino.Quantita < dettaglio.Quantita)
                            {
                                throw new Exception($"Quantità insufficiente per il libro {libro.Titolo}. Disponibile: {libroMagazzino.Quantita}");
                            }

                            libroMagazzino.Quantita -= dettaglio.Quantita;
                            context.Entry(libroMagazzino).State = System.Data.Entity.EntityState.Modified;
                        }
                       
                        dettaglio.Libro = null; 
                        dettaglio.Vendita = vendita;
                        context.Entry(dettaglio).State = System.Data.Entity.EntityState.Added;

                    }

                    context.Vendite.Add(vendita);
                    context.VenditaDettagli.AddRange(dettagliVendita);
                    context.SaveChanges();
                }

                Logger.LogInfo(nameof(VenditaService), nomeMetodo, "Vendita registrata con successo");
            }
            catch (Exception ex)
            {
                Logger.LogError(nameof(VenditaService), nomeMetodo, ex);
                throw;
            }
        }









        public int GetQuantitaDisponibile(int libroId)
        {
            if (_magazzinoRepository == null)
            {
                throw new Exception("MagazzinoRepository non è inizializzato correttamente.");
            }

            var libroMagazzino = _magazzinoRepository.GetLibroMagazzinoById(libroId);

            if (libroMagazzino == null)
            {
                return 0;
            }

            return libroMagazzino.Quantita;
        }


        public List<Vendita> GetVendite()
        {
            return _venditaRepository.GetAllVendite();
        }

        public int GetNumeroAcquistiCliente(int clienteId)
        {
            int numeroAcquisti = GetVendite()
                .Where(v => v.ClienteId == clienteId)
                .Count();

            
            return numeroAcquisti;
        }


        public decimal GetTotaleSpesoCliente(int clienteId)
        {
            decimal totaleSpeso = GetVendite()
                .Where(v => v.ClienteId == clienteId)
                .Sum(v => v.Totale);

            
            return totaleSpeso;
        }



        public List<Vendita> GetVenditePerPeriodo(DateTime dataInizio, DateTime dataFine)
        {
            string nomeMetodo = nameof(GetVenditePerPeriodo);
            try
            {
                Logger.LogInfo(nameof(VenditaService), nomeMetodo, $"Richiesta vendite dal {dataInizio:dd/MM/yyyy} al {dataFine:dd/MM/yyyy}");
                return _venditaRepository.GetVenditePerPeriodo(dataInizio, dataFine);
            }
            catch (Exception ex)
            {
                Logger.LogError(nameof(VenditaService), nomeMetodo, ex);
                throw;
            }
        }

    }
}
