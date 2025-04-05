using NUnit.Framework;
using Moq;
using Microsoft.EntityFrameworkCore;
using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Test.Repositorio_Test
{
    [TestFixture]
    public class BitacoraRepositorio_Test
    {
        private ApplicationDbContext _dbContext;
        private BitacoraRepositorio _bitacoraRepositorio;

        [SetUp]
        public void Setup()
        {
            // Configurar la base de datos en memoria
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);

            // Inicializar el repositorio
            _bitacoraRepositorio = new BitacoraRepositorio(_dbContext);
        }

        [TearDown]
        public void Teardown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task ObtenerBitacoras_DebeDevolverTodasLasBitacoras()
        {
            // Arrange
            var bitacoras = new List<Bitacora>
            {
                new Bitacora { Id = 1, Descripcion = "Bitacora 1", UsuarioId = "user1" },
                new Bitacora { Id = 2, Descripcion = "Bitacora 2", UsuarioId = "user2" },
                new Bitacora { Id = 3, Descripcion = "Bitacora 3", UsuarioId = "user3" }
            };

            _dbContext.Bitacora.AddRange(bitacoras);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = _bitacoraRepositorio.ObtenerBitacoras().ToList();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Bitacora 1", result[0].Descripcion);
            Assert.AreEqual("Bitacora 2", result[1].Descripcion);
            Assert.AreEqual("Bitacora 3", result[2].Descripcion);
        }

        [Test]
        public async Task Agregar_DebeAgregarBitacora()
        {
            // Arrange
            var bitacora = new Bitacora { Id = 4, Descripcion = "Bitacora 4", UsuarioId = "user4" };

            // Act
            await _bitacoraRepositorio.Agregar(bitacora);
            await _dbContext.SaveChangesAsync();
            var result = await _bitacoraRepositorio.Obtener(bitacora.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Bitacora 4", result.Descripcion);
        }

        [Test]
        public async Task Obtener_DebeDevolverBitacoraPorId()
        {
            // Arrange
            var bitacora = new Bitacora { Id = 5, Descripcion = "Bitacora 5", UsuarioId = "user5" };
            _dbContext.Bitacora.Add(bitacora);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _bitacoraRepositorio.Obtener(5);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Bitacora 5", result.Descripcion);
        }

        [Test]
        public async Task ObtenerTodos_DebeDevolverTodasLasBitacoras()
        {
            // Arrange
            var bitacoras = new List<Bitacora>
            {
                new Bitacora { Id = 6, Descripcion = "Bitacora 6", UsuarioId = "user6" },
                new Bitacora { Id = 7, Descripcion = "Bitacora 7", UsuarioId = "user7" }
            };
            _dbContext.Bitacora.AddRange(bitacoras);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _bitacoraRepositorio.ObtenerTodos();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task ObtenerPrimero_DebeDevolverPrimeraBitacoraQueCumpleElFiltro()
        {
            // Arrange
            var bitacoras = new List<Bitacora>
            {
                new Bitacora { Id = 8, Descripcion = "Bitacora 8", UsuarioId = "user8" },
                new Bitacora { Id = 9, Descripcion = "Bitacora 9", UsuarioId = "user9" }
            };
            _dbContext.Bitacora.AddRange(bitacoras);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _bitacoraRepositorio.ObtenerPrimero(b => b.Descripcion.Contains("9"));

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Bitacora 9", result.Descripcion);
        }

        [Test]
        public async Task Remover_DebeEliminarBitacora()
        {
            // Arrange
            var bitacora = new Bitacora { Id = 10, Descripcion = "Bitacora 10", UsuarioId = "user10" };
            _dbContext.Bitacora.Add(bitacora);
            await _dbContext.SaveChangesAsync();

            // Act
            _bitacoraRepositorio.Remover(bitacora);
            await _dbContext.SaveChangesAsync();
            var result = await _bitacoraRepositorio.Obtener(10);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task RemoverRango_DebeEliminarVariasBitacoras()
        {
            // Arrange
            var bitacoras = new List<Bitacora>
            {
                new Bitacora { Id = 11, Descripcion = "Bitacora 11", UsuarioId = "user11" },
                new Bitacora { Id = 12, Descripcion = "Bitacora 12", UsuarioId = "user12" }
            };
            _dbContext.Bitacora.AddRange(bitacoras);
            await _dbContext.SaveChangesAsync();

            // Act
            _bitacoraRepositorio.RemoverRango(bitacoras);
            await _dbContext.SaveChangesAsync();
            var result1 = await _bitacoraRepositorio.Obtener(11);
            var result2 = await _bitacoraRepositorio.Obtener(12);

            // Assert
            Assert.IsNull(result1);
            Assert.IsNull(result2);
        }

        
    }
}
