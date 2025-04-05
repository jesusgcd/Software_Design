using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos.ViewModels
{
    public class MostrarPreciosVM
    {
        public Producto Producto { get; set; }
        public List<Precio> Precios { get; set; }
    }
}
