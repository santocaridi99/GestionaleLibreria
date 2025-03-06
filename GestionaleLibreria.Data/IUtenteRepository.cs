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
        void AggiungiUtente(Utente utente);
        Utente GetUtenteByUsername(string username);
    }


    public class UtenteRepository : IUtenteRepository
    {
        private readonly LibraryContext _context;

        public UtenteRepository(LibraryContext context)
        {
            _context = context;

            // Se non esistono utenti, crea un admin di default
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

        public void AggiungiUtente(Utente utente)
        {
            utente.PasswordHash = PasswordHelper.HashPassword(utente.PasswordHash);
            _context.Utenti.Add(utente);
            _context.SaveChanges();
        }

        public Utente GetUtenteByUsername(string username)
        {
            return _context.Utenti.FirstOrDefault(u => u.Username == username);
        }
    }
}
