using SistemaInventarioV6.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio
{
    public interface IProductRepositorio : IRepositorio<Product>
    {
        void Actualizar(Product product);

        IEnumerable<SelectListItem> ObtenerTodosDropDownList(string obj);
    }
}
