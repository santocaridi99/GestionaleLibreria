using System;
using System.Collections.Generic;
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
            return _vendite;
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
