using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Modelos.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {

        private readonly ApplicationDbContext _db;

        public ProductoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Producto producto)
        {
            var productoBD = _db.Producto.FirstOrDefault(b => b.Id == producto.Id);
            if (productoBD != null)
            {
                if (productoBD.ImagenUrl != null)
                {
                    productoBD.ImagenUrl = producto.ImagenUrl;
                }
                productoBD.Nombre = producto.Nombre;
                productoBD.Descripcion = producto.Descripcion;
                productoBD.LineaComidaId = producto.LineaComidaId;

                _db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
        {
            // Validar la entrada
            if (string.IsNullOrEmpty(obj))
            {
                throw new ArgumentNullException(nameof(obj), "El parámetro 'obj' no puede ser nulo o vacío.");
            }

            if (obj == "LineaComida")
            {
                return _db.LineaComida.Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }


        public IEnumerable<Producto> ObtenerProductosPorLineaComida(int lineaComidaId)
        {
            return _db.Producto.Where(p => p.LineaComidaId == lineaComidaId);
        }

    }
}