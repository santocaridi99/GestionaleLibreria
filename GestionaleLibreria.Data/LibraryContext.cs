using System;
using System.Collections.Generic;
using System.Data.Entity;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext()
            : base("name=LibraryDB")
        {
        }

        public DbSet<Libro> Libri { get; set; }
        public DbSet<Cliente> Clienti { get; set; }
        public DbSet<Vendita> Vendite { get; set; }
        public DbSet<LibroMagazzino> LibriMagazzino { get; set; }
        public DbSet<Magazzino> Magazzini { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurazione della relazione: Un libro ha molti LibroMagazzino
            modelBuilder.Entity<Libro>()
                .HasMany(l => l.LibriMagazzino)
                .WithRequired(lm => lm.Libro)
                .HasForeignKey(lm => lm.LibroId);

            // Configurazione della relazione: Un magazzino ha molti LibroMagazzino
            modelBuilder.Entity<Magazzino>()
                .HasMany(m => m.LibriMagazzino)
                .WithRequired(lm => lm.Magazzino)
                .HasForeignKey(lm => lm.MagazzinoId);
        }
    }
}
