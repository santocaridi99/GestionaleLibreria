using GestionaleLibreria.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionaleLibreria.Data
{

    public interface IUtenteRepository
    {
        void AggiungiUtente(string username, string passwordInChiaro, string ruolo);
        Utente GetUtenteByUsername(string username);
        bool AnyUserExists(); // Metodo utile per verificare esistenza utenti
    }



    public class UtenteRepository : IUtenteRepository
    {
        private readonly LibraryContext _context;

        public UtenteRepository(LibraryContext context)
        {
            _context = context;

            if (!_context.Utenti.Any())
            {
                var admin = new Utente
                {
                    Username = "admin",
                    PasswordHash = PasswordHelper.HashPassword("admin"),
                    Ruolo = "Admin"
                };

                _context.Utenti.Add(admin);
                _context.SaveChanges();
            }
        }

        public void AggiungiUtente(string username, string passwordInChiaro, string ruolo)
        {
            var nuovoUtente = new Utente
            {
                Username = username,
                PasswordHash = PasswordHelper.HashPassword(passwordInChiaro),
                Ruolo = ruolo
            };

            _context.Utenti.Add(nuovoUtente);
            _context.SaveChanges();
        }

        public Utente GetUtenteByUsername(string username)
        {
            return _context.Utenti.FirstOrDefault(u => u.Username == username);
        }

        public bool AnyUserExists()
        {
            return _context.Utenti.Any();
        }
    }

}
