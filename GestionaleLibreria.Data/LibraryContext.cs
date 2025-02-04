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
    }
}
