using SistemaInventarioV6.AccesoDatos.Data;
using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.AccesoDatos.Repositorio
{
    public class ProductRepositorio : Repositorio<Product>, IProductRepositorio
    {

        private readonly ApplicationDbContext _db;

        public ProductRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Product product)
        {
            var productDB = _db.Products.FirstOrDefault(reg => reg.ID == product.ID);
            if(productDB != null)
            {
                if(product.ImageURL!= null)
                {
                    productDB.ImageURL = product.ImageURL;
                }
                productDB.Name = product.Name;
                productDB.Description = product.Description;
                productDB.FoodCategoryId = product.FoodCategoryId;
                _db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropDownList(string obj)
        {
            if (obj == "FoodCategory")
            {
                return _db.FoodCategories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.ID.ToString()
                }); ;
            }
            return null;
        }
    }
}