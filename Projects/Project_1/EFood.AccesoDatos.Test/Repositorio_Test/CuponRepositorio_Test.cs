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
    public class CuponRepositorio_Test
    {
        private ApplicationDbContext _dbContext;
        private CuponRepositorio _cuponRepositorio;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _cuponRepositorio = new CuponRepositorio(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task Agregar_DebeAgregarCupon()
        {
            var cupon = new Cupon
            {
                Codigo = "DESC20",
                Descripcion = "Descuento del 20%",
                CantidadDisponible = 10,
                Descuento = 20
            };

            await _cuponRepositorio.Agregar(cupon);
            await _dbContext.SaveChangesAsync();

            var cupones = await _cuponRepositorio.ObtenerTodos();

            Assert.AreEqual(1, cupones.Count());
            Assert.AreEqual("DESC20", cupones.First().Codigo);
        }

        [Test]
        public async Task Obtener_DebeDevolverCuponPorId()
        {
            var cupon = new Cupon
            {
                Codigo = "DESC20",
                Descripcion = "Descuento del 20%",
                CantidadDisponible = 10,
                Descuento = 20
            };

            await _cuponRepositorio.Agregar(cupon);
            await _dbContext.SaveChangesAsync();

            var cuponObtenido = await _cuponRepositorio.Obtener(cupon.Id);

            Assert.NotNull(cuponObtenido);
            Assert.AreEqual("DESC20", cuponObtenido.Codigo);
        }

        [Test]
        public async Task ObtenerTodos_DebeDevolverTodosLosCupones()
        {
            var cupon1 = new Cupon
            {
                Codigo = "DESC20",
                Descripcion = "Descuento del 20%",
                CantidadDisponible = 10,
                Descuento = 20
            };

            var cupon2 = new Cupon
            {
                Codigo = "DESC30",
                Descripcion = "Descuento del 30%",
                CantidadDisponible = 5,
                Descuento = 30
            };

            await _cuponRepositorio.Agregar(cupon1);
            await _cuponRepositorio.Agregar(cupon2);
            await _dbContext.SaveChangesAsync();

            var cupones = await _cuponRepositorio.ObtenerTodos();

            Assert.AreEqual(2, cupones.Count());
        }

        [Test]
        public async Task ObtenerPrimero_DebeDevolverElPrimerCuponQueCumplaElFiltro()
        {
            var cupon1 = new Cupon
            {
                Codigo = "DESC20",
                Descripcion = "Descuento del 20%",
                CantidadDisponible = 10,
                Descuento = 20
            };

            var cupon2 = new Cupon
            {
                Codigo = "DESC30",
                Descripcion = "Descuento del 30%",
                CantidadDisponible = 5,
                Descuento = 30
            };

            await _cuponRepositorio.Agregar(cupon1);
            await _cuponRepositorio.Agregar(cupon2);
            await _dbContext.SaveChangesAsync();

            var cuponObtenido = await _cuponRepositorio.ObtenerPrimero(c => c.Codigo == "DESC30");

            Assert.NotNull(cuponObtenido);
            Assert.AreEqual("DESC30", cuponObtenido.Codigo);
        }

        [Test]
        public async Task Remover_DebeEliminarCupon()
        {
            var cupon = new Cupon
            {
                Codigo = "DESC20",
                Descripcion = "Descuento del 20%",
                CantidadDisponible = 10,
                Descuento = 20
            };

            await _cuponRepositorio.Agregar(cupon);
            await _dbContext.SaveChangesAsync();

            _cuponRepositorio.Remover(cupon);
            await _dbContext.SaveChangesAsync();

            var cupones = await _cuponRepositorio.ObtenerTodos();

            Assert.AreEqual(0, cupones.Count());
        }

        [Test]
        public async Task RemoverRango_DebeEliminarMultiplesCupones()
        {
            var cupon1 = new Cupon
            {
                Codigo = "DESC20",
                Descripcion = "Descuento del 20%",
                CantidadDisponible = 10,
                Descuento = 20
            };

            var cupon2 = new Cupon
            {
                Codigo = "DESC30",
                Descripcion = "Descuento del 30%",
                CantidadDisponible = 5,
                Descuento = 30
            };

            await _cuponRepositorio.Agregar(cupon1);
            await _cuponRepositorio.Agregar(cupon2);
            await _dbContext.SaveChangesAsync();

            _cuponRepositorio.RemoverRango(new List<Cupon> { cupon1, cupon2 });
            await _dbContext.SaveChangesAsync();

            var cupones = await _cuponRepositorio.ObtenerTodos();

            Assert.AreEqual(0, cupones.Count());
        }

        [Test]
        public async Task Actualizar_DebeActualizarCupon()
        {
            var cupon = new Cupon
            {
                Codigo = "DESC20",
                Descripcion = "Descuento del 20%",
                CantidadDisponible = 10,
                Descuento = 20
            };

            await _cuponRepositorio.Agregar(cupon);
            await _dbContext.SaveChangesAsync();

            cupon.Codigo = "DESC30";
            cupon.Descripcion = "Descuento del 30%";
            cupon.CantidadDisponible = 5;
            cupon.Descuento = 30;

            _cuponRepositorio.Actualizar(cupon);
            await _dbContext.SaveChangesAsync();

            var cuponActualizado = await _cuponRepositorio.Obtener(cupon.Id);

            Assert.NotNull(cuponActualizado);
            Assert.AreEqual("DESC30", cuponActualizado.Codigo);
            Assert.AreEqual("Descuento del 30%", cuponActualizado.Descripcion);
            Assert.AreEqual(5, cuponActualizado.CantidadDisponible);
            Assert.AreEqual(30, cuponActualizado.Descuento);
        }
    }
}
