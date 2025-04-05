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
using EFood.Modelos.ViewModels;

namespace EFood.AccesoDatos.Test.Repositorio_Test
{
    [TestFixture]
    public class ProductoRepositorio_Test
    {
        private ApplicationDbContext _context;
        private IProductoRepositorio _productoRepositorio;

        [SetUp]
        public void Setup()
        {
            // Configuración del contexto en memoria para las pruebas
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);
            _productoRepositorio = new ProductoRepositorio(_context);
        }

        [TearDown]
        public void Teardown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Agregar_DebeAgregarNuevoProducto()
        {
            // Arrange
            var lineaComida = new LineaComida { Nombre = "Comida Rápida" };
            await _context.AddAsync(lineaComida);
            await _context.SaveChangesAsync();

            var nuevoProducto = new Producto
            {
                LineaComidaId = lineaComida.Id,
                Nombre = "Hamburguesa",
                Descripcion = "Hamburguesa con queso",
                ImagenUrl = "hamburguesa.jpg"
            };

            // Act
            await _productoRepositorio.Agregar(nuevoProducto);
            await _context.SaveChangesAsync();

            // Assert
            var productoAgregado = await _context.Producto.FindAsync(nuevoProducto.Id);
            Assert.IsNotNull(productoAgregado);
            Assert.AreEqual(nuevoProducto.Nombre, productoAgregado.Nombre);
        }

        [Test]
        public async Task Actualizar_DebeActualizarProductoExistente()
        {
            // Arrange
            var lineaComida = new LineaComida { Nombre = "Comida Rápida" };
            await _context.AddAsync(lineaComida);
            await _context.SaveChangesAsync();

            var producto = new Producto
            {
                LineaComidaId = lineaComida.Id,
                Nombre = "Hamburguesa",
                Descripcion = "Hamburguesa con queso",
                ImagenUrl = "hamburguesa.jpg"
            };
            await _context.AddAsync(producto);
            await _context.SaveChangesAsync();

            // Act
            producto.Nombre = "Hamburguesa Doble";
            producto.Descripcion = "Hamburguesa con doble queso";
            _productoRepositorio.Actualizar(producto);
            await _context.SaveChangesAsync();

            // Assert
            var productoActualizado = await _context.Producto.FindAsync(producto.Id);
            Assert.IsNotNull(productoActualizado);
            Assert.AreEqual("Hamburguesa Doble", productoActualizado.Nombre);
        }

        [Test]
        public async Task Obtener_DebeRetornarProductoPorId()
        {
            // Arrange
            var lineaComida = new LineaComida { Nombre = "Comida Rápida" };
            await _context.AddAsync(lineaComida);
            await _context.SaveChangesAsync();

            var producto = new Producto
            {
                LineaComidaId = lineaComida.Id,
                Nombre = "Hamburguesa",
                Descripcion = "Hamburguesa con queso",
                ImagenUrl = "hamburguesa.jpg"
            };
            await _context.AddAsync(producto);
            await _context.SaveChangesAsync();

            // Act
            var productoObtenido = await _productoRepositorio.Obtener(producto.Id);

            // Assert
            Assert.IsNotNull(productoObtenido);
            Assert.AreEqual(producto.Id, productoObtenido.Id);
        }

        [Test]
        public async Task ObtenerTodos_DebeRetornarTodosLosProductos()
        {
            // Arrange
            var lineaComida1 = new LineaComida { Nombre = "Comida Rápida" };
            var lineaComida2 = new LineaComida { Nombre = "Postres" };
            await _context.AddAsync(lineaComida1);
            await _context.AddAsync(lineaComida2);
            await _context.SaveChangesAsync();

            var producto1 = new Producto
            {
                LineaComidaId = lineaComida1.Id,
                Nombre = "Hamburguesa",
                Descripcion = "Hamburguesa con queso",
                ImagenUrl = "hamburguesa.jpg"
            };
            var producto2 = new Producto
            {
                LineaComidaId = lineaComida2.Id,
                Nombre = "Helado",
                Descripcion = "Helado de vainilla",
                ImagenUrl = "helado.jpg"
            };
            await _context.AddAsync(producto1);
            await _context.AddAsync(producto2);
            await _context.SaveChangesAsync();

            // Act
            var productos = await _productoRepositorio.ObtenerTodos();

            // Assert
            Assert.IsNotNull(productos);
            Assert.AreEqual(2, productos.Count());
        }

        [Test]
        public async Task Remover_DebeEliminarProductoExistente()
        {
            // Arrange
            var lineaComida = new LineaComida { Nombre = "Comida Rápida" };
            await _context.AddAsync(lineaComida);
            await _context.SaveChangesAsync();

            var producto = new Producto
            {
                LineaComidaId = lineaComida.Id,
                Nombre = "Hamburguesa",
                Descripcion = "Hamburguesa con queso",
                ImagenUrl = "hamburguesa.jpg"
            };
            await _context.AddAsync(producto);
            await _context.SaveChangesAsync();

            // Act
            _productoRepositorio.Remover(producto);
            await _context.SaveChangesAsync();

            // Assert
            var productoEliminado = await _context.Producto.FindAsync(producto.Id);
            Assert.IsNull(productoEliminado);
        }

        [Test]
        public async Task ObtenerTodosDropdownLista_DebeRetornarListaDeSeleccionPorLineaComida()
        {
            // Arrange
            var lineaComida1 = new LineaComida { Nombre = "Comida Rápida" };
            var lineaComida2 = new LineaComida { Nombre = "Postres" };
            await _context.AddAsync(lineaComida1);
            await _context.AddAsync(lineaComida2);
            await _context.SaveChangesAsync();

            // Act
            var dropdownLista = _productoRepositorio.ObtenerTodosDropdownLista("LineaComida");

            // Assert
            Assert.IsNotNull(dropdownLista);
            Assert.AreEqual(2, dropdownLista.Count());
            Assert.IsTrue(dropdownLista.Any(l => l.Text == "Comida Rápida"));
            Assert.IsTrue(dropdownLista.Any(l => l.Text == "Postres"));
        }

        [Test]
        public async Task ObtenerProductosPorLineaComida_DebeRetornarProductosPorLineaComida()
        {
            // Arrange
            var lineaComida1 = new LineaComida { Nombre = "Comida Rápida" };
            var lineaComida2 = new LineaComida { Nombre = "Postres" };
            await _context.AddAsync(lineaComida1);
            await _context.AddAsync(lineaComida2);
            await _context.SaveChangesAsync();

            var producto1 = new Producto
            {
                LineaComidaId = lineaComida1.Id,
                Nombre = "Hamburguesa",
                Descripcion = "Hamburguesa con queso",
                ImagenUrl = "hamburguesa.jpg"
            };
            var producto2 = new Producto
            {
                LineaComidaId = lineaComida1.Id,
                Nombre = "Papas Fritas",
                Descripcion = "Papas fritas crujientes",
                ImagenUrl = "papas.jpg"
            };
            var producto3 = new Producto
            {
                LineaComidaId = lineaComida2.Id,
                Nombre = "Helado",
                Descripcion = "Helado de vainilla",
                ImagenUrl = "helado.jpg"
            };
            await _context.AddAsync(producto1);
            await _context.AddAsync(producto2);
            await _context.AddAsync(producto3);
            await _context.SaveChangesAsync();

            // Act
            var productosComidaRapida = _productoRepositorio.ObtenerProductosPorLineaComida(lineaComida1.Id);

            // Assert
            Assert.IsNotNull(productosComidaRapida);
        }

    }
}
