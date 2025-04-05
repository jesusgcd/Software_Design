using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos.ViewModels
{
    public class NuevoPrecioVM
    {
        public int ProductoId { get; set; }

        public int PrecioId { get; set; } = 0;

        [Display(Name = "Tipo de Precio")]
        [Required(ErrorMessage = "Debe seleccionar un tipo de precio")]
        public int TipoPrecioId { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "Debe ingresar un valor para el precio")]
        public double Valor { get; set; }

        public IEnumerable<SelectListItem> TiposPrecio { get; set; }

        public string mensajeError { get; set; }
    }
}
