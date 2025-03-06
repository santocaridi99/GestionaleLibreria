﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionaleLibreria.Data.Models
{
    public class LibroMagazzino
    {
        [Key]
        public int Id { get; set; }
        // Chiave esterna per Libro
        public int LibroId { get; set; }
        [ForeignKey("LibroId")]
        public virtual Libro Libro { get; set; }
        // Chiave esterna per Magazzino
        public int MagazzinoId { get; set; }
        [ForeignKey("MagazzinoId")]
        public virtual Magazzino Magazzino { get; set; }

        public int Quantita { get;  set; }

        public LibroMagazzino() { }  // Costruttore vuoto per Entity Framework

        public LibroMagazzino(Libro libro, int quantita)
        {
            Libro = libro;
            LibroId = libro.Id;
            Quantita = quantita;
        }

        public void AggiungiScorte(int quantita)
        {
            Quantita += quantita;
        }

        public bool RimuoviScorte(int quantita)
        {
            if (quantita > Quantita)
                return false;
            Quantita -= quantita;
            return true;
        }
    }
}