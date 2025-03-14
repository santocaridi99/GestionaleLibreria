using System;
using System.Collections.Generic;
using System.Linq;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using Moq;
using NUnit.Framework;

namespace GestionaleLibreria.Tests
{
    [TestFixture]
    public class VenditaRepositoryTests
    {
        private Mock<IVenditaRepository> _mockVenditaRepository;

        [SetUp]
        public void Setup()
        {
            _mockVenditaRepository = new Mock<IVenditaRepository>();
        }

        [Test]
        public void GetAllVendite_DeveRestituireElencoVendite()
        {
            var venditeMock = new List<Vendita>
            {
                new Vendita { Id = 1, ClienteId = 2, DataVendita = DateTime.Now, Totale = 30.5m },
                new Vendita { Id = 2, ClienteId = 3, DataVendita = DateTime.Now, Totale = 15m }
            };

            _mockVenditaRepository.Setup(repo => repo.GetAllVendite()).Returns(venditeMock);

            var vendite = _mockVenditaRepository.Object.GetAllVendite();

            Assert.That(vendite, Is.Not.Null);
            Assert.That(vendite.Count, Is.EqualTo(2));
            Assert.That(vendite.First().Totale, Is.EqualTo(30.5m));
        }
    }
}
