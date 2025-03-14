using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using GestionaleLibreria.Business;

namespace GestionaleLibreria.Tests
{
    [TestFixture]
    public class VenditaServiceTests
    {
        private Mock<IVenditaRepository> _mockVenditaRepository;
        private Mock<IMagazzinoRepository> _mockMagazzinoRepository;
        private Mock<ILibroRepository> _mockLibroRepository;
        private VenditaService _venditaService;

        [SetUp]
        public void Setup()
        {
            _mockVenditaRepository = new Mock<IVenditaRepository>();
            _mockMagazzinoRepository = new Mock<IMagazzinoRepository>();
            _mockLibroRepository = new Mock<ILibroRepository>();

            _venditaService = new VenditaService(_mockVenditaRepository.Object, _mockMagazzinoRepository.Object, _mockLibroRepository.Object);
        }

       

        // ✅ Test: GetVendite dovrebbe restituire una lista di vendite
        [Test]
        public void GetVendite_DovrebbeRestituireListaVendite()
        {
            var venditeMock = new List<Vendita>
            {
                new Vendita { Id = 1, ClienteId = 1, DataVendita = DateTime.Now, Totale = 20 },
                new Vendita { Id = 2, ClienteId = 2, DataVendita = DateTime.Now, Totale = 30 }
            };

            _mockVenditaRepository.Setup(repo => repo.GetAllVendite()).Returns(venditeMock);

            var vendite = _venditaService.GetVendite();

            Assert.That(vendite.Count, Is.EqualTo(2));
        }

        // ✅ Test: GetVenditePerPeriodo dovrebbe restituire le vendite nel periodo richiesto
        [Test]
        public void GetVenditePerPeriodo_DovrebbeRestituireVenditeNelPeriodo()
        {
            var dataInizio = new DateTime(2025, 3, 1);
            var dataFine = new DateTime(2025, 3, 31);

            var venditeMock = new List<Vendita>
            {
                new Vendita { Id = 1, ClienteId = 1, DataVendita = new DateTime(2025, 3, 10), Totale = 20 },
                new Vendita { Id = 2, ClienteId = 2, DataVendita = new DateTime(2025, 3, 20), Totale = 30 }
            };

            _mockVenditaRepository.Setup(repo => repo.GetVenditePerPeriodo(dataInizio, dataFine)).Returns(venditeMock);

            var vendite = _venditaService.GetVenditePerPeriodo(dataInizio, dataFine);

            Assert.That(vendite.Count, Is.EqualTo(2));
        }

        // ✅ Test: GetNumeroAcquistiCliente dovrebbe restituire il numero di acquisti di un cliente
        [Test]
        public void GetNumeroAcquistiCliente_DovrebbeRestituireNumeroCorretto()
        {
            var venditeMock = new List<Vendita>
            {
                new Vendita { Id = 1, ClienteId = 1, DataVendita = DateTime.Now, Totale = 20 },
                new Vendita { Id = 2, ClienteId = 1, DataVendita = DateTime.Now, Totale = 30 }
            };

            _mockVenditaRepository.Setup(repo => repo.GetAllVendite()).Returns(venditeMock);

            int numeroAcquisti = _venditaService.GetNumeroAcquistiCliente(1);

            Assert.That(numeroAcquisti, Is.EqualTo(2));
        }

        // ✅ Test: GetTotaleSpesoCliente dovrebbe restituire il totale speso dal cliente
        [Test]
        public void GetTotaleSpesoCliente_DovrebbeRestituireTotaleCorretto()
        {
            var venditeMock = new List<Vendita>
            {
                new Vendita { Id = 1, ClienteId = 1, DataVendita = DateTime.Now, Totale = 20 },
                new Vendita { Id = 2, ClienteId = 1, DataVendita = DateTime.Now, Totale = 30 }
            };

            _mockVenditaRepository.Setup(repo => repo.GetAllVendite()).Returns(venditeMock);

            decimal totaleSpeso = _venditaService.GetTotaleSpesoCliente(1);

            Assert.That(totaleSpeso, Is.EqualTo(50));
        }

        // ✅ Test: GetQuantitaDisponibile dovrebbe restituire la quantità disponibile di un libro
        [Test]
        public void GetQuantitaDisponibile_DovrebbeRestituireQuantitaCorretta()
        {
            int libroId = 1;
            var libroMagazzinoMock = new LibroMagazzino { LibroId = libroId, Quantita = 10 };

            _mockMagazzinoRepository.Setup(repo => repo.GetLibroMagazzinoById(libroId)).Returns(libroMagazzinoMock);

            int quantitaDisponibile = _venditaService.GetQuantitaDisponibile(libroId);

            Assert.That(quantitaDisponibile, Is.EqualTo(10));
        }

        // ✅ Test: GetQuantitaDisponibile dovrebbe restituire 0 se il libro non esiste nel magazzino
        [Test]
        public void GetQuantitaDisponibile_DovrebbeRestituireZeroSeLibroNonEsiste()
        {
            int libroId = 2;

            _mockMagazzinoRepository.Setup(repo => repo.GetLibroMagazzinoById(libroId)).Returns((LibroMagazzino)null);

            int quantitaDisponibile = _venditaService.GetQuantitaDisponibile(libroId);

            Assert.That(quantitaDisponibile, Is.EqualTo(0));
        }
    }
}
