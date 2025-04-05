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
    public class PriceRepositorio : Repositorio<Price>, IPriceRepositorio
    {

        private readonly ApplicationDbContext _db;

        public PriceRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Price price)
        {
            var priceDB = _db.Prices.FirstOrDefault(reg => reg.ID == price.ID);
            if(priceDB != null)
            {
                priceDB.Cost = price.Cost;
                priceDB.SizeId = price.SizeId;
                priceDB.ProductId = price.ProductId;
                _db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropDownList(string obj)
        {
            if (obj == "Size")
            {
                return _db.Sizes.Select(c => new SelectListItem
                {
                    Text = c.Description,
                    Value = c.ID.ToString()
                }); ;
            }
            if (obj == "Product")
            {
                return _db.Products.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.ID.ToString()
                }); ;
            }
            return null;
        }
    }
}