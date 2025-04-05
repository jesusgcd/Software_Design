using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio;
using EFood.Modelos;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EFood.AccesoDatos.Test.Repositorio_Test
{
    [TestFixture]
    public class ProcesadorPagoRepositorio_Test
    {
        private ApplicationDbContext _context;
        private IProcesadorPagoRepositorio _procesadorPagoRepositorio;

        [SetUp]
        public void Setup()
        {
            // Configuración del contexto en memoria para las pruebas
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);
            _procesadorPagoRepositorio = new ProcesadorPagoRepositorio(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Agregar_DebeAgregarNuevoProcesadorPago()
        {
            // Arrange
            var nuevoProcesadorPago = new ProcesadorPago
            {
                Nombre = "PayPal",
                RequiereVerificacion = true,
                MetodoVerificacion = "Email",
                Estado = true,
                PasarelaPago = "https://paypal.com",
                MetodoPagoId = 1
            };

            // Act
            await _procesadorPagoRepositorio.Agregar(nuevoProcesadorPago);
            await _context.SaveChangesAsync();

            // Assert
            var procesadorPagoAgregado = await _context.ProcesadorPago.FindAsync(nuevoProcesadorPago.Id);
            Assert.IsNotNull(procesadorPagoAgregado);
            Assert.AreEqual("PayPal", procesadorPagoAgregado.Nombre);
        }

        [Test]
        public async Task Actualizar_DebeActualizarProcesadorPagoExistente()
        {
            // Arrange: Agregar un procesador de pago inicial al contexto
            var procesadorPagoInicial = new ProcesadorPago
            {
                Nombre = "Stripe",
                RequiereVerificacion = false,
                MetodoVerificacion = "",
                Estado = true,
                PasarelaPago = "https://stripe.com",
                MetodoPagoId = 2
            };
            await _context.AddAsync(procesadorPagoInicial);
            await _context.SaveChangesAsync();

            // Act: Actualizar el procesador de pago
            procesadorPagoInicial.Nombre = "Stripe Updated";
            _procesadorPagoRepositorio.Actualizar(procesadorPagoInicial);
            await _context.SaveChangesAsync();

            // Assert: Verificar que el procesador de pago se ha actualizado correctamente
            var procesadorPagoActualizado = await _context.ProcesadorPago.FindAsync(procesadorPagoInicial.Id);
            Assert.IsNotNull(procesadorPagoActualizado);
            Assert.AreEqual("Stripe Updated", procesadorPagoActualizado.Nombre);
        }

        [Test]
        public async Task Remover_DebeEliminarProcesadorPagoExistente()
        {
            // Arrange: Agregar un procesador de pago inicial al contexto
            var procesadorPagoInicial = new ProcesadorPago
            {
                Nombre = "Square",
                RequiereVerificacion = false,
                MetodoVerificacion = "",
                Estado = true,
                PasarelaPago = "https://square.com",
                MetodoPagoId = 3
            };
            await _context.AddAsync(procesadorPagoInicial);
            await _context.SaveChangesAsync();

            // Act: Eliminar el procesador de pago
            _procesadorPagoRepositorio.Remover(procesadorPagoInicial);
            await _context.SaveChangesAsync();

            // Assert: Verificar que el procesador de pago se ha eliminado correctamente
            var procesadorPagoEliminado = await _context.ProcesadorPago.FindAsync(procesadorPagoInicial.Id);
            Assert.IsNull(procesadorPagoEliminado);
        }

        [Test]
        public async Task ObtenerProcesadoresPagoPorMetodo_DebeRetornarListaDeProcesadoresPago()
        {
            // Arrange: Agregar varios procesadores de pago al contexto para un método específico
            var metodoPago = new MetodoPago { Nombre = "Tarjeta de Crédito" };
            await _context.MetodoPago.AddAsync(metodoPago);
            await _context.SaveChangesAsync();

            var procesador1 = new ProcesadorPago { Nombre = "Visa", RequiereVerificacion = false, MetodoVerificacion = "", Estado = true, PasarelaPago = "https://visa.com", MetodoPagoId = metodoPago.Id };
            var procesador2 = new ProcesadorPago { Nombre = "MasterCard", RequiereVerificacion = false, MetodoVerificacion = "", Estado = true, PasarelaPago = "https://mastercard.com", MetodoPagoId = metodoPago.Id };

            await _context.ProcesadorPago.AddRangeAsync(procesador1, procesador2);
            await _context.SaveChangesAsync();

            // Act: Obtener procesadores de pago por método de pago
            var procesadores = await _procesadorPagoRepositorio.ObtenerTodos(p => p.MetodoPagoId == metodoPago.Id);

            // Assert: Verificar que se han obtenido los procesadores esperados
            Assert.IsNotNull(procesadores);
            Assert.AreEqual(2, procesadores.Count());
            Assert.IsTrue(procesadores.Any(p => p.Nombre == "Visa"));
            Assert.IsTrue(procesadores.Any(p => p.Nombre == "MasterCard"));
        }

        [Test]
        public async Task Obtener_DebeRetornarProcesadorPagoPorId()
        {
            // Arrange: Agregar un procesador de pago al contexto
            var procesadorPago = new ProcesadorPago
            {
                Nombre = "American Express",
                RequiereVerificacion = true,
                MetodoVerificacion = "SMS",
                Estado = true,
                PasarelaPago = "https://americanexpress.com",
                MetodoPagoId = 4
            };
            await _context.ProcesadorPago.AddAsync(procesadorPago);
            await _context.SaveChangesAsync();

            // Act: Obtener el procesador de pago por ID
            var procesadorPagoObtenido = await _procesadorPagoRepositorio.Obtener(procesadorPago.Id);

            // Assert: Verificar que se ha obtenido el procesador de pago correcto
            Assert.IsNotNull(procesadorPagoObtenido);
            Assert.AreEqual(procesadorPago.Id, procesadorPagoObtenido.Id);
        }

        [Test]
        public async Task ObtenerTodos_DebeRetornarTodosLosProcesadoresPago()
        {
            // Arrange: Agregar varios procesadores de pago al contexto
            var procesador1 = new ProcesadorPago { Nombre = "Visa", RequiereVerificacion = false, MetodoVerificacion = "", Estado = true, PasarelaPago = "https://visa.com", MetodoPagoId = 5 };
            var procesador2 = new ProcesadorPago { Nombre = "MasterCard", RequiereVerificacion = false, MetodoVerificacion = "", Estado = true, PasarelaPago = "https://mastercard.com", MetodoPagoId = 6 };
            await _context.ProcesadorPago.AddRangeAsync(procesador1, procesador2);
            await _context.SaveChangesAsync();

            // Act: Obtener todos los procesadores de pago
            var procesadores = await _procesadorPagoRepositorio.ObtenerTodos();

            // Assert: Verificar que se han obtenido todos los procesadores de pago
            Assert.IsNotNull(procesadores);
            Assert.AreEqual(2, procesadores.Count());
        }

        [Test]
        public async Task ObtenerPrimero_DebeRetornarElPrimerProcesadorPagoQueCumpleConElFiltro()
        {
            // Arrange: Agregar varios procesadores de pago al contexto
            var procesador1 = new ProcesadorPago { Nombre = "Visa", RequiereVerificacion = false, MetodoVerificacion = "", Estado = true, PasarelaPago = "https://visa.com", MetodoPagoId = 7 };
            var procesador2 = new ProcesadorPago { Nombre = "MasterCard", RequiereVerificacion = false, MetodoVerificacion = "", Estado = true, PasarelaPago = "https://mastercard.com", MetodoPagoId = 8 };
            await _context.ProcesadorPago.AddRangeAsync(procesador1, procesador2);
            await _context.SaveChangesAsync();

            // Act: Obtener el primer procesador de pago que cumple con el filtro
            var primerProcesadorPago = await _procesadorPagoRepositorio.ObtenerPrimero(p => p.Nombre == "Visa");

            // Assert: Verificar que se ha obtenido el procesador de pago correcto
            Assert.IsNotNull(primerProcesadorPago);
            Assert.AreEqual(procesador1.Id, primerProcesadorPago.Id);
        }

        [Test]
        public async Task RemoverRango_DebeEliminarVariosProcesadoresPago()
        {
            // Arrange: Agregar varios procesadores de pago al contexto
            var procesador1 = new ProcesadorPago { Nombre = "Discover", RequiereVerificacion = true, MetodoVerificacion = "Email", Estado = true, PasarelaPago = "https://discover.com", MetodoPagoId = 9 };
            var procesador2 = new ProcesadorPago { Nombre = "JCB", RequiereVerificacion = true, MetodoVerificacion = "SMS", Estado = true, PasarelaPago = "https://jcb.com", MetodoPagoId = 10 };
            await _context.ProcesadorPago.AddRangeAsync(procesador1, procesador2);
            await _context.SaveChangesAsync();

            // Act: Eliminar los procesadores de pago
            _procesadorPagoRepositorio.RemoverRango(new List<ProcesadorPago> { procesador1, procesador2 });
            await _context.SaveChangesAsync();

            // Assert: Verificar que los procesadores de pago se han eliminado correctamente
            var procesadorPagoEliminado1 = await _context.ProcesadorPago.FindAsync(procesador1.Id);
            var procesadorPagoEliminado2 = await _context.ProcesadorPago.FindAsync(procesador2.Id);
            Assert.IsNull(procesadorPagoEliminado1);
            Assert.IsNull(procesadorPagoEliminado2);
        }

        [Test]
        public void ObtenerTodosDropdownLista_DebeRetornarSelectListItemsParaMetodoPago()
        {
            // Arrange: Agregar varios métodos de pago al contexto
            var metodoPago1 = new MetodoPago { Nombre = "Tarjeta de Crédito" };
            var metodoPago2 = new MetodoPago { Nombre = "Tarjeta de Débito" };
            _context.MetodoPago.AddRange(metodoPago1, metodoPago2);
            _context.SaveChanges();

            // Act: Obtener todos los métodos de pago como SelectListItem
            var result = _procesadorPagoRepositorio.ObtenerTodosDropdownLista("MetodoPago");

            // Assert: Verificar que se han obtenido los métodos de pago esperados
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(i => i.Text == "Tarjeta de Crédito" && i.Value == metodoPago1.Id.ToString()));
            Assert.IsTrue(result.Any(i => i.Text == "Tarjeta de Débito" && i.Value == metodoPago2.Id.ToString()));
        }

        [Test]
        public void ObtenerTodosDropdownLista_DebeLanzarExcepcionParaParametroInvalido()
        {
            // Act & Assert: Verificar que se lanza una excepción cuando el parámetro es nulo o vacío
            Assert.Throws<ArgumentNullException>(() => _procesadorPagoRepositorio.ObtenerTodosDropdownLista(null));
            Assert.Throws<ArgumentNullException>(() => _procesadorPagoRepositorio.ObtenerTodosDropdownLista(""));
        }
    }
}
