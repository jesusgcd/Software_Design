using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio;
using EFood.Modelos;
using EFood.AccesoDatos.Repositorio.IRepositorio;

namespace EFood.AccesoDatos.Test.Repositorio_Test
{
    [TestFixture]
    public class PrecioRepositorio_Test
    {
        private ApplicationDbContext _context;
        private IPrecioRepositorio _precioRepositorio;

        [SetUp]
        public void Setup()
        {
            // Configuración del contexto en memoria para las pruebas
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);
            _precioRepositorio = new PrecioRepositorio(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Agregar_DebeAgregarNuevoPrecio()
        {
            // Arrange
            var nuevoPrecio = new Precio { ProductoID = 1, TipoPrecioID = 1, Monto = 10.0 };

            // Act
            await _precioRepositorio.Agregar(nuevoPrecio);
            await _context.SaveChangesAsync();

            // Assert
            var precioAgregado = await _context.Precio.FindAsync(nuevoPrecio.Id);
            Assert.IsNotNull(precioAgregado);
            Assert.AreEqual(1, precioAgregado.ProductoID);
            Assert.AreEqual(1, precioAgregado.TipoPrecioID);
            Assert.AreEqual(10.0, precioAgregado.Monto);
        }

        [Test]
        public async Task Actualizar_DebeActualizarPrecioExistente()
        {
            // Arrange: Agregar un precio inicial al contexto
            var precioInicial = new Precio { ProductoID = 2, TipoPrecioID = 2, Monto = 15.0 };
            await _context.AddAsync(precioInicial);
            await _context.SaveChangesAsync();

            // Act: Actualizar el precio
            precioInicial.Monto = 20.0;
            _precioRepositorio.Actualizar(precioInicial);
            await _context.SaveChangesAsync();

            // Assert: Verificar que el precio se ha actualizado correctamente
            var precioActualizado = await _context.Precio.FindAsync(precioInicial.Id);
            Assert.IsNotNull(precioActualizado);
            Assert.AreEqual(20.0, precioActualizado.Monto);
        }

        [Test]
        public async Task Remover_DebeEliminarPrecioExistente()
        {
            // Arrange: Agregar un precio inicial al contexto
            var precioInicial = new Precio { ProductoID = 3, TipoPrecioID = 3, Monto = 25.0 };
            await _context.AddAsync(precioInicial);
            await _context.SaveChangesAsync();

            // Act: Eliminar el precio
            _precioRepositorio.Remover(precioInicial);
            await _context.SaveChangesAsync();

            // Assert: Verificar que el precio se ha eliminado correctamente
            var precioEliminado = await _context.Precio.FindAsync(precioInicial.Id);
            Assert.IsNull(precioEliminado);
        }

        [Test]
        public async Task ObtenerPreciosPorProducto_DebeRetornarListaDePrecios()
        {
            // Arrange: Agregar varios precios al contexto para un producto específico
            var producto = new Producto { Nombre = "Producto1", Descripcion = "Descripcion1" };
            var tipoPrecio1 = new TipoPrecio { Nombre = "Tipo1" };
            var tipoPrecio2 = new TipoPrecio { Nombre = "Tipo2" };
            

            // Agregar los productos y tipos de precios primero
            await _context.Producto.AddAsync(producto);
            await _context.TipoPrecio.AddRangeAsync(tipoPrecio1, tipoPrecio2);
            await _context.SaveChangesAsync();

            var precio1 = new Precio { ProductoID = producto.Id, TipoPrecioID = tipoPrecio1.Id, Monto = 10.0 };
            var precio2 = new Precio { ProductoID = producto.Id, TipoPrecioID = tipoPrecio2.Id, Monto = 15.0 };

            // Agregar precios
            await _context.Precio.AddRangeAsync(precio1, precio2);
            await _context.SaveChangesAsync();

            // Act: Obtener precios por producto
            var precios = await _precioRepositorio.ObtenerPreciosPorProducto(producto.Id);

            // Assert: Verificar que se han obtenido los precios esperados
            Assert.IsNotNull(precios);
            Assert.AreEqual(2, precios.Count());
            Assert.IsTrue(precios.Any(p => p.Monto == 10.0));
            Assert.IsTrue(precios.Any(p => p.Monto == 15.0));
        }
        [Test]
        public async Task Obtener_DebeRetornarPrecioPorId()
        {
            // Arrange: Agregar un precio al contexto
            var precio = new Precio { ProductoID = 4, TipoPrecioID = 4, Monto = 30.0 };
            await _context.Precio.AddAsync(precio);
            await _context.SaveChangesAsync();

            // Act: Obtener el precio por ID
            var precioObtenido = await _precioRepositorio.Obtener(precio.Id);

            // Assert: Verificar que se ha obtenido el precio correcto
            Assert.IsNotNull(precioObtenido);
            Assert.AreEqual(precio.Id, precioObtenido.Id);
        }

        [Test]
        public async Task ObtenerTodos_DebeRetornarTodosLosPrecios()
        {
            // Arrange: Agregar varios precios al contexto
            var precio1 = new Precio { ProductoID = 5, TipoPrecioID = 5, Monto = 35.0 };
            var precio2 = new Precio { ProductoID = 6, TipoPrecioID = 6, Monto = 40.0 };
            await _context.Precio.AddRangeAsync(precio1, precio2);
            await _context.SaveChangesAsync();

            // Act: Obtener todos los precios
            var precios = await _precioRepositorio.ObtenerTodos();

            // Assert: Verificar que se han obtenido todos los precios
            Assert.IsNotNull(precios);
            Assert.AreEqual(2, precios.Count());
        }

        [Test]
        public async Task ObtenerPrimero_DebeRetornarElPrimerPrecioQueCumpleConElFiltro()
        {
            // Arrange: Agregar varios precios al contexto
            var precio1 = new Precio { ProductoID = 7, TipoPrecioID = 7, Monto = 45.0 };
            var precio2 = new Precio { ProductoID = 8, TipoPrecioID = 8, Monto = 50.0 };
            await _context.Precio.AddRangeAsync(precio1, precio2);
            await _context.SaveChangesAsync();

            // Act: Obtener el primer precio que cumple con el filtro
            var primerPrecio = await _precioRepositorio.ObtenerPrimero(p => p.Monto > 40.0);

            // Assert: Verificar que se ha obtenido el precio correcto
            Assert.IsNotNull(primerPrecio);
            Assert.AreEqual(precio1.Id, primerPrecio.Id);
        }

        [Test]
        public async Task RemoverRango_DebeEliminarVariosPrecios()
        {
            // Arrange: Agregar varios precios al contexto
            var precio1 = new Precio { ProductoID = 9, TipoPrecioID = 9, Monto = 55.0 };
            var precio2 = new Precio { ProductoID = 10, TipoPrecioID = 10, Monto = 60.0 };
            await _context.Precio.AddRangeAsync(precio1, precio2);
            await _context.SaveChangesAsync();

            // Act: Eliminar varios precios
            _precioRepositorio.RemoverRango(new List<Precio> { precio1, precio2 });
            await _context.SaveChangesAsync();

            // Assert: Verificar que los precios se han eliminado correctamente
            var precioEliminado1 = await _context.Precio.FindAsync(precio1.Id);
            var precioEliminado2 = await _context.Precio.FindAsync(precio2.Id);
            Assert.IsNull(precioEliminado1);
            Assert.IsNull(precioEliminado2);
        }
    }
}
