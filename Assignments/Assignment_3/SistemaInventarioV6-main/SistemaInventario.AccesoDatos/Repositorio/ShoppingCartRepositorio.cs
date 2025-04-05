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
    public class ShoppingCartRepositorio : Repositorio<ShoppingCart>, IShoppingCartRepositorio
    {

        private readonly ApplicationDbContext _db;

        public ShoppingCartRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(ShoppingCart shoppingCart)
        {
            _db.Update(shoppingCart);
        }

        public IEnumerable<SelectListItem> ObtenerTodosDropDownList(string obj, int productId)
        {
            if (obj == "Price" && productId != 0)
            {
                //Seleccione los precios relacionados al producto del parámetro
                return _db.Prices.Where(c=>c.ProductId == productId).Select(c => new SelectListItem
                {
                    Text = c.Size.Description + " - ₡" + c.Cost,
                    Value = c.ID.ToString()
                }); ;
            }
            if (obj == "Card")
            {
                return _db.Cards.Select(c => new SelectListItem
                {
                    Text = c.Description,
                    Value = c.ID.ToString()
                }); ;
            }
            return null;
        }
    }
}