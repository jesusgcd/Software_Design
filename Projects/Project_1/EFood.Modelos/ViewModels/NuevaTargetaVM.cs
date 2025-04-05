using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos.ViewModels
{
	public class NuevaTargetaVM
	{
		public int procesadorPagoId { get; set; }

		[Display(Name = "Tipo de Targeta")]
		[Required(ErrorMessage = "Debe seleccionar un tipo de targeta")]
		public int tipoTarjetaId { get; set; }

		public IEnumerable<SelectListItem> tiposTarjeta { get; set; }

		public String mensajeError { get; set; } = "";

	}
}
