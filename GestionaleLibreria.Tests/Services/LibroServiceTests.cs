using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using GestionaleLibreria.Business;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;
using GestionaleLibreria.Business.Services;

namespace GestionaleLibreria.Tests
{
    [TestFixture]
    public class LibroServiceTests
    {
        private Mock<ILibroRepository> _mockLibroRepository;
        private Mock<IMagazzinoRepository> _mockMagazzinoRepository;
        private LibroService _libroService;

        [SetUp]
        public void Setup()
        {
            _mockLibroRepository = new Mock<ILibroRepository>();
            _mockMagazzinoRepository = new Mock<IMagazzinoRepository>();
            _libroService = new LibroService(_mockLibroRepository.Object, _mockMagazzinoRepository.Object);
        }

        [Test]
        public void GetAllLibri_DovrebbeRestituireListaLibri()
        {
            var libriMock = new List<Libro>
            {
                new Libro { Id = 1, Titolo = "Libro 1", Autore = "Autore 1", ISBN = "1234567890123", Prezzo = 10 },
                new Libro { Id = 2, Titolo = "Libro 2", Autore = "Autore 2", ISBN = "9876543210987", Prezzo = 15 }
            };

            _mockLibroRepository.Setup(repo => repo.GetAllLibri()).Returns(libriMock);

            var libri = _libroService.GetAllLibri();

            Assert.That(libri.Count, Is.EqualTo(2));
        }

        [Test]
        public void AggiungiLibro_DovrebbeAggiungereLibro()
        {
            var nuovoLibro = new Libro { Id = 3, Titolo = "Libro 3", Autore = "Autore 3", ISBN = "1111111111111", Prezzo = 20 };

            _libroService.AggiungiLibro(nuovoLibro);

            _mockLibroRepository.Verify(repo => repo.AddLibro(nuovoLibro), Times.Once);
        }

        [Test]
        public void ModificaLibro_DovrebbeModificareLibro()
        {
            var libroModificato = new Libro { Id = 1, Titolo = "Libro Modificato", Autore = "Autore Modificato", ISBN = "1234567890123", Prezzo = 12 };

            _libroService.ModificaLibro(libroModificato);

            _mockLibroRepository.Verify(repo => repo.UpdateLibro(libroModificato), Times.Once);
        }

        [Test]
        public void EliminaLibro_DovrebbeEliminareLibro()
        {
            int libroId = 1;

            _libroService.EliminaLibro(libroId);

            _mockLibroRepository.Verify(repo => repo.DeleteLibro(libroId), Times.Once);
        }

        [Test]
        public void CercaLibri_DovrebbeRestituireLibriFiltrati()
        {
            var libriMock = new List<Libro>
            {
                new Libro { Id = 1, Titolo = "Libro Java", Autore = "Autore 1", ISBN = "1234567890123", Prezzo = 10 },
                new Libro { Id = 2, Titolo = "Libro C#", Autore = "Autore 2", ISBN = "9876543210987", Prezzo = 15 }
            };

            _mockLibroRepository.Setup(repo => repo.GetAllLibri()).Returns(libriMock);

            var risultati = _libroService.CercaLibri("C#");

            Assert.That(risultati.Count, Is.EqualTo(1));
            Assert.That(risultati.First().Titolo, Is.EqualTo("Libro C#"));
        }
    }
}
