using System;
using System.Collections.Generic;
using System.Linq;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;


namespace GestionaleLibreria.Business.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public List<Cliente> GetAllClienti()
        {
            return _clienteRepository.GetAllClienti();
        }

        public void AddCliente(Cliente cliente)
        {
            _clienteRepository.AddCliente(cliente);
        }

        public void UpdateCliente(Cliente cliente)
        {
            _clienteRepository.UpdateCliente(cliente);
        }

        public void DeleteCliente(int id)
        {
            _clienteRepository.DeleteCliente(id);
        }

        public List<Cliente> CercaClienti(string filtro, string criterio)
        {
          
            var tutti = _clienteRepository.GetAllClienti();

         
            if (string.IsNullOrEmpty(filtro))
                return tutti;

          
            return tutti.Where(cliente =>
                (criterio == "Nome" && cliente.Nome.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (criterio == "Cognome" && cliente.Cognome.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (criterio == "Email" && cliente.Email.IndexOf(filtro, StringComparison.OrdinalIgnoreCase) >= 0)
            ).ToList();
        }

    }
}
