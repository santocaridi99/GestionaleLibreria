using GestionaleLibreria.Data.Models;
using GestionaleLibreria.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionaleLibreria.Business
{
    public class UtenteService
    {
        private readonly IUtenteRepository _utenteRepository;

        public UtenteService(IUtenteRepository utenteRepository)
        {
            _utenteRepository = utenteRepository;
        }

        public void RegistraUtente(string username, string password, string ruolo)
        {
            if (_utenteRepository.GetUtenteByUsername(username) != null)
            {
                throw new Exception("Username già esistente");
            }

            Utente nuovoUtente = new Utente
            {
                Username = username,
                PasswordHash = PasswordHelper.HashPassword(password),
                Ruolo = ruolo
            };

            _utenteRepository.AggiungiUtente(nuovoUtente);
        }

        public Utente EffettuaLogin(string username, string password)
        {
            Utente utente = _utenteRepository.GetUtenteByUsername(username);

            if (utente != null && PasswordHelper.VerifyPassword(password, utente.PasswordHash))
            {
                return utente;
            }

            return null; // Login fallito
        }
    }


}
