using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionaleLibreria.Business
{
    public class MagazzinoService
    {
        private readonly Magazzino _magazzino;

        public MagazzinoService(Magazzino magazzino)
        {
            _magazzino = magazzino;
        }

        // Aggiungi libri fisici al magazzino
        public void AggiungiLibroFisico(Libro libro, int quantita)
        {
            var record = _magazzino.LibriMagazzino.FirstOrDefault(lm => lm.Libro.ISBN == libro.ISBN);
            if (record != null)
            {
                record.AggiungiScorte(quantita);
            }
            else
            {
                _magazzino.LibriMagazzino.Add(new LibroMagazzino(libro, quantita));
            }
        }

        // Rimuovi una quantità di un libro
        public bool RimuoviLibroFisico(string isbn, int quantita)
        {
            var record = _magazzino.LibriMagazzino.FirstOrDefault(lm => lm.Libro.ISBN == isbn);
            if (record != null)
            {
                return record.RimuoviScorte(quantita);
            }
            return false;
        }

        public int OttieniQuantitaLibro(string isbn)
        {
            var record = _magazzino.LibriMagazzino.FirstOrDefault(lm => lm.Libro.ISBN == isbn);
            return record != null ? record.Quantita : 0;
        }
    }
}
