using System.Collections.Generic;
using System.Linq;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.Data
{
    public interface IVenditaRepository
    {
        List<Vendita> GetAllVendite();
        void AddVendita(Vendita vendita);
  
    }
    public class VenditaRepository : IVenditaRepository
    {
        private readonly List<Vendita> _vendite = new List<Vendita>();

        public List<Vendita> GetAllVendite()
        {
            return _vendite;
        }

        public void AddVendita(Vendita vendita)
        {
            // Simula l'assegnazione dell'ID
            vendita.Id = _vendite.Count > 0 ? _vendite.Max(v => v.Id) + 1 : 1;
            _vendite.Add(vendita);
        }
    }
}
