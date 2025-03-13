using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GestionaleLibreria.Data.Logging;
using GestionaleLibreria.Data.Models;


namespace GestionaleLibreria.Data
{
    public interface IVenditaRepository
    {
        List<Vendita> GetAllVendite();
        void AddVendita(Vendita vendita);
        void SaveChanges();
        List<Vendita> GetVenditePerPeriodo(DateTime dataInizio, DateTime dataFine);

    }
    public class VenditaRepository : IVenditaRepository
    {
        private readonly List<Vendita> _vendite = new List<Vendita>();
        private readonly LibraryContext _context;
       

        public VenditaRepository()
        {
            _context = new LibraryContext();
        }

        public List<Vendita> GetAllVendite()
        {
            using (var context = new LibraryContext())
            {
                return context.Vendite
                    .Include(v => v.Cliente)
                   .Include("DettagliVendita.Libro")
                    .ToList();
            }
        }

        public void AddVendita(Vendita vendita)
        {
            if (vendita == null)
                throw new ArgumentNullException(nameof(vendita), "La vendita non può essere null.");

            if (vendita.ClienteId != null  && vendita.ClienteId > 0)
            {
                // Se il cliente esiste, lo attacchiamo al contesto invece di aggiungerlo
                _context.Entry(vendita).Reference(v => v.Cliente).Load();
            }

            // Per ogni dettaglio della vendita, assicuriamoci che il libro sia tracciato correttamente
            foreach (var dettaglio in vendita.DettagliVendita)
            {
                if (_context.Libri.Any(l => l.Id == dettaglio.LibroId))
                {
                    _context.Libri.Attach(dettaglio.Libro);
                }
            }

            // Ora possiamo aggiungere la vendita
            _context.Vendite.Add(vendita);
        }
        public List<Vendita> GetVenditePerPeriodo(DateTime dataInizio, DateTime dataFine)
        {
            string nomeMetodo = nameof(GetVenditePerPeriodo);
            try
            {
                Logger.LogInfo(nameof(VenditaRepository), nomeMetodo,
                    $"Recupero vendite dal {dataInizio:yyyy-MM-dd HH:mm:ss} al {dataFine:yyyy-MM-dd HH:mm:ss}");

                using (var context = new LibraryContext())
                {
                    var vendite = context.Vendite
                        .Where(v => v.DataVendita >= dataInizio && v.DataVendita <= dataFine)
                        .Include(v => v.Cliente)
                        .ToList();

                    Logger.LogInfo(nameof(VenditaRepository), nomeMetodo,
                        $"Trovate {vendite.Count} vendite nel periodo selezionato.");

                    return vendite;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(nameof(VenditaRepository), nomeMetodo, ex);
                throw;
            }
        }




        public void SaveChanges()
        {
            if (_context == null)
            {
                throw new InvalidOperationException("Il contesto del database è null. La vendita non può essere salvata.");
            }

            Logger.LogInfo(nameof(VenditaRepository), nameof(SaveChanges), "Salvataggio delle modifiche nel database.");
            _context.SaveChanges();
        }

    }
}
