using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext()
            : base("name=LibraryDB")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<LibraryContext>());
            InizializzaAdmin();
        }

        public DbSet<Libro> Libri { get; set; }
        public DbSet<Cliente> Clienti { get; set; }
        public DbSet<Vendita> Vendite { get; set; }
        public DbSet<LibroMagazzino> LibriMagazzino { get; set; }
        public DbSet<Magazzino> Magazzini { get; set; }
        public DbSet<Utente> Utenti { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Libro>()
            .Property(l => l.ISBN)
            .HasColumnAnnotation(
                "Index", new IndexAnnotation(new IndexAttribute("IX_ISBN") { IsUnique = true })
            );


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



        private void InizializzaAdmin()
        {
            if (!Utenti.Any(u => u.Ruolo == "Admin"))
            {
                var admin = new Utente
                {
                    Username = "admin",
                    PasswordHash = PasswordHelper.HashPassword("admin"),
                    Ruolo = "Admin"
                };

                Utenti.Add(admin);
                SaveChanges();
            }
        }
    }
}

