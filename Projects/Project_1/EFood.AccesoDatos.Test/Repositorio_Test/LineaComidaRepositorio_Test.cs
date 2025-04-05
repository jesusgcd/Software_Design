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
    public class LineaComidaRepositorio_Test
    {
        private ApplicationDbContext _dbContext;
        private LineaComidaRepositorio _lineaComidaRepositorio;

        [SetUp]
        public void Setup()
        {
            // Configurar la base de datos en memoria
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);

            // Inicializar el repositorio
            _lineaComidaRepositorio = new LineaComidaRepositorio(_dbContext);
        }

        [Test]
        public async Task Agregar_DebeAgregarLineaComida()
        {
            // Arrange
            var lineaComida = new LineaComida { Id = 1, Nombre = "Comida 1" };

            // Act
            await _lineaComidaRepositorio.Agregar(lineaComida);
            await _dbContext.SaveChangesAsync();
            var result = await _lineaComidaRepositorio.Obtener(lineaComida.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Comida 1", result.Nombre);
        }

        [Test]
        public async Task Obtener_DebeDevolverLineaComidaPorId()
        {
            // Arrange
            var lineaComida = new LineaComida { Id = 2, Nombre = "Comida 2" };
            _dbContext.Set<LineaComida>().Add(lineaComida);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _lineaComidaRepositorio.Obtener(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Comida 2", result.Nombre);
        }

        [Test]
        public async Task ObtenerTodos_DebeDevolverTodasLasLineasDeComida()
        {
            // Arrange
            var lineaComidas = new List<LineaComida>
            {
                new LineaComida { Id = 3, Nombre = "Comida 3" },
                new LineaComida { Id = 4, Nombre = "Comida 4" }
            };
            _dbContext.Set<LineaComida>().AddRange(lineaComidas);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _lineaComidaRepositorio.ObtenerTodos();

            // Assert
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task ObtenerPrimero_DebeDevolverPrimeraLineaComidaQueCumpleElFiltro()
        {
            // Arrange
            var lineaComidas = new List<LineaComida>
            {
                new LineaComida { Id = 5, Nombre = "Comida 5" },
                new LineaComida { Id = 6, Nombre = "Comida 6" }
            };
            _dbContext.Set<LineaComida>().AddRange(lineaComidas);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _lineaComidaRepositorio.ObtenerPrimero(e => e.Nombre.Contains("6"));

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Comida 6", result.Nombre);
        }

        [Test]
        public void ObtenerTodosDropdownLista_DebeDevolverListaDesplegable()
        {
            // Arrange
            var lineaComidas = new List<LineaComida>
            {
                new LineaComida { Id = 7, Nombre = "Comida 7" },
                new LineaComida { Id = 8, Nombre = "Comida 8" }
            };
            _dbContext.Set<LineaComida>().AddRange(lineaComidas);
            _dbContext.SaveChanges();

            // Act
            var result = _lineaComidaRepositorio.ObtenerTodosDropdownLista("LineaComida").ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Comida 7", result[0].Text);
            Assert.AreEqual("7", result[0].Value);
            Assert.AreEqual("Comida 8", result[1].Text);
            Assert.AreEqual("8", result[1].Value);
        }

        [Test]
        public void Actualizar_DebeActualizarLineaComida()
        {
            // Arrange
            var lineaComida = new LineaComida { Id = 9, Nombre = "Comida 9" };
            _dbContext.Set<LineaComida>().Add(lineaComida);
            _dbContext.SaveChanges();

            // Act
            lineaComida.Nombre = "Comida 9 Actualizada";
            _lineaComidaRepositorio.Actualizar(lineaComida);
            var result = _lineaComidaRepositorio.Obtener(lineaComida.Id).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Comida 9 Actualizada", result.Nombre);
        }

        [Test]
        public async Task Remover_DebeEliminarLineaComida()
        {
            // Arrange
            var lineaComida = new LineaComida { Id = 10, Nombre = "Comida 10" };
            _dbContext.Set<LineaComida>().Add(lineaComida);
            await _dbContext.SaveChangesAsync();

            // Act
            _lineaComidaRepositorio.Remover(lineaComida);
            await _dbContext.SaveChangesAsync();
            var result = await _lineaComidaRepositorio.Obtener(10);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task RemoverRango_DebeEliminarVariasLineasComida()
        {
            // Arrange
            var lineaComidas = new List<LineaComida>
            {
                new LineaComida { Id = 11, Nombre = "Comida 11" },
                new LineaComida { Id = 12, Nombre = "Comida 12" }
            };
            _dbContext.Set<LineaComida>().AddRange(lineaComidas);
            await _dbContext.SaveChangesAsync();

            // Act
            _lineaComidaRepositorio.RemoverRango(lineaComidas);
            await _dbContext.SaveChangesAsync();
            var result1 = await _lineaComidaRepositorio.Obtener(11);
            var result2 = await _lineaComidaRepositorio.Obtener(12);

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