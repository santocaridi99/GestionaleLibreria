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
public class LibroRepositoryTests
{
    private Mock<LibraryContext> _mockContext;
    private Mock<DbSet<Libro>> _mockDbSet;
    private LibroRepository _libroRepository;
    private List<Libro> _libriMock;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<LibraryContext>();

            _libriMock = new List<Libro>
    {
        new Libro { Id = 1, Titolo = "Libro 1", Autore = "Autore 1", ISBN = "1234567890123", Prezzo = 10 },
        new Libro { Id = 2, Titolo = "Libro 2", Autore = "Autore 2", ISBN = "9876543210987", Prezzo = 15 }
    };

            _mockDbSet = MockDbSetHelper.CreateMockDbSet(_libriMock);


            // 🔹 Utilizza `As<IQueryable<T>>()` per permettere il setup della query LINQ
            _mockContext.Setup(c => c.Libri).Returns(_mockDbSet.Object);

            // Mock del salvataggio
            _mockContext.Setup(m => m.SaveChanges()).Verifiable();

            _libroRepository = new LibroRepository(_mockContext.Object);
        }



    [Test]
    public void GetAllLibri_DovrebbeRestituireListaLibri()
    {
        // Act
        var libri = _libroRepository.GetAllLibri();

        // Assert
        Assert.That(libri, Is.Not.Null);
        Assert.That(libri.Count, Is.EqualTo(2));
    }

    [Test]
    public void AddLibro_DovrebbeAggiungereLibro()
    {
        // Arrange
        var nuovoLibro = new Libro { Id = 3, Titolo = "Libro 3", Autore = "Autore 3", ISBN = "1111111111111", Prezzo = 20 };

        // Act
        _libroRepository.AddLibro(nuovoLibro);

        // Assert
        Assert.That(_libriMock.Count, Is.EqualTo(3));
        _mockContext.Verify(m => m.SaveChanges(), Times.Once);
    }

    [Test]
    public void DeleteLibro_DovrebbeRimuovereLibro()
    {
        // Arrange
        int libroId = 1;
        var libroDaRimuovere = _libriMock.FirstOrDefault(l => l.Id == libroId);

        // Act
        _libroRepository.DeleteLibro(libroId);

        // Assert
        Assert.That(_libriMock.Count, Is.EqualTo(1));
        Assert.That(!_libriMock.Contains(libroDaRimuovere));
    }
}


    public static class MockDbSetHelper
    {
        public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator);

            mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(sourceList.Add);
            mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>(t => sourceList.Remove(t));

            return mockSet;
        }
    }


}
