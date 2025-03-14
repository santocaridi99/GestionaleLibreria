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
    public class MagazzinoRepositoryTests
    {
        private Mock<LibraryContext> _mockContext;
        private Mock<DbSet<LibroMagazzino>> _mockDbSet;
        private MagazzinoRepository _magazzinoRepository;
        private List<LibroMagazzino> _libriMagazzinoMock;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<LibraryContext>();
            _mockDbSet = new Mock<DbSet<LibroMagazzino>>();

            // Simuliamo un database in memoria
            _libriMagazzinoMock = new List<LibroMagazzino>
            {
                new LibroMagazzino { Id = 1, LibroId = 101, Quantita = 10 },
                new LibroMagazzino { Id = 2, LibroId = 102, Quantita = 5 }
            };

            var queryableLibriMagazzino = _libriMagazzinoMock.AsQueryable();
            _mockDbSet.As<IQueryable<LibroMagazzino>>().Setup(m => m.Provider).Returns(queryableLibriMagazzino.Provider);
            _mockDbSet.As<IQueryable<LibroMagazzino>>().Setup(m => m.Expression).Returns(queryableLibriMagazzino.Expression);
            _mockDbSet.As<IQueryable<LibroMagazzino>>().Setup(m => m.ElementType).Returns(queryableLibriMagazzino.ElementType);
            _mockDbSet.As<IQueryable<LibroMagazzino>>().Setup(m => m.GetEnumerator()).Returns(queryableLibriMagazzino.GetEnumerator());

            _mockContext.Setup(c => c.LibriMagazzino).Returns(_mockDbSet.Object);

            _magazzinoRepository = new MagazzinoRepository(_mockContext.Object);
        }

        [Test]
        public void GetAllLibriInMagazzino_DovrebbeRestituireLista()
        {
            _mockContext.Setup(c => c.LibriMagazzino.ToList()).Returns(_libriMagazzinoMock);

            var libri = _magazzinoRepository.GetAllLibriInMagazzino();
            Assert.That(libri, Is.Not.Null);
            Assert.That(libri.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetLibroMagazzinoById_DovrebbeRestituireLibroEsistente()
        {
            int libroId = 101;
            _mockContext.Setup(c => c.LibriMagazzino.FirstOrDefault(l => l.LibroId == libroId))
                        .Returns(_libriMagazzinoMock.First(l => l.LibroId == libroId));

            var libroMagazzino = _magazzinoRepository.GetLibroMagazzinoById(libroId);

            Assert.That(libroMagazzino, Is.Not.Null);
            Assert.That(libroMagazzino.LibroId, Is.EqualTo(101));
        }

        [Test]
        public void AggiungiLibroMagazzino_DovrebbeAggiungereLibro()
        {
            var nuovoLibro = new LibroMagazzino { Id = 3, LibroId = 103, Quantita = 15 };

            _mockDbSet.Setup(m => m.Add(It.IsAny<LibroMagazzino>())).Callback<LibroMagazzino>(l => _libriMagazzinoMock.Add(l));

            _magazzinoRepository.AggiungiLibroMagazzino(nuovoLibro);

            Assert.That(_libriMagazzinoMock.Count, Is.EqualTo(3));
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Test]
        public void RimuoviLibroMagazzino_DovrebbeRimuovereLibro()
        {
            int libroId = 101;
            _mockDbSet.Setup(m => m.Remove(It.IsAny<LibroMagazzino>())).Callback<LibroMagazzino>(l => _libriMagazzinoMock.Remove(l));

            _magazzinoRepository.RimuoviLibroMagazzino(libroId);

            Assert.That(_libriMagazzinoMock.Count, Is.EqualTo(1));
        }
    }
}
