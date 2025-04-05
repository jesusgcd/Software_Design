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
using EFood.Utilidades;

namespace EFood.AccesoDatos.Test.Repositorio_Test
{
    [TestFixture]
    public class OrdenRepositorio_Test
    {
        private ApplicationDbContext _dbContext;
        private IOrdenRepositorio _ordenRepositorio;

        [SetUp]
        public void Setup()
        {
            // Configurar la base de datos en memoria
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);

            // Inicializar el repositorio
            _ordenRepositorio = new OrdenRepositorio(_dbContext);
        }

        [Test]
        public async Task Agregar_DebeAgregarOrden()
        {
            var orden = new Orden
            {
                ProcesadorPagoId = 1,
                NombreCliente = "Juan",
                ApellidosCliente = "Perez",
                TelefonoCliente = "123456789",
                Direccion = "Calle Principal",
                Estado = "Pendiente",
                MontoTotal = 100.0,
                FechaHora = DateTime.Now
            };

            await _ordenRepositorio.Agregar(orden);
            await _dbContext.SaveChangesAsync();

            var ordenes = await _ordenRepositorio.ObtenerTodos();

            Assert.AreEqual(1, ordenes.Count());
            Assert.AreEqual("Juan", ordenes.First().NombreCliente);
        }

        [Test]
        public async Task Obtener_DebeDevolverOrdenPorId()
        {
            var orden = new Orden
            {
                ProcesadorPagoId = 1,
                NombreCliente = "Maria",
                ApellidosCliente = "Lopez",
                TelefonoCliente = "987654321",
                Direccion = "Calle Secundaria",
                Estado = "Completado",
                MontoTotal = 200.0,
                FechaHora = DateTime.Now
            };

            await _ordenRepositorio.Agregar(orden);
            await _dbContext.SaveChangesAsync();

            var ordenObtenida = await _ordenRepositorio.Obtener(orden.Id);

            Assert.NotNull(ordenObtenida);
            Assert.AreEqual("Maria", ordenObtenida.NombreCliente);
        }

        [Test]
        public async Task ObtenerTodos_DebeDevolverTodasLasOrdenes()
        {
            var orden1 = new Orden
            {
                ProcesadorPagoId = 1,
                NombreCliente = "Pedro",
                ApellidosCliente = "Gomez",
                TelefonoCliente = "555123456",
                Direccion = "Calle Mayor",
                Estado = "Pendiente",
                MontoTotal = 150.0,
                FechaHora = DateTime.Now
            };

            var orden2 = new Orden
            {
                ProcesadorPagoId = 2,
                NombreCliente = "Ana",
                ApellidosCliente = "Martinez",
                TelefonoCliente = "777987654",
                Direccion = "Avenida Principal",
                Estado = "Completado",
                MontoTotal = 300.0,
                FechaHora = DateTime.Now
            };

            await _ordenRepositorio.Agregar(orden1);
            await _ordenRepositorio.Agregar(orden2);
            await _dbContext.SaveChangesAsync();

            var ordenes = await _ordenRepositorio.ObtenerTodos();

            Assert.AreEqual(2, ordenes.Count());
        }

        [Test]
        public async Task ObtenerPrimero_DebeDevolverLaPrimeraOrdenQueCumplaElFiltro()
        {
            var orden1 = new Orden
            {
                ProcesadorPagoId = 1,
                NombreCliente = "Carlos",
                ApellidosCliente = "Sanchez",
                TelefonoCliente = "111222333",
                Direccion = "Avenida Secundaria",
                Estado = "Pendiente",
                MontoTotal = 120.0,
                FechaHora = DateTime.Now
            };

            var orden2 = new Orden
            {
                ProcesadorPagoId = 2,
                NombreCliente = "Laura",
                ApellidosCliente = "Ramirez",
                TelefonoCliente = "444555666",
                Direccion = "Avenida Terciaria",
                Estado = "Pendiente",
                MontoTotal = 180.0,
                FechaHora = DateTime.Now
            };

            await _ordenRepositorio.Agregar(orden1);
            await _ordenRepositorio.Agregar(orden2);
            await _dbContext.SaveChangesAsync();

            var ordenObtenida = await _ordenRepositorio.ObtenerPrimero(o => o.Estado == "Pendiente");

            Assert.NotNull(ordenObtenida);
            Assert.AreEqual("Carlos", ordenObtenida.NombreCliente);
        }

        [Test]
        public async Task Remover_DebeEliminarOrden()
        {
            var orden = new Orden
            {
                ProcesadorPagoId = 1,
                NombreCliente = "Roberto",
                ApellidosCliente = "Fernandez",
                TelefonoCliente = "999888777",
                Direccion = "Calle del Centro",
                Estado = "Cancelado",
                MontoTotal = 80.0,
                FechaHora = DateTime.Now
            };

            await _ordenRepositorio.Agregar(orden);
            await _dbContext.SaveChangesAsync();

            _ordenRepositorio.Remover(orden);
            await _dbContext.SaveChangesAsync();

            var ordenes = await _ordenRepositorio.ObtenerTodos();

            Assert.AreEqual(0, ordenes.Count());
        }

