using System;
using System.Collections.Generic;
using System.Linq;
using GestionaleLibreria.Data.Models;
using GestionaleLibreria.Data.Logging;

namespace GestionaleLibreria.Data
{
    public interface IClienteRepository
    {
        List<Cliente> GetAllClienti();
        void AddCliente(Cliente cliente);
        void UpdateCliente(Cliente cliente);
        void DeleteCliente(int id);
    }

    public class ClienteRepository : IClienteRepository
    {
        private readonly LibraryContext _context;
        private static readonly string NomeClasse = nameof(ClienteRepository);

        public ClienteRepository()
        {
            _context = new LibraryContext();
        }

        public List<Cliente> GetAllClienti()
        {
            string nomeMetodo = nameof(GetAllClienti);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, "Recupero di tutti i clienti dal database.");
                var clienti = _context.Clienti.ToList();
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Recuperati {clienti.Count} clienti.");
                return clienti;
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public void AddCliente(Cliente cliente)
        {
            string nomeMetodo = nameof(AddCliente);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Tentativo di aggiunta cliente");
                _context.Clienti.Add(cliente);
                _context.SaveChanges();
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Cliente aggiunto con successo");
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public void UpdateCliente(Cliente cliente)
        {
            string nomeMetodo = nameof(UpdateCliente);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Tentativo di aggiornamento cliente ID: {cliente.Id}");
                var existing = _context.Clienti.FirstOrDefault(c => c.Id == cliente.Id);

                if (existing != null)
                {
                    existing.Nome = cliente.Nome;
                    existing.Cognome = cliente.Cognome;
                    existing.Email = cliente.Email;
                    existing.Telefono = cliente.Telefono;
                    _context.SaveChanges();
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Cliente aggiornato");
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Cliente con ID {cliente.Id} non trovato per l'aggiornamento.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }

        public void DeleteCliente(int id)
        {
            string nomeMetodo = nameof(DeleteCliente);
            try
            {
                Logger.LogInfo(NomeClasse, nomeMetodo, $"Tentativo di eliminazione cliente con ID: {id}");
                var cliente = _context.Clienti.FirstOrDefault(c => c.Id == id);

                if (cliente != null)
                {
                    _context.Clienti.Remove(cliente);
                    _context.SaveChanges();
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Cliente eliminato");
                }
                else
                {
                    Logger.LogInfo(NomeClasse, nomeMetodo, $"Cliente con ID {id} non trovato per l'eliminazione.");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(NomeClasse, nomeMetodo, ex);
                throw;
            }
        }
    }
}
