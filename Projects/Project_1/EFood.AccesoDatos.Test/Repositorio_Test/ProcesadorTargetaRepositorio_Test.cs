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
using System;

namespace EFood.AccesoDatos.Test.Repositorio_Test
{
    [TestFixture]
    public class ProcesadorTarjetaRepositorio_Test
    {
        private ApplicationDbContext _context;
        private IProcesadorTarjetaRepositorio _procesadorTarjetaRepositorio;

        [SetUp]
        public void Setup()
        {
            // Configuración del contexto en memoria para las pruebas
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);
            _procesadorTarjetaRepositorio = new ProcesadorTarjetaRepositorio(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Agregar_DebeAgregarNuevoProcesadorTarjeta()
        {
            // Arrange
            var procesadorPago = new ProcesadorPago { Nombre = "PayPal", MetodoVerificacion = "Metodo1", PasarelaPago = "Pasarela1" };
            var tipoTarjeta = new TipoTarjeta { Nombre = "Visa" };
            await _context.AddAsync(procesadorPago);
            await _context.AddAsync(tipoTarjeta);
            await _context.SaveChangesAsync();

            var nuevoProcesadorTarjeta = new ProcesadorTarjeta
            {
                ProcesadorPagoId = procesadorPago.Id,
                TipoTarjetaId = tipoTarjeta.Id
            };

            // Act
            await _procesadorTarjetaRepositorio.Agregar(nuevoProcesadorTarjeta);
            await _context.SaveChangesAsync();

            // Assert
            var procesadorTarjetaAgregado = await _context.ProcesadorTarjeta.FindAsync(nuevoProcesadorTarjeta.Id);
            Assert.IsNotNull(procesadorTarjetaAgregado);
            Assert.AreEqual(procesadorPago.Id, procesadorTarjetaAgregado.ProcesadorPagoId);
            Assert.AreEqual(tipoTarjeta.Id, procesadorTarjetaAgregado.TipoTarjetaId);
        }

        [Test]
        public async Task Actualizar_DebeActualizarProcesadorTarjetaExistente()
        {
            // Arrange: Agregar un procesador de tarjeta inicial al contexto
            var procesadorPago = new ProcesadorPago { Nombre = "Stripe", MetodoVerificacion = "Metodo2", PasarelaPago = "Pasarela2" };
            var tipoTarjeta = new TipoTarjeta { Nombre = "MasterCard" };
            await _context.AddAsync(procesadorPago);
            await _context.AddAsync(tipoTarjeta);
            await _context.SaveChangesAsync();

            var procesadorTarjetaInicial = new ProcesadorTarjeta
            {
                ProcesadorPagoId = procesadorPago.Id,
                TipoTarjetaId = tipoTarjeta.Id
            };
            await _context.AddAsync(procesadorTarjetaInicial);
            await _context.SaveChangesAsync();

            // Act: Actualizar el procesador de tarjeta
            var nuevoTipoTarjeta = new TipoTarjeta { Nombre = "American Express" };
            await _context.AddAsync(nuevoTipoTarjeta);
            await _context.SaveChangesAsync();

            procesadorTarjetaInicial.TipoTarjetaId = nuevoTipoTarjeta.Id;
            _procesadorTarjetaRepositorio.Actualizar(procesadorTarjetaInicial);
            await _context.SaveChangesAsync();

            // Assert: Verificar que el procesador de tarjeta se ha actualizado correctamente
            var procesadorTarjetaActualizado = await _context.ProcesadorTarjeta.FindAsync(procesadorTarjetaInicial.Id);
            Assert.IsNotNull(procesadorTarjetaActualizado);
            Assert.AreEqual(nuevoTipoTarjeta.Id, procesadorTarjetaActualizado.TipoTarjetaId);
        }

        [Test]
        public async Task Remover_DebeEliminarProcesadorTarjetaExistente()
        {
            // Arrange: Agregar un procesador de tarjeta inicial al contexto
            var procesadorPago = new ProcesadorPago { Nombre = "Square", MetodoVerificacion = "Metodo3", PasarelaPago = "Pasarela3" };
            var tipoTarjeta = new TipoTarjeta { Nombre = "Discover" };
            await _context.AddAsync(procesadorPago);
            await _context.AddAsync(tipoTarjeta);
            await _context.SaveChangesAsync();

            var procesadorTarjetaInicial = new ProcesadorTarjeta
            {
                ProcesadorPagoId = procesadorPago.Id,
                TipoTarjetaId = tipoTarjeta.Id
            };
            await _context.AddAsync(procesadorTarjetaInicial);
            await _context.SaveChangesAsync();

            // Act: Eliminar el procesador de tarjeta
            _procesadorTarjetaRepositorio.Remover(procesadorTarjetaInicial);
            await _context.SaveChangesAsync();

            // Assert: Verificar que el procesador de tarjeta se ha eliminado correctamente
            var procesadorTarjetaEliminado = await _context.ProcesadorTarjeta.FindAsync(procesadorTarjetaInicial.Id);
            Assert.IsNull(procesadorTarjetaEliminado);
        }

        [Test]
        public async Task ObtenerTarjetasPorProcesadorPago_DebeRetornarListaDeTarjetas()
        {
            // Arrange: Agregar varios procesadores de tarjeta al contexto para un procesador de pago específico
            var procesadorPago = new ProcesadorPago { Nombre = "Visa", MetodoVerificacion = "Metodo4", PasarelaPago = "Pasarela4" };
            var tipoTarjeta1 = new TipoTarjeta { Nombre = "Gold" };
            var tipoTarjeta2 = new TipoTarjeta { Nombre = "Platinum" };

            await _context.AddAsync(procesadorPago);
            await _context.AddAsync(tipoTarjeta1);
            await _context.AddAsync(tipoTarjeta2);
            await _context.SaveChangesAsync();

            var procesadorTarjeta1 = new ProcesadorTarjeta { ProcesadorPagoId = procesadorPago.Id, TipoTarjetaId = tipoTarjeta1.Id };
            var procesadorTarjeta2 = new ProcesadorTarjeta { ProcesadorPagoId = procesadorPago.Id, TipoTarjetaId = tipoTarjeta2.Id };

            await _context.ProcesadorTarjeta.AddRangeAsync(procesadorTarjeta1, procesadorTarjeta2);
            await _context.SaveChangesAsync();

            // Act: Obtener tarjetas por procesador de pago
            var tarjetas = await _procesadorTarjetaRepositorio.ObtenerTarjetasPorProcesadorPago(procesadorPago.Id);

            // Assert: Verificar que se han obtenido las tarjetas esperadas
            Assert.IsNotNull(tarjetas);
            Assert.AreEqual(2, tarjetas.Count);
            Assert.IsTrue(tarjetas.Any(t => t.TipoTarjetaId == tipoTarjeta1.Id));
            Assert.IsTrue(tarjetas.Any(t => t.TipoTarjetaId == tipoTarjeta2.Id));
        }

        [Test]
        public async Task Obtener_DebeRetornarProcesadorTarjetaPorId()
        {
            // Arrange: Agregar un procesador de tarjeta al contexto
            var procesadorPago = new ProcesadorPago { Nombre = "American Express", MetodoVerificacion = "Metodo5", PasarelaPago = "Pasarela5" };
            var tipoTarjeta = new TipoTarjeta { Nombre = "Blue" };
            await _context.AddAsync(procesadorPago);
            await _context.AddAsync(tipoTarjeta);
            await _context.SaveChangesAsync();

            var procesadorTarjeta = new ProcesadorTarjeta
            {
                ProcesadorPagoId = procesadorPago.Id,
                TipoTarjetaId = tipoTarjeta.Id
            };
            await _context.AddAsync(procesadorTarjeta);
            await _context.SaveChangesAsync();

            // Act: Obtener el procesador de tarjeta por ID
            var procesadorTarjetaObtenido = await _procesadorTarjetaRepositorio.Obtener(procesadorTarjeta.Id);

            // Assert: Verificar que se ha obtenido el procesador de tarjeta correcto
            Assert.IsNotNull(procesadorTarjetaObtenido);
            Assert.AreEqual(procesadorTarjeta.Id, procesadorTarjetaObtenido.Id);
        }

        [Test]
        public async Task ObtenerTodos_DebeRetornarTodosLosProcesadoresTarjeta()
        {
            // Arrange: Agregar varios procesadores de tarjeta al contexto
            var procesadorPago1 = new ProcesadorPago { Nombre = "Visa", MetodoVerificacion = "Metodo6", PasarelaPago = "Pasarela6" };
            var procesadorPago2 = new ProcesadorPago { Nombre = "MasterCard", MetodoVerificacion = "Metodo7", PasarelaPago = "Pasarela7" };
            var tipoTarjeta1 = new TipoTarjeta { Nombre = "Gold" };
            var tipoTarjeta2 = new TipoTarjeta { Nombre = "Platinum" };

            await _context.AddAsync(procesadorPago1);
            await _context.AddAsync(procesadorPago2);
            await _context.AddAsync(tipoTarjeta1);
            await _context.AddAsync(tipoTarjeta2);
            await _context.SaveChangesAsync();

            var procesadorTarjeta1 = new ProcesadorTarjeta { ProcesadorPagoId = procesadorPago1.Id, TipoTarjetaId = tipoTarjeta1.Id };
            var procesadorTarjeta2 = new ProcesadorTarjeta { ProcesadorPagoId = procesadorPago2.Id, TipoTarjetaId = tipoTarjeta2.Id };

            await _context.ProcesadorTarjeta.AddRangeAsync(procesadorTarjeta1, procesadorTarjeta2);
            await _context.SaveChangesAsync();

            // Act: Obtener todos los procesadores de tarjeta
            var procesadoresTarjeta = await _procesadorTarjetaRepositorio.ObtenerTodos();

            // Assert: Verificar que se han obtenido todos los procesadores de tarjeta
            Assert.IsNotNull(procesadoresTarjeta);
            Assert.AreEqual(2, procesadoresTarjeta.Count());
        }

        [Test]
        public async Task ObtenerPrimero_DebeRetornarPrimerProcesadorTarjetaQueCumplaCondicion()
        {
            // Arrange: Agregar varios procesadores de tarjeta al contexto
            var procesadorPago1 = new ProcesadorPago { Nombre = "Visa", MetodoVerificacion = "Metodo8", PasarelaPago = "Pasarela8" };
            var procesadorPago2 = new ProcesadorPago { Nombre = "MasterCard", MetodoVerificacion = "Metodo9", PasarelaPago = "Pasarela9" };
            var tipoTarjeta1 = new TipoTarjeta { Nombre = "Gold" };
            var tipoTarjeta2 = new TipoTarjeta { Nombre = "Platinum" };

            await _context.AddAsync(procesadorPago1);
            await _context.AddAsync(procesadorPago2);
            await _context.AddAsync(tipoTarjeta1);
            await _context.AddAsync(tipoTarjeta2);
            await _context.SaveChangesAsync();

            var procesadorTarjeta1 = new ProcesadorTarjeta { ProcesadorPagoId = procesadorPago1.Id, TipoTarjetaId = tipoTarjeta1.Id };
            var procesadorTarjeta2 = new ProcesadorTarjeta { ProcesadorPagoId = procesadorPago2.Id, TipoTarjetaId = tipoTarjeta2.Id };

            await _context.ProcesadorTarjeta.AddRangeAsync(procesadorTarjeta1, procesadorTarjeta2);
            await _context.SaveChangesAsync();

            // Act: Obtener el primer procesador de tarjeta que cumpla con la condición
            var primerProcesadorTarjeta = await _procesadorTarjetaRepositorio.ObtenerPrimero(t => t.ProcesadorPagoId == procesadorPago2.Id);

            // Assert: Verificar que se ha obtenido el procesador de tarjeta correcto
            Assert.IsNotNull(primerProcesadorTarjeta);
            Assert.AreEqual(procesadorPago2.Id, primerProcesadorTarjeta.ProcesadorPagoId);
        }

        [Test]
        public async Task RemoverRango_DebeEliminarVariosProcesadoresTarjeta()
        {
            // Arrange: Agregar varios procesadores de tarjeta al contexto
            var procesadorPago = new ProcesadorPago { Nombre = "Visa", MetodoVerificacion = "Metodo10", PasarelaPago = "Pasarela10" };
            var tipoTarjeta1 = new TipoTarjeta { Nombre = "Gold" };
            var tipoTarjeta2 = new TipoTarjeta { Nombre = "Platinum" };

            await _context.AddAsync(procesadorPago);
            await _context.AddAsync(tipoTarjeta1);
            await _context.AddAsync(tipoTarjeta2);
            await _context.SaveChangesAsync();

            var procesadorTarjeta1 = new ProcesadorTarjeta { ProcesadorPagoId = procesadorPago.Id, TipoTarjetaId = tipoTarjeta1.Id };
            var procesadorTarjeta2 = new ProcesadorTarjeta { ProcesadorPagoId = procesadorPago.Id, TipoTarjetaId = tipoTarjeta2.Id };

            await _context.ProcesadorTarjeta.AddRangeAsync(procesadorTarjeta1, procesadorTarjeta2);
            await _context.SaveChangesAsync();

            // Act: Eliminar varios procesadores de tarjeta
            _procesadorTarjetaRepositorio.RemoverRango(new List<ProcesadorTarjeta> { procesadorTarjeta1, procesadorTarjeta2 });
            await _context.SaveChangesAsync();

            // Assert: Verificar que se han eliminado los procesadores de tarjeta correctamente
            var procesadorTarjetaEliminado1 = await _context.ProcesadorTarjeta.FindAsync(procesadorTarjeta1.Id);
            var procesadorTarjetaEliminado2 = await _context.ProcesadorTarjeta.FindAsync(procesadorTarjeta2.Id);
            Assert.IsNull(procesadorTarjetaEliminado1);
            Assert.IsNull(procesadorTarjetaEliminado2);
        }
    }
}
