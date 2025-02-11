using System.Collections.Generic;
using System.Linq;
using GestionaleLibreria.Data.Models;


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

        public ClienteRepository()
        {
            _context = new LibraryContext();
        }

        public List<Cliente> GetAllClienti()
        {
            return _context.Clienti.ToList();   
        }

        public void AddCliente(Cliente cliente)
        {
            _context.Clienti.Add(cliente);
            _context.SaveChanges();
        }

        public void UpdateCliente(Cliente cliente)
        {
            var existing = _context.Clienti.FirstOrDefault(c => c.Id == cliente.Id);
            if (existing != null)
            {
                existing.Nome = cliente.Nome;
                existing.Cognome = cliente.Cognome;
                existing.Email = cliente.Email;
                existing.Telefono = cliente.Telefono;
                _context.SaveChanges();
            }
        }

        public void DeleteCliente(int id)
        {
            var cliente = _context.Clienti.FirstOrDefault(c => c.Id == id);
            if (cliente != null)
            {
                _context.Clienti.Remove(cliente);
                _context.SaveChanges();
            }
        }
    }


}