        [Test]
        public async Task RemoverRango_DebeEliminarVariasOrdenes()
        {
            var orden1 = new Orden
            {
                ProcesadorPagoId = 1,
                NombreCliente = "Elena",
                ApellidosCliente = "Diaz",
                TelefonoCliente = "333444555",
                Direccion = "Avenida Cuarta",
                Estado = "Pendiente",
                MontoTotal = 250.0,
                FechaHora = DateTime.Now
            };

            var orden2 = new Orden
            {
                ProcesadorPagoId = 2,
                NombreCliente = "Oscar",
                ApellidosCliente = "Jimenez",
                TelefonoCliente = "666777888",
                Direccion = "Calle Quinta",
                Estado = "Pendiente",
                MontoTotal = 220.0,
                FechaHora = DateTime.Now
            };

            await _ordenRepositorio.Agregar(orden1);
            await _ordenRepositorio.Agregar(orden2);
            await _dbContext.SaveChangesAsync();

            _ordenRepositorio.RemoverRango(new List<Orden> { orden1, orden2 });
            await _dbContext.SaveChangesAsync();

            var ordenes = await _ordenRepositorio.ObtenerTodos();

            Assert.AreEqual(0, ordenes.Count());
        }

        [Test]
        public async Task Actualizar_DebeActualizarOrden()
        {
            var orden = new Orden
            {
                ProcesadorPagoId = 1,
                NombreCliente = "Fernando",
                ApellidosCliente = "Lopez",
                TelefonoCliente = "000111222",
                Direccion = "Calle Nueva",
                Estado = "Pendiente",
                MontoTotal = 180.0,
                FechaHora = DateTime.Now
            };

            await _ordenRepositorio.Agregar(orden);
            await _dbContext.SaveChangesAsync();

            orden.NombreCliente = "Javier";
            orden.ApellidosCliente = "Rojas";
            orden.TelefonoCliente = "999888777";
            orden.Direccion = "Avenida Principal";
            orden.Estado = "Completado";
            orden.MontoTotal = 200.0;

            _ordenRepositorio.Actualizar(orden);
            await _dbContext.SaveChangesAsync();

            var ordenActualizada = await _ordenRepositorio.Obtener(orden.Id);

            Assert.NotNull(ordenActualizada);
            Assert.AreEqual("Javier", ordenActualizada.NombreCliente);
            Assert.AreEqual("Rojas", ordenActualizada.ApellidosCliente);
            Assert.AreEqual("999888777", ordenActualizada.TelefonoCliente);
            Assert.AreEqual("Avenida Principal", ordenActualizada.Direccion);
            Assert.AreEqual("Completado", ordenActualizada.Estado);
            Assert.AreEqual(200.0, ordenActualizada.MontoTotal);
        }

        [Test]
        public async Task ObtenerTodosLista_DebeDevolverTodasLasOrdenes()
        {
            // Arrange
            var ordenes = new List<Orden>
            {
                new Orden { Id = 1, NombreCliente = "Cliente1", ApellidosCliente = "Apellido1", Direccion = "Direccion1", TelefonoCliente = "65783456" ,FechaHora = DateTime.Now, Estado = DS.EstadoProcesado },
                new Orden { Id = 2, NombreCliente = "Cliente2", ApellidosCliente = "Apellido2", Direccion = "Direccion2", TelefonoCliente = "23485621", FechaHora = DateTime.Now, Estado = DS.EstadoEnCurso },
                new Orden { Id = 3, NombreCliente = "Cliente3", ApellidosCliente = "Apellido3", Direccion = "Direccion3", TelefonoCliente = "21345321",  FechaHora = DateTime.Now, Estado = "Cancelada" }
            };
            _dbContext.Orden.AddRange(ordenes);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _ordenRepositorio.ObtenerTodosLista();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Cliente1", result[0].NombreCliente);
            Assert.AreEqual("Cliente2", result[1].NombreCliente);
            Assert.AreEqual("Cliente3", result[2].NombreCliente);
        }

        [Test]
        public async Task ObtenerTodosLista_FiltradoPorFechasYEstado_DebeDevolverOrdenesFiltradas()
        {
            // Arrange
            var fechaInicio = DateTime.Today.AddDays(-7);
            var fechaFin = DateTime.Today;
            var estadoSeleccionado = "Pendiente";

            var ordenes = new List<Orden>
    {
        new Orden { Id = 1, NombreCliente = "Cliente1", ApellidosCliente = "Apellido1", TelefonoCliente = "123456789", Direccion = "Dirección1", Estado = "Pendiente", MontoTotal = 100.0, FechaHora = fechaInicio.AddDays(1) },
        new Orden { Id = 2, NombreCliente = "Cliente2", ApellidosCliente = "Apellido2", TelefonoCliente = "987654321", Direccion = "Dirección2", Estado = "Completada", MontoTotal = 150.0, FechaHora = fechaInicio.AddDays(2) },
        new Orden { Id = 3, NombreCliente = "Cliente3", ApellidosCliente = "Apellido3", TelefonoCliente = "456123789", Direccion = "Dirección3", Estado = "Pendiente", MontoTotal = 120.0, FechaHora = fechaInicio.AddDays(3) },
        new Orden { Id = 4, NombreCliente = "Cliente4", ApellidosCliente = "Apellido4", TelefonoCliente = "789456123", Direccion = "Dirección4", Estado = "Cancelada", MontoTotal = 80.0, FechaHora = fechaInicio.AddDays(4) },
        new Orden { Id = 5, NombreCliente = "Cliente5", ApellidosCliente = "Apellido5", TelefonoCliente = "321654987", Direccion = "Dirección5", Estado = "Pendiente", MontoTotal = 90.0, FechaHora = fechaInicio.AddDays(5) }
    };
            _dbContext.Orden.AddRange(ordenes);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _ordenRepositorio.ObtenerTodosLista(fechaInicio, fechaFin, estadoSeleccionado);

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.All(o => o.Estado == "Pendiente"));
            Assert.IsTrue(result.All(o => o.FechaHora >= fechaInicio && o.FechaHora <= fechaFin));
        }


        [TearDown]
        public void Teardown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}