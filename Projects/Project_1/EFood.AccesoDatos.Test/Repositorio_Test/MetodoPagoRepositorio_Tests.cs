using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio;
using EFood.Modelos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Test.Repositorio_Test
{
    [TestFixture]
    public class MetodoPagoRepositorio_Tests
    {
        private ApplicationDbContext _dbContext;
        private MetodoPagoRepositorio _metodoPagoRepositorio;

        [SetUp]
        public void Setup()
        {
            // Configurar la base de datos en memoria
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);

            // Inicializar el repositorio
            _metodoPagoRepositorio = new MetodoPagoRepositorio(_dbContext);
        }

        [Test]
        public async Task Agregar_DebeAgregarMetodoPago()
        {
            // Arrange
            var metodoPago = new MetodoPago { Id = 1, Nombre = "Pago 1" };

            // Act
            await _metodoPagoRepositorio.Agregar(metodoPago);
            await _dbContext.SaveChangesAsync();
            var result = await _metodoPagoRepositorio.Obtener(metodoPago.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Pago 1", result.Nombre);
        }

        [Test]
        public async Task Obtener_DebeDevolverMetodoPagoPorId()
        {
            // Arrange
            var metodoPago = new MetodoPago { Id = 2, Nombre = "Pago 2" };
            _dbContext.Set<MetodoPago>().Add(metodoPago);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _metodoPagoRepositorio.Obtener(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Pago 2", result.Nombre);
        }

        [Test]
        public async Task ObtenerTodos_DebeDevolverTodosLosMetodosPago()
        {
            // Arrange
            var metodoPagos = new List<MetodoPago>
            {
                new MetodoPago { Id = 3, Nombre = "Pago 3" },
                new MetodoPago { Id = 4, Nombre = "Pago 4" }
            };
            _dbContext.Set<MetodoPago>().AddRange(metodoPagos);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _metodoPagoRepositorio.ObtenerTodos();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task ObtenerPrimero_DebeDevolverPrimerMetodoPagoQueCumpleElFiltro()
        {
            // Arrange
            var metodoPagos = new List<MetodoPago>
            {
                new MetodoPago { Id = 5, Nombre = "Pago 5" },
                new MetodoPago { Id = 6, Nombre = "Pago 6" }
            };
            _dbContext.Set<MetodoPago>().AddRange(metodoPagos);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _metodoPagoRepositorio.ObtenerPrimero(e => e.Nombre.Contains("6"));

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Pago 6", result.Nombre);
        }

        [Test]
        public void Actualizar_DebeActualizarMetodoPago()
        {
            // Arrange
            var metodoPago = new MetodoPago { Id = 7, Nombre = "Pago 7" };
            _dbContext.Set<MetodoPago>().Add(metodoPago);
            _dbContext.SaveChanges();

            // Act
            metodoPago.Nombre = "Pago 7 Actualizado";
            _metodoPagoRepositorio.Actualizar(metodoPago);
            var result = _metodoPagoRepositorio.Obtener(metodoPago.Id).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Pago 7 Actualizado", result.Nombre);
        }

        [Test]
        public async Task Remover_DebeEliminarMetodoPago()
        {
            // Arrange
            var metodoPago = new MetodoPago { Id = 8, Nombre = "Pago 8" };
            _dbContext.Set<MetodoPago>().Add(metodoPago);
            await _dbContext.SaveChangesAsync();

            // Act
            _metodoPagoRepositorio.Remover(metodoPago);
            await _dbContext.SaveChangesAsync();
            var result = await _metodoPagoRepositorio.Obtener(8);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task RemoverRango_DebeEliminarVariosMetodosPago()
        {
            // Arrange
            var metodoPagos = new List<MetodoPago>
            {
                new MetodoPago { Id = 9, Nombre = "Pago 9" },
                new MetodoPago { Id = 10, Nombre = "Pago 10" }
            };
            _dbContext.Set<MetodoPago>().AddRange(metodoPagos);
            await _dbContext.SaveChangesAsync();

            // Act
            _metodoPagoRepositorio.RemoverRango(metodoPagos);
            await _dbContext.SaveChangesAsync();
            var result1 = await _metodoPagoRepositorio.Obtener(9);
            var result2 = await _metodoPagoRepositorio.Obtener(10);

            // Assert
            Assert.IsNull(result1);
            Assert.IsNull(result2);
        }

        [TearDown]
        public void Teardown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
