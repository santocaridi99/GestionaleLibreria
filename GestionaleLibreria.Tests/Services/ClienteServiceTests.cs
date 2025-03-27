using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using GestionaleLibreria.Business.Services;

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
        public void GetAllClienti_DovrebbeRestituireTuttiClienti()
        {
            var clientiMock = new List<Cliente>
            {
                new Cliente { Id = 1, Nome = "Mario", Cognome = "Rossi", Email = "mario@example.com" },
                new Cliente { Id = 2, Nome = "Luigi", Cognome = "Bianchi", Email = "luigi@example.com" }
            };

            _mockClienteRepository.Setup(repo => repo.GetAllClienti()).Returns(clientiMock);

            var clienti = _clienteService.GetAllClienti();

            Assert.That(clienti.Count, Is.EqualTo(2));
            Assert.That(clienti.Any(c => c.Nome == "Mario"));
        }

        [Test]
        public void AddCliente_DovrebbeChiamareRepository()
        {
            var nuovoCliente = new Cliente { Nome = "Giulia", Cognome = "Verdi", Email = "giulia@example.com" };

            _clienteService.AddCliente(nuovoCliente);

            _mockClienteRepository.Verify(repo => repo.AddCliente(nuovoCliente), Times.Once);
        }

        [Test]
        public void UpdateCliente_DovrebbeChiamareRepository()
        {
            var cliente = new Cliente { Id = 1, Nome = "Aggiorna", Cognome = "Cliente", Email = "aggiorna@example.com" };

            _clienteService.UpdateCliente(cliente);

            _mockClienteRepository.Verify(repo => repo.UpdateCliente(cliente), Times.Once);
        }

        [Test]
        public void DeleteCliente_DovrebbeChiamareRepository()
        {
            int clienteId = 1;

            _clienteService.DeleteCliente(clienteId);

            _mockClienteRepository.Verify(repo => repo.DeleteCliente(clienteId), Times.Once);
        }

        [Test]
        public void CercaClienti_PerNome_DovrebbeRestituireClientiFiltrati()
        {
            var clientiMock = new List<Cliente>
            {
                new Cliente { Nome = "Anna", Cognome = "Bianchi", Email = "anna@example.com" },
                new Cliente { Nome = "Annalisa", Cognome = "Rossi", Email = "annalisa@example.com" },
                new Cliente { Nome = "Luca", Cognome = "Verdi", Email = "luca@example.com" }
            };

            _mockClienteRepository.Setup(repo => repo.GetAllClienti()).Returns(clientiMock);

            var risultati = _clienteService.CercaClienti("Anna", "Nome");

            Assert.That(risultati.Count, Is.EqualTo(2));
            Assert.That(risultati.All(c => c.Nome.Contains("Anna")));
        }

        [Test]
        public void CercaClienti_EmailNonTrovata_DovrebbeRestituireListaVuota()
        {
            var clientiMock = new List<Cliente>
            {
                new Cliente { Nome = "Marco", Cognome = "Testa", Email = "marco@example.com" }
            };

            _mockClienteRepository.Setup(repo => repo.GetAllClienti()).Returns(clientiMock);

            var risultati = _clienteService.CercaClienti("noemail@example.com", "Email");

            Assert.That(risultati, Is.Empty);
        }
    }
}
