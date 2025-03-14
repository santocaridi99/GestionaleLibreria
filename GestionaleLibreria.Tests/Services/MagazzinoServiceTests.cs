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
    public class MagazzinoServiceTests
    {
        private Mock<IMagazzinoRepository> _mockMagazzinoRepository;
        private Mock<ILibroRepository> _mockLibroRepository;
        private MagazzinoService _magazzinoService;

        [SetUp]
        public void Setup()
        {
            _mockMagazzinoRepository = new Mock<IMagazzinoRepository>();
            _mockLibroRepository = new Mock<ILibroRepository>();
            _magazzinoService = new MagazzinoService(_mockMagazzinoRepository.Object, _mockLibroRepository.Object);
        }

        [Test]
        public void GetLibriMagazzino_DovrebbeRestituireListaLibri()
        {
            var libriMagazzinoMock = new List<LibroMagazzino>
            {
                new LibroMagazzino { Id = 1, LibroId = 101, Quantita = 10 },
                new LibroMagazzino { Id = 2, LibroId = 102, Quantita = 5 }
            };

            _mockMagazzinoRepository.Setup(repo => repo.GetAllLibriInMagazzino()).Returns(libriMagazzinoMock);

            var libri = _magazzinoService.GetLibriMagazzino();

            Assert.That(libri.Count, Is.EqualTo(2));
        }

        [Test]
        public void AggiungiScorte_DovrebbeAumentareQuantita()
        {
            var libroMagazzino = new LibroMagazzino { Id = 1, LibroId = 101, Quantita = 10 };

            _mockMagazzinoRepository.Setup(repo => repo.GetLibroMagazzinoById(101)).Returns(libroMagazzino);

            _magazzinoService.AggiungiScorte(101, 5);

            Assert.That(libroMagazzino.Quantita, Is.EqualTo(15));
            _mockMagazzinoRepository.Verify(repo => repo.AggiornaLibroMagazzino(libroMagazzino), Times.Once);
        }

        [Test]
        public void RimuoviScorte_DovrebbeDiminuireQuantita()
        {
            var libroMagazzino = new LibroMagazzino { Id = 1, LibroId = 101, Quantita = 10 };

            _mockMagazzinoRepository.Setup(repo => repo.GetLibroMagazzinoById(101)).Returns(libroMagazzino);

            bool risultato = _magazzinoService.RimuoviScorte(101, 5);

            Assert.That(risultato, Is.True);
            Assert.That(libroMagazzino.Quantita, Is.EqualTo(5));
            _mockMagazzinoRepository.Verify(repo => repo.AggiornaLibroMagazzino(libroMagazzino), Times.Once);
        }

        [Test]
        public void RimuoviScorte_QuantitaInsufficiente_DovrebbeFallire()
        {
            var libroMagazzino = new LibroMagazzino { Id = 1, LibroId = 101, Quantita = 4 };

            _mockMagazzinoRepository.Setup(repo => repo.GetLibroMagazzinoById(101)).Returns(libroMagazzino);

            bool risultato = _magazzinoService.RimuoviScorte(101, 5);

            Assert.That(risultato, Is.False);
            _mockMagazzinoRepository.Verify(repo => repo.AggiornaLibroMagazzino(It.IsAny<LibroMagazzino>()), Times.Never);
        }

        [Test]
        public void OttieniQuantitaLibro_DovrebbeRestituireQuantita()
        {
            var libroMagazzino = new LibroMagazzino { Id = 1, LibroId = 101, Quantita = 10 };

            _mockMagazzinoRepository.Setup(repo => repo.GetLibroMagazzinoById(101)).Returns(libroMagazzino);

            int quantita = _magazzinoService.OttieniQuantitaLibro(101);

            Assert.That(quantita, Is.EqualTo(10));
        }
    }
}
