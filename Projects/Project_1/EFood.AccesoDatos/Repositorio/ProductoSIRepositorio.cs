using Microsoft.AspNetCore.Mvc.Rendering;
using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio
{
    public class ProductoSIRepositorio : Repositorio<ProductoSI>, IProductoSIRepositorio
    {

        private readonly ApplicationDbContext _db;

        public ProductoSIRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(ProductoSI ProductoSI)
        {
           var ProductoSIBD = _db.ProductoSIs.FirstOrDefault(b => b.Id == ProductoSI.Id);
            if(ProductoSIBD != null)
            {
                if(ProductoSI.ImagenUrl !=null)
                {
                    ProductoSIBD.ImagenUrl = ProductoSI.ImagenUrl;
                }
                ProductoSIBD.NumeroSerie= ProductoSI.NumeroSerie;
                ProductoSIBD.Descripcion = ProductoSI.Descripcion;
                ProductoSIBD.Precio = ProductoSI.Precio;
                ProductoSIBD.Costo = ProductoSI.Costo;
                ProductoSIBD.CategoriaId = ProductoSI.CategoriaId;
                ProductoSIBD.MarcaId = ProductoSI.MarcaId;
                ProductoSIBD.PadreId = ProductoSI.PadreId;
                ProductoSIBD.Estado = ProductoSI.Estado;


                _db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropdownLista(string obj)
        {
            if(obj == "Categoria")
            {
                return _db.Categorias.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            if (obj == "Marca")
            {
                return _db.Marcas.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Nombre,
                    Value = c.Id.ToString()
                });
            }
            if (obj == "ProductoSI")
            {
                return _db.ProductoSIs.Where(c => c.Estado == true).Select(c => new SelectListItem
                {
                    Text = c.Descripcion,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }
    }
}
