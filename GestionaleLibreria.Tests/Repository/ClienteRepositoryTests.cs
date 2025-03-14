using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using System.Data.Entity;

namespace GestionaleLibreria.Tests
{
    [TestFixture]
    public class ClienteRepositoryTests
    {
        private Mock<LibraryContext> _mockContext;
        private Mock<DbSet<Cliente>> _mockDbSet;
        private ClienteRepository _clienteRepository;
        private List<Cliente> _clientiMock;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<LibraryContext>();
            _mockDbSet = new Mock<DbSet<Cliente>>();

            // Simuliamo un database in memoria
            _clientiMock = new List<Cliente>
            {
                new Cliente { Id = 1, Nome = "Mario", Cognome = "Rossi", Email = "mario.rossi@email.com", Telefono = "1234567890" },
                new Cliente { Id = 2, Nome = "Luigi", Cognome = "Verdi", Email = "luigi.verdi@email.com", Telefono = "0987654321" }
            };

            var queryableClienti = _clientiMock.AsQueryable();
            _mockDbSet.As<IQueryable<Cliente>>().Setup(m => m.Provider).Returns(queryableClienti.Provider);
            _mockDbSet.As<IQueryable<Cliente>>().Setup(m => m.Expression).Returns(queryableClienti.Expression);
            _mockDbSet.As<IQueryable<Cliente>>().Setup(m => m.ElementType).Returns(queryableClienti.ElementType);
            _mockDbSet.As<IQueryable<Cliente>>().Setup(m => m.GetEnumerator()).Returns(queryableClienti.GetEnumerator());

            _mockContext.Setup(c => c.Clienti).Returns(_mockDbSet.Object);

            _clienteRepository = new ClienteRepository();
        }

        [Test]
        public void GetAllClienti_DovrebbeRestituireListaClienti()
        {
            _mockContext.Setup(c => c.Clienti.ToList()).Returns(_clientiMock);

            var clienti = _clienteRepository.GetAllClienti();
            Assert.That(clienti, Is.Not.Null);
            Assert.That(clienti.Count, Is.EqualTo(2));
        }

        [Test]
        public void AddCliente_DovrebbeAggiungereCliente()
        {
            var nuovoCliente = new Cliente { Id = 3, Nome = "Paolo", Cognome = "Bianchi", Email = "paolo.bianchi@email.com", Telefono = "1122334455" };

            _mockDbSet.Setup(m => m.Add(It.IsAny<Cliente>())).Callback<Cliente>(c => _clientiMock.Add(c));

            _clienteRepository.AddCliente(nuovoCliente);

            Assert.That(_clientiMock.Count, Is.EqualTo(3));
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Test]
        public void UpdateCliente_DovrebbeAggiornareDatiCliente()
        {
            var clienteEsistente = _clientiMock.First();
            clienteEsistente.Email = "nuova.email@email.com";

            _mockContext.Setup(c => c.Clienti.FirstOrDefault(cl => cl.Id == clienteEsistente.Id))
                        .Returns(clienteEsistente);

            _clienteRepository.UpdateCliente(clienteEsistente);

            Assert.That(_clientiMock.First().Email, Is.EqualTo("nuova.email@email.com"));
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Test]
        public void DeleteCliente_DovrebbeRimuovereCliente()
        {
            int clienteId = 1;
            var clienteDaEliminare = _clientiMock.First(c => c.Id == clienteId);

            _mockDbSet.Setup(m => m.Remove(It.IsAny<Cliente>())).Callback<Cliente>(c => _clientiMock.Remove(c));

            _clienteRepository.DeleteCliente(clienteId);

            Assert.That(_clientiMock.Count, Is.EqualTo(1));
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
