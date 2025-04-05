using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos.ViewModels
{
    public class ProcesadorPagoVM
    {
        public ProcesadorPago ProcesadorPago { get; set; }
		public IEnumerable<SelectListItem> MetodoPagoLista { get; set; }
		public IEnumerable<SelectListItem> TipoTarjeta { get; set; }

        public String mensajeError { get; set; } = "";
    }
}
