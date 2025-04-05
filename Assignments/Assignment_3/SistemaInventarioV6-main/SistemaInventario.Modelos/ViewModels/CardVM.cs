using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos.ViewModels
{
    public class CardVM
    {
        [Required(ErrorMessage = "Tarjeta es requerida")]
        public int CardId { get; set; }

        [Required(ErrorMessage = "Número de tarjeta es requerido")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "CVV es requerido")]
        public int CVV { get; set; }

        [Required(ErrorMessage = "Mes de vencimiento es requerido")]
        [Range(1, 12, ErrorMessage = "Ingrese un número del 1 al 12")]
        public int ExpMonth { get; set; }

        [Required(ErrorMessage = "Año de vencimiento es requerido")]
        [Range(2000, 3000, ErrorMessage = "Ingrese un año valido")]
        public int ExpYear { get; set; }
    }
}
