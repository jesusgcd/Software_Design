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
    public class TipoPrecioRepositorio_Test
    {
        private ApplicationDbContext _context;
        private ITipoPrecioRepositorio _tipoPrecioRepositorio;

        [SetUp]
        public void Setup()
        {
            // Configuración del contexto en memoria para las pruebas
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);
            _tipoPrecioRepositorio = new TipoPrecioRepositorio(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        [Test]
        public async Task Agregar_DebeAgregarNuevoTipoPrecio()
        {
            // Arrange
            var nuevoTipoPrecio = new TipoPrecio
            {
                Nombre = "Precio Regular"
            };

            // Act
            await _tipoPrecioRepositorio.Agregar(nuevoTipoPrecio);
            await _context.SaveChangesAsync();

            // Assert
            var tipoPrecioAgregado = await _context.TipoPrecio.FindAsync(nuevoTipoPrecio.Id);
            Assert.IsNotNull(tipoPrecioAgregado);
            Assert.AreEqual(nuevoTipoPrecio.Nombre, tipoPrecioAgregado.Nombre);
        }

        [Test]
        public async Task Actualizar_DebeActualizarTipoPrecioExistente()
        {
            // Arrange
            var tipoPrecio = new TipoPrecio
            {
                Nombre = "Precio Regular"
            };
            await _context.AddAsync(tipoPrecio);
            await _context.SaveChangesAsync();

            // Act
            tipoPrecio.Nombre = "Nuevo Precio Regular";
            _tipoPrecioRepositorio.Actualizar(tipoPrecio);
            await _context.SaveChangesAsync();

            // Assert
            var tipoPrecioActualizado = await _context.TipoPrecio.FindAsync(tipoPrecio.Id);
            Assert.IsNotNull(tipoPrecioActualizado);
            Assert.AreEqual("Nuevo Precio Regular", tipoPrecioActualizado.Nombre);
        }

        [Test]
        public async Task Obtener_DebeRetornarTipoPrecioPorId()
        {
            // Arrange
            var tipoPrecio = new TipoPrecio
            {
                Nombre = "Precio Regular"
            };
            await _context.AddAsync(tipoPrecio);
            await _context.SaveChangesAsync();

            // Act
            var tipoPrecioObtenido = await _tipoPrecioRepositorio.Obtener(tipoPrecio.Id);

            // Assert
            Assert.IsNotNull(tipoPrecioObtenido);
            Assert.AreEqual(tipoPrecio.Id, tipoPrecioObtenido.Id);
        }

        [Test]
        public async Task ObtenerTodosDropdownLista_DebeRetornarListaDeSeleccionPorTiposPrecio()
        {
            // Arrange
            var tipoPrecio1 = new TipoPrecio
            {
                Nombre = "Precio Regular"
            };
            var tipoPrecio2 = new TipoPrecio
            {
                Nombre = "Precio Especial"
            };

            await _context.AddAsync(tipoPrecio1);
            await _context.AddAsync(tipoPrecio2);
            await _context.SaveChangesAsync();

            // Act
            var dropdownLista = _tipoPrecioRepositorio.ObtenerTodosDropdownLista("TiposPrecio");

            // Assert
            Assert.IsNotNull(dropdownLista);
            Assert.IsTrue(dropdownLista.Any()); // Verifica que haya al menos un elemento en la lista

            // Verifica que ambos tipos de precio estén en la lista
            var tipoPrecioDropdown1 = dropdownLista.FirstOrDefault(item => item.Text == tipoPrecio1.Nombre);
            var tipoPrecioDropdown2 = dropdownLista.FirstOrDefault(item => item.Text == tipoPrecio2.Nombre);

            Assert.IsNotNull(tipoPrecioDropdown1);
            Assert.IsNotNull(tipoPrecioDropdown2);

            Assert.AreEqual(tipoPrecio1.Id.ToString(), tipoPrecioDropdown1.Value);
            Assert.AreEqual(tipoPrecio2.Id.ToString(), tipoPrecioDropdown2.Value);
        }


        [Test]
        public async Task Remover_DebeEliminarTipoPrecioExistente()
        {
            // Arrange
            var tipoPrecio = new TipoPrecio
            {
                Nombre = "Precio Regular"
            };
            await _context.AddAsync(tipoPrecio);
            await _context.SaveChangesAsync();

            // Act
            _tipoPrecioRepositorio.Remover(tipoPrecio);
            await _context.SaveChangesAsync();

            // Assert
            var tipoPrecioEliminado = await _context.TipoPrecio.FindAsync(tipoPrecio.Id);
            Assert.IsNull(tipoPrecioEliminado);
        }


    }
}
