using GestionaleLibreria.Data.Logging;
using GestionaleLibreria.Data.Models;
using System;
using System.Linq;

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
        private static readonly string NomeClasse = nameof(UtenteRepository);

        public UtenteRepository(LibraryContext context)
        {
            _context = context;
            string nomeMetodo = nameof(UtenteRepository);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Inizializzazione repository utenti.");

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
                    Logger.LogInfo(NomeClasse, nomeMetodo, "Utente admin creato con successo.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public void AggiungiUtente(string username, string passwordInChiaro, string ruolo)
        {
            string nomeMetodo = nameof(AggiungiUtente);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Tentativo di aggiunta utente: {username} con ruolo: {ruolo}.");

                if (_context.Utenti.Any(u => u.Username == username))
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Errore: L'utente '{username}' esiste già.");
                    throw new Exception($"L'utente '{username}' esiste già.");
                }

                var nuovoUtente = new Utente
                {
                    Username = username,
                    PasswordHash = PasswordHelper.HashPassword(passwordInChiaro),
                    Ruolo = ruolo
                };

                _context.Utenti.Add(nuovoUtente);
                _context.SaveChanges();

                Logger.LogInfo(NomeClasse, nomeMetodo, $"Utente '{username}' aggiunto con successo.");
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public Utente GetUtenteByUsername(string username)
        {
            string nomeMetodo = nameof(GetUtenteByUsername);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Ricerca utente: {username}");

                var utente = _context.Utenti.FirstOrDefault(u => u.Username == username);

                if (utente != null)
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Utente trovato: {username}");
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Utente '{username}' non trovato.");
                }

                return utente;
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public bool AnyUserExists()
        {
            string nomeMetodo = nameof(AnyUserExists);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Verifica esistenza di utenti nel database.");

                bool esisteUtente = _context.Utenti.Any();

                Logger.LogInfo(NomeClasse, nomeMetodo, $"Esistono utenti: {esisteUtente}");
                return esisteUtente;
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }
    }
}
