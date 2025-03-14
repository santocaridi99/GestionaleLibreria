using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.Tests
{
    [TestFixture]
    public class ClienteServiceTests
    {
        private Mock<IClienteRepository> _mockClienteRepository;
        private ClienteService _clienteService;

        [SetUp]
        public void Setup()
        {
            _mockClienteRepository = new Mock<IClienteRepository>();
            _clienteService = new ClienteService(_mockClienteRepository.Object);
        }

        [Test]
        public void GetAllClienti_DovrebbeRestituireListaClienti()
        {
            var clientiMock = new List<Cliente>
            {
                new Cliente { Id = 1, Nome = "Mario", Cognome = "Rossi", Email = "mario.rossi@email.com", Telefono = "1234567890" },
                new Cliente { Id = 2, Nome = "Luigi", Cognome = "Verdi", Email = "luigi.verdi@email.com", Telefono = "0987654321" }
            };

            _mockClienteRepository.Setup(repo => repo.GetAllClienti()).Returns(clientiMock);

            var clienti = _clienteService.GetAllClienti();

            Assert.That(clienti.Count, Is.EqualTo(2));
        }

        [Test]
        public void AddCliente_DovrebbeChiamareRepository()
        {
            var nuovoCliente = new Cliente { Id = 3, Nome = "Paolo", Cognome = "Bianchi", Email = "paolo.bianchi@email.com", Telefono = "1122334455" };

            _clienteService.AddCliente(nuovoCliente);

            _mockClienteRepository.Verify(repo => repo.AddCliente(nuovoCliente), Times.Once);
        }

        [Test]
        public void UpdateCliente_DovrebbeChiamareRepository()
        {
            var clienteEsistente = new Cliente { Id = 1, Nome = "Mario", Cognome = "Rossi", Email = "mario.rossi@email.com", Telefono = "1234567890" };

            _clienteService.UpdateCliente(clienteEsistente);

            _mockClienteRepository.Verify(repo => repo.UpdateCliente(clienteEsistente), Times.Once);
        }

        [Test]
        public void DeleteCliente_DovrebbeChiamareRepository()
        {
            int clienteId = 1;

            _clienteService.DeleteCliente(clienteId);

            _mockClienteRepository.Verify(repo => repo.DeleteCliente(clienteId), Times.Once);
        }
    }
}
