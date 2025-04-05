using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Test.Repositorio_Test
{
    [TestFixture]
    public class TipoTarjetaRepositorio_Test
    {
        private ApplicationDbContext _context;
        private ITipoTarjetaRepositorio _tipoTarjetaRepositorio;

        [SetUp]
        public void Setup()
        {
            // Configurar el contexto de la base de datos en memoria para las pruebas
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated(); // Asegurarse de que la base de datos en memoria esté creada

            // Inicializar el repositorio con el contexto de la base de datos en memoria
            _tipoTarjetaRepositorio = new TipoTarjetaRepositorio(_context);
        }

        [TearDown]
        public void TearDown()
        {
            // Limpiar la base de datos en memoria después de cada prueba
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task ObtenerTodosDropdownLista_DebeRetornarListaDeSeleccionPorTipoTarjeta()
        {
            // Arrange
            var tipoTarjeta1 = new TipoTarjeta { Nombre = "Débito" };
            var tipoTarjeta2 = new TipoTarjeta { Nombre = "Crédito" };

            await _context.TipoTarjeta.AddRangeAsync(tipoTarjeta1, tipoTarjeta2);
            await _context.SaveChangesAsync();

            // Act
            var dropdownLista = _tipoTarjetaRepositorio.ObtenerTodosDropdownLista("TipoTarjeta");

            // Assert
            Assert.IsNotNull(dropdownLista);
            Assert.IsTrue(dropdownLista.Any()); // Verificar que la lista no esté vacía

            // Verificar que ambos tipos de tarjeta estén en la lista
            var tipoTarjetaDropdown1 = dropdownLista.FirstOrDefault(item => item.Text == tipoTarjeta1.Nombre);
            var tipoTarjetaDropdown2 = dropdownLista.FirstOrDefault(item => item.Text == tipoTarjeta2.Nombre);

            Assert.IsNotNull(tipoTarjetaDropdown1);
            Assert.IsNotNull(tipoTarjetaDropdown2);

            Assert.AreEqual(tipoTarjeta1.Id.ToString(), tipoTarjetaDropdown1.Value);
            Assert.AreEqual(tipoTarjeta2.Id.ToString(), tipoTarjetaDropdown2.Value);
        }

        [Test]
        public async Task ObtenerTodosDropdownListaAsync_DebeRetornarListaDeSeleccionPorTipoTarjetaConListaProcesadores()
        {
            // Arrange
            var tipoTarjeta1 = new TipoTarjeta { Nombre = "Débito" };
            var tipoTarjeta2 = new TipoTarjeta { Nombre = "Crédito" };

            await _context.TipoTarjeta.AddRangeAsync(tipoTarjeta1, tipoTarjeta2);
            await _context.SaveChangesAsync();

            var listaProcesadorTarjeta = new List<ProcesadorTarjeta>
            {
                new ProcesadorTarjeta { TipoTarjetaId = tipoTarjeta1.Id },
                new ProcesadorTarjeta { TipoTarjetaId = tipoTarjeta2.Id }
            };

            // Act
            var dropdownLista = await _tipoTarjetaRepositorio.ObtenerTodosDropdownListaAsync("TipoTarjeta", listaProcesadorTarjeta);

            // Assert
            Assert.IsNotNull(dropdownLista);
            Assert.IsTrue(dropdownLista.Any()); // Verificar que la lista no esté vacía

            // Verificar que ambos tipos de tarjeta estén en la lista
            var tipoTarjetaDropdown1 = dropdownLista.FirstOrDefault(item => item.Text == tipoTarjeta1.Nombre);
            var tipoTarjetaDropdown2 = dropdownLista.FirstOrDefault(item => item.Text == tipoTarjeta2.Nombre);

            Assert.IsNotNull(tipoTarjetaDropdown1);
            Assert.IsNotNull(tipoTarjetaDropdown2);

            Assert.AreEqual(tipoTarjeta1.Id.ToString(), tipoTarjetaDropdown1.Value);
            Assert.AreEqual(tipoTarjeta2.Id.ToString(), tipoTarjetaDropdown2.Value);
        }

        [Test]
        public async Task Agregar_DebeAgregarTipoTarjetaCorrectamente()
        {
            // Arrange
            var tipoTarjeta = new TipoTarjeta { Nombre = "Débito" };

            // Act
            await _tipoTarjetaRepositorio.Agregar(tipoTarjeta);
            await _context.SaveChangesAsync();

            var tipoTarjetaAgregada = await _context.TipoTarjeta.FirstOrDefaultAsync(t => t.Id == tipoTarjeta.Id);

            // Assert
            Assert.IsNotNull(tipoTarjetaAgregada);
            
        }


        [Test]
        public async Task Obtener_DebeObtenerTipoTarjetaCorrectamente()
        {
            // Arrange
            var tipoTarjeta = new TipoTarjeta { Nombre = "Crédito" };
            await _context.TipoTarjeta.AddAsync(tipoTarjeta);
            await _context.SaveChangesAsync();

            // Act
            var tipoTarjetaObtenida = await _tipoTarjetaRepositorio.Obtener(tipoTarjeta.Id);

            // Assert
            Assert.IsNotNull(tipoTarjetaObtenida);
            Assert.AreEqual(tipoTarjeta.Nombre, tipoTarjetaObtenida.Nombre);
        }

        [Test]
        public async Task ObtenerTodos_DebeObtenerTodosLosTiposDeTarjeta()
        {
            // Arrange
            var tiposTarjeta = new List<TipoTarjeta>
            {
                new TipoTarjeta { Nombre = "Débito" },
                new TipoTarjeta { Nombre = "Crédito" }
            };
            await _context.TipoTarjeta.AddRangeAsync(tiposTarjeta);
            await _context.SaveChangesAsync();

            // Act
            var tiposTarjetaObtenidos = await _tipoTarjetaRepositorio.ObtenerTodos();

            // Assert
            Assert.IsNotNull(tiposTarjetaObtenidos);
            Assert.AreEqual(tiposTarjeta.Count, tiposTarjetaObtenidos.Count());
            CollectionAssert.AreEqual(tiposTarjeta.Select(t => t.Nombre).ToList(),
                                      tiposTarjetaObtenidos.Select(t => t.Nombre).ToList());
        }

        [Test]
        public async Task Actualizar_DebeActualizarTipoTarjetaCorrectamente()
        {
            // Arrange
            var tipoTarjeta = new TipoTarjeta { Nombre = "Débito" };
            await _context.TipoTarjeta.AddAsync(tipoTarjeta);
            await _context.SaveChangesAsync();

            var tipoTarjetaModificada = new TipoTarjeta { Id = tipoTarjeta.Id, Nombre = "Crédito" };

            // Act
            _tipoTarjetaRepositorio.Actualizar(tipoTarjetaModificada);
            var tipoTarjetaActualizada = await _context.TipoTarjeta.FirstOrDefaultAsync(t => t.Id == tipoTarjeta.Id);

            // Assert
            Assert.IsNotNull(tipoTarjetaActualizada);
            Assert.AreEqual(tipoTarjetaModificada.Nombre, tipoTarjetaActualizada.Nombre);
        }

        [Test]
        public async Task Remover_DebeRemoverTipoTarjetaCorrectamente()
        {
            // Arrange
            var tipoTarjeta = new TipoTarjeta { Nombre = "Crédito" };
            await _context.TipoTarjeta.AddAsync(tipoTarjeta);
            await _context.SaveChangesAsync();

            // Act
            _tipoTarjetaRepositorio.Remover(tipoTarjeta);
            await _context.SaveChangesAsync();
            var tipoTarjetaEliminada = await _context.TipoTarjeta.FirstOrDefaultAsync(t => t.Id == tipoTarjeta.Id);

            // Assert
            Assert.IsNull(tipoTarjetaEliminada);
        }

        [Test]
        public async Task RemoverRango_DebeRemoverRangoDeTiposTarjetaCorrectamente()
        {
            // Arrange
            var tiposTarjeta = new List<TipoTarjeta>
            {
                new TipoTarjeta { Nombre = "Débito" },
                new TipoTarjeta { Nombre = "Crédito" }
            };
            await _context.TipoTarjeta.AddRangeAsync(tiposTarjeta);
            await _context.SaveChangesAsync();

            // Act
            _tipoTarjetaRepositorio.RemoverRango(tiposTarjeta);
            await _context.SaveChangesAsync();
            var tiposTarjetaRestantes = await _context.TipoTarjeta.ToListAsync();

            // Assert
            Assert.IsEmpty(tiposTarjetaRestantes);
        }

        [Test]
        public async Task ObtenerPrimero_DebeObtenerPrimerTipoTarjetaQueCumplaCondiciones()
        {
            // Arrange
            var tiposTarjeta = new List<TipoTarjeta>
            {
                new TipoTarjeta { Nombre = "Débito" },
                new TipoTarjeta { Nombre = "Crédito" }
            };
            await _context.TipoTarjeta.AddRangeAsync(tiposTarjeta);
            await _context.SaveChangesAsync();

            // Act
            var tipoTarjetaObtenida = await _tipoTarjetaRepositorio.ObtenerPrimero(t => t.Nombre == "Débito");

            // Assert
            Assert.IsNotNull(tipoTarjetaObtenida);
            Assert.AreEqual("Débito", tipoTarjetaObtenida.Nombre);
        }
    }
}
