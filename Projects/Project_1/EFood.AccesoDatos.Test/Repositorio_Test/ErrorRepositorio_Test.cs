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
    public class ErrorRepositorio_Test
    {
        private ApplicationDbContext _dbContext;
        private ErrorRepositorio _errorRepositorio;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _errorRepositorio = new ErrorRepositorio(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task Agregar_DebeAgregarError()
        {
            var error = new Error
            {
                Codigo = "ERR001",
                Descripcion = "Error de prueba",
                FechaHora = DateTime.Now
            };

            await _errorRepositorio.Agregar(error);
            await _dbContext.SaveChangesAsync();

            var errores = await _errorRepositorio.ObtenerTodos();

            Assert.AreEqual(1, errores.Count());
            Assert.AreEqual("ERR001", errores.First().Codigo);
        }

        [Test]
        public async Task Obtener_DebeDevolverErrorPorId()
        {
            var error = new Error
            {
                Codigo = "ERR001",
                Descripcion = "Error de prueba",
                FechaHora = DateTime.Now
            };

            await _errorRepositorio.Agregar(error);
            await _dbContext.SaveChangesAsync();

            var errorObtenido = await _errorRepositorio.Obtener(error.Id);

            Assert.NotNull(errorObtenido);
            Assert.AreEqual("ERR001", errorObtenido.Codigo);
        }

        [Test]
        public async Task ObtenerTodos_DebeDevolverTodosLosErrores()
        {
            var error1 = new Error
            {
                Codigo = "ERR001",
                Descripcion = "Error 1",
                FechaHora = DateTime.Now
            };

            var error2 = new Error
            {
                Codigo = "ERR002",
                Descripcion = "Error 2",
                FechaHora = DateTime.Now
            };

            await _errorRepositorio.Agregar(error1);
            await _errorRepositorio.Agregar(error2);
            await _dbContext.SaveChangesAsync();

            var errores = await _errorRepositorio.ObtenerTodos();

            Assert.AreEqual(2, errores.Count());
        }

        [Test]
        public async Task ObtenerPrimero_DebeDevolverElPrimerErrorQueCumplaElFiltro()
        {
            var error1 = new Error
            {
                Codigo = "ERR001",
                Descripcion = "Error 1",
                FechaHora = DateTime.Now
            };

            var error2 = new Error
            {
                Codigo = "ERR002",
                Descripcion = "Error 2",
                FechaHora = DateTime.Now
            };

            await _errorRepositorio.Agregar(error1);
            await _errorRepositorio.Agregar(error2);
            await _dbContext.SaveChangesAsync();

            var errorObtenido = await _errorRepositorio.ObtenerPrimero(e => e.Codigo == "ERR002");

            Assert.NotNull(errorObtenido);
            Assert.AreEqual("ERR002", errorObtenido.Codigo);
        }

        [Test]
        public async Task Remover_DebeEliminarError()
        {
            var error = new Error
            {
                Codigo = "ERR001",
                Descripcion = "Error de prueba",
                FechaHora = DateTime.Now
            };

            await _errorRepositorio.Agregar(error);
            await _dbContext.SaveChangesAsync();

            _errorRepositorio.Remover(error);
            await _dbContext.SaveChangesAsync();

            var errores = await _errorRepositorio.ObtenerTodos();

            Assert.AreEqual(0, errores.Count());
        }

        [Test]
        public async Task RemoverRango_DebeEliminarMultiplesErrores()
        {
            var error1 = new Error
            {
                Codigo = "ERR001",
                Descripcion = "Error 1",
                FechaHora = DateTime.Now
            };

            var error2 = new Error
            {
                Codigo = "ERR002",
                Descripcion = "Error 2",
                FechaHora = DateTime.Now
            };

            await _errorRepositorio.Agregar(error1);
            await _errorRepositorio.Agregar(error2);
            await _dbContext.SaveChangesAsync();

            _errorRepositorio.RemoverRango(new List<Error> { error1, error2 });
            await _dbContext.SaveChangesAsync();

            var errores = await _errorRepositorio.ObtenerTodos();

            Assert.AreEqual(0, errores.Count());
        }

        [Test]
        public async Task ObtenerErrores_DebeDevolverTodosLosErrores()
        {
            var error1 = new Error
            {
                Codigo = "ERR001",
                Descripcion = "Error 1",
                FechaHora = DateTime.Now
            };

            var error2 = new Error
            {
                Codigo = "ERR002",
                Descripcion = "Error 2",
                FechaHora = DateTime.Now
            };

            await _errorRepositorio.Agregar(error1);
            await _errorRepositorio.Agregar(error2);
            await _dbContext.SaveChangesAsync();

            var errores = _errorRepositorio.ObtenerErrores();

            Assert.AreEqual(2, errores.Count());
        }
    }
}
