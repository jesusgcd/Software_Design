using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos.ViewModels
{
    public class MostrarPedidosVM
	{
        public int NumeroOrden { get; set; }
        public String NombreUsuario { get; set; }
        public List<Array> OrdenesDetalles { get; set; }
        public String mensajeError { get; set; }
    }
}
