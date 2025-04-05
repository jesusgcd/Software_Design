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
    public class CarroCompraRepositorio_Test
    {
        private ApplicationDbContext _dbContext;
        private CarroCompraRepositorio _carroCompraRepositorio;

        [SetUp]
        public void Setup()
        {
            // Configurar la base de datos en memoria
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);

            // Inicializar el repositorio
            _carroCompraRepositorio = new CarroCompraRepositorio(_dbContext);
        }

        [Test]
        public async Task Agregar_DebeAgregarCarroCompra()
        {
            // Arrange
            var carroCompra = new CarroCompra { Id = 1, ProductoId = 1, PrecioId = 1, Cantidad = 2, SesionUsuario = "user1" };

            // Act
            await _carroCompraRepositorio.Agregar(carroCompra);
            await _dbContext.SaveChangesAsync();
            var result = await _carroCompraRepositorio.Obtener(carroCompra.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ProductoId);
        }

        [Test]
        public async Task Obtener_DebeDevolverCarroCompraPorId()
        {
            // Arrange
            var carroCompra = new CarroCompra { Id = 2, ProductoId = 2, PrecioId = 2, Cantidad = 3, SesionUsuario = "user2" };
            _dbContext.CarroCompra.Add(carroCompra);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _carroCompraRepositorio.Obtener(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ProductoId);
        }

        [Test]
        public async Task ObtenerTodos_DebeDevolverTodosLosCarrosCompra()
        {
            // Arrange
            var carrosCompra = new List<CarroCompra>
            {
                new CarroCompra { Id = 3, ProductoId = 3, PrecioId = 3, Cantidad = 1, SesionUsuario = "user3" },
                new CarroCompra { Id = 4, ProductoId = 4, PrecioId = 4, Cantidad = 2, SesionUsuario = "user4" }
            };
            _dbContext.CarroCompra.AddRange(carrosCompra);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _carroCompraRepositorio.ObtenerTodos();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task ObtenerPrimero_DebeDevolverPrimeroQueCumpleElFiltro()
        {
            // Arrange
            var carrosCompra = new List<CarroCompra>
            {
                new CarroCompra { Id = 5, ProductoId = 5, PrecioId = 5, Cantidad = 1, SesionUsuario = "user5" },
                new CarroCompra { Id = 6, ProductoId = 6, PrecioId = 6, Cantidad = 2, SesionUsuario = "user6" }
            };
            _dbContext.CarroCompra.AddRange(carrosCompra);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _carroCompraRepositorio.ObtenerPrimero(c => c.ProductoId == 6);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.ProductoId);
        }

        [Test]
        public async Task Remover_DebeEliminarCarroCompra()
        {
            // Arrange
            var carroCompra = new CarroCompra { Id = 7, ProductoId = 7, PrecioId = 7, Cantidad = 1, SesionUsuario = "user7" };
            _dbContext.CarroCompra.Add(carroCompra);
            await _dbContext.SaveChangesAsync();

            // Act
            _carroCompraRepositorio.Remover(carroCompra);
            await _dbContext.SaveChangesAsync();
            var result = await _carroCompraRepositorio.Obtener(7);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task RemoverRango_DebeEliminarVariosCarrosCompra()
        {
            // Arrange
            var carrosCompra = new List<CarroCompra>
            {
                new CarroCompra { Id = 8, ProductoId = 8, PrecioId = 8, Cantidad = 1, SesionUsuario = "user8" },
                new CarroCompra { Id = 9, ProductoId = 9, PrecioId = 9, Cantidad = 2, SesionUsuario = "user9" }
            };
            _dbContext.CarroCompra.AddRange(carrosCompra);
            await _dbContext.SaveChangesAsync();

            // Act
            _carroCompraRepositorio.RemoverRango(carrosCompra);
            await _dbContext.SaveChangesAsync();
            var result1 = await _carroCompraRepositorio.Obtener(8);
            var result2 = await _carroCompraRepositorio.Obtener(9);

            // Assert
            Assert.IsNull(result1);
            Assert.IsNull(result2);
        }

        [Test]
        public void Actualizar_DebeActualizarCarroCompra()
        {
            // Arrange
            var carroCompra = new CarroCompra { Id = 10, ProductoId = 10, PrecioId = 10, Cantidad = 1, SesionUsuario = "user10" };
            _dbContext.CarroCompra.Add(carroCompra);
            _dbContext.SaveChanges();

            // Act
            carroCompra.ProductoId = 11;
            carroCompra.PrecioId = 11;
            carroCompra.Cantidad = 2;
            _carroCompraRepositorio.Actualizar(carroCompra);
            var result = _dbContext.CarroCompra.FirstOrDefault(c => c.Id == carroCompra.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(11, result.ProductoId);
            Assert.AreEqual(11, result.PrecioId);
            Assert.AreEqual(2, result.Cantidad);
        }

        [TearDown]
        public void Teardown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
