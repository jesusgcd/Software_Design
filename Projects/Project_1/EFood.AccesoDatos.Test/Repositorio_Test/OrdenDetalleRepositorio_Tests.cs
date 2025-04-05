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
    public class OrdenDetalleRepositorio_Tests
    {
        private ApplicationDbContext _dbContext;
        private OrdenDetalleRepositorio _ordenDetalleRepositorio;

        [SetUp]
        public void Setup()
        {
            // Configurar la base de datos en memoria
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);

            // Inicializar el repositorio
            _ordenDetalleRepositorio = new OrdenDetalleRepositorio(_dbContext);
        }

        [Test]
        public async Task Agregar_DebeAgregarOrdenDetalle()
        {
            // Arrange
            var ordenDetalle = new OrdenDetalle { Id = 1, OrdenId = 1, ProductoId = 1, TipoPrecioId = 1, Cantidad = 1, Monto = 10.0 };

            // Act
            await _ordenDetalleRepositorio.Agregar(ordenDetalle);
            await _dbContext.SaveChangesAsync();
            var result = await _ordenDetalleRepositorio.Obtener(ordenDetalle.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.OrdenId);
        }

        [Test]
        public async Task Obtener_DebeDevolverOrdenDetallePorId()
        {
            // Arrange
            var ordenDetalle = new OrdenDetalle { Id = 2, OrdenId = 2, ProductoId = 2, TipoPrecioId = 2, Cantidad = 2, Monto = 20.0 };
            _dbContext.Set<OrdenDetalle>().Add(ordenDetalle);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _ordenDetalleRepositorio.Obtener(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.OrdenId);
        }

        [Test]
        public async Task ObtenerTodos_DebeDevolverTodosLosOrdenesDetalle()
        {
            // Arrange
            var ordenDetalles = new List<OrdenDetalle>
            {
                new OrdenDetalle { Id = 3, OrdenId = 3, ProductoId = 3, TipoPrecioId = 3, Cantidad = 3, Monto = 30.0 },
                new OrdenDetalle { Id = 4, OrdenId = 4, ProductoId = 4, TipoPrecioId = 4, Cantidad = 4, Monto = 40.0 }
            };
            _dbContext.Set<OrdenDetalle>().AddRange(ordenDetalles);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _ordenDetalleRepositorio.ObtenerTodos();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task ObtenerPrimero_DebeDevolverPrimerOrdenDetalleQueCumpleElFiltro()
        {
            // Arrange
            var ordenDetalles = new List<OrdenDetalle>
            {
                new OrdenDetalle { Id = 5, OrdenId = 5, ProductoId = 5, TipoPrecioId = 5, Cantidad = 5, Monto = 50.0 },
                new OrdenDetalle { Id = 6, OrdenId = 6, ProductoId = 6, TipoPrecioId = 6, Cantidad = 6, Monto = 60.0 }
            };
            _dbContext.Set<OrdenDetalle>().AddRange(ordenDetalles);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _ordenDetalleRepositorio.ObtenerPrimero(e => e.OrdenId == 6);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(6, result.OrdenId);
        }

        [Test]
        public void Actualizar_DebeActualizarOrdenDetalle()
        {
            // Arrange
            var ordenDetalle = new OrdenDetalle { Id = 7, OrdenId = 7, ProductoId = 7, TipoPrecioId = 7, Cantidad = 7, Monto = 70.0 };
            _dbContext.Set<OrdenDetalle>().Add(ordenDetalle);
            _dbContext.SaveChanges();

            // Act
            ordenDetalle.Cantidad = 10;
            _ordenDetalleRepositorio.Actualizar(ordenDetalle);
            var result = _ordenDetalleRepositorio.Obtener(ordenDetalle.Id).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Cantidad);
        }

        [Test]
        public async Task Remover_DebeEliminarOrdenDetalle()
        {
            // Arrange
            var ordenDetalle = new OrdenDetalle { Id = 8, OrdenId = 8, ProductoId = 8, TipoPrecioId = 8, Cantidad = 8, Monto = 80.0 };
            _dbContext.Set<OrdenDetalle>().Add(ordenDetalle);
            await _dbContext.SaveChangesAsync();

            // Act
            _ordenDetalleRepositorio.Remover(ordenDetalle);
            await _dbContext.SaveChangesAsync();
            var result = await _ordenDetalleRepositorio.Obtener(8);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task RemoverRango_DebeEliminarVariosOrdenesDetalle()
        {
            // Arrange
            var ordenDetalles = new List<OrdenDetalle>
            {
                new OrdenDetalle { Id = 9, OrdenId = 9, ProductoId = 9, TipoPrecioId = 9, Cantidad = 9, Monto = 90.0 },
                new OrdenDetalle { Id = 10, OrdenId = 10, ProductoId = 10, TipoPrecioId = 10, Cantidad = 10, Monto = 100.0 }
            };
            _dbContext.Set<OrdenDetalle>().AddRange(ordenDetalles);
            await _dbContext.SaveChangesAsync();

            // Act
            _ordenDetalleRepositorio.RemoverRango(ordenDetalles);
            await _dbContext.SaveChangesAsync();
            var result1 = await _ordenDetalleRepositorio.Obtener(9);
            var result2 = await _ordenDetalleRepositorio.Obtener(10);

            // Assert
            Assert.IsNull(result1);
            Assert.IsNull(result2);
        }

        [Test]
        public async Task ObteneOrdenDetallePorOrdenId_DebeDevolverOrdenesDetallePorOrdenId()
        {
            // Arrange
            var usuario = new Usuario { Nombre = "Usuario", Apellidos = "Prueba", PhoneNumber = "123456789" };

            var orden = new Orden
            {
                NombreCliente = usuario.Nombre,
                ApellidosCliente = usuario.Apellidos,
                Direccion = "Dirección de Prueba",
                Estado = "Estado de Prueba",
                TelefonoCliente = usuario.PhoneNumber  // Asegúrate de que el usuario tenga un número de teléfono válido
            };

            _dbContext.Orden.Add(orden);

            var producto1 = new Producto { Id = 11, Nombre = "Producto 1" , Descripcion = "Producto 1"};
            var producto2 = new Producto { Id = 12, Nombre = "Producto 2", Descripcion = "Producto 2" };
            var tipoPrecio = new TipoPrecio { Id = 11, Nombre = "TipoPrecio 1"};

            _dbContext.Producto.Add(producto1);
            _dbContext.Producto.Add(producto2);
            _dbContext.TipoPrecio.Add(tipoPrecio);

            await _dbContext.SaveChangesAsync();

            var ordenDetalles = new List<OrdenDetalle>
    {
        new OrdenDetalle { OrdenId = orden.Id, ProductoId = producto1.Id, TipoPrecioId = tipoPrecio.Id, Cantidad = 11, Monto = 110.0 },
        new OrdenDetalle { OrdenId = orden.Id, ProductoId = producto2.Id, TipoPrecioId = tipoPrecio.Id, Cantidad = 12, Monto = 120.0 }
    };

            _dbContext.OrdenDetalle.AddRange(ordenDetalles);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _ordenDetalleRepositorio.ObteneOrdenDetallePorOrdenId(orden.Id);

            // Assert
            Assert.AreEqual(2, result.Count);
        }


        [TearDown]
        public void Teardown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}