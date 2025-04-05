using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos.ViewModels
{
    public class PedidoConsultaVM
    {

		[Required(ErrorMessage = "Debe seleccionar un estado")]
		public String EstadoSeleccionado { get; set; }
        public List<Array> ListaPedidos { get; set; }
        public String mensajeError { get; set; }
    }
}
