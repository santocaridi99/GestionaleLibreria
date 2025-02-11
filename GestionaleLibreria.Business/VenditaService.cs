using GestionaleLibreria.Data.Models;
using GestionaleLibreria.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionaleLibreria.Business
{
    public class VenditaService
    {
        private readonly IVenditaRepository _venditaRepository;

       
        public VenditaService(IVenditaRepository venditaRepository)
        {
            _venditaRepository = venditaRepository;
        }

        // Registra una nuova vendita
        public void RegistraVendita(Vendita vendita)
        {
            // Calcola il totale utilizzando il metodo CalcolaPrezzo() del libro
            vendita.Totale = vendita.QuantitaVenduta * vendita.Libro.CalcolaPrezzo();
            // Imposta la data della vendita
            vendita.DataVendita = DateTime.Now;

            _venditaRepository.AddVendita(vendita);
        }

        public List<Vendita> GetVendite()
        {
            return _venditaRepository.GetAllVendite();
        }
    }
}
