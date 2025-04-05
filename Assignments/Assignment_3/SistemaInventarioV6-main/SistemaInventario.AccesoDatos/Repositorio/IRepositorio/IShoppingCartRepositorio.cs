using SistemaInventarioV6.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio
{
    public interface IShoppingCartRepositorio : IRepositorio<ShoppingCart>
    {
        void Actualizar(ShoppingCart shoppingCart);

        IEnumerable<SelectListItem> ObtenerTodosDropDownList(string obj, int productId);
    }
}
