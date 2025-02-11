using System.Collections.Generic;
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
    }
}
