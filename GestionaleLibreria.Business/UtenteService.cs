using GestionaleLibreria.Data.Models;
using GestionaleLibreria.Data;
using System;

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
            throw new Exception("Username già esistente.");
        }

        _utenteRepository.AggiungiUtente(username, password, ruolo);
    }

    public Utente EffettuaLogin(string username, string password)
    {
        Utente utente = _utenteRepository.GetUtenteByUsername(username);

        if (utente != null && PasswordHelper.VerifyPassword(password, utente.PasswordHash))
        {
            return utente;
        }

        return null;
    }
}
