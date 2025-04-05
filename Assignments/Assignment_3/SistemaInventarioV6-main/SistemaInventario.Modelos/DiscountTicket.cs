using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class DiscountTicket
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Código es requerido")]
        [MaxLength(15, ErrorMessage = "Código es de max. 15 caracteres")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(30, ErrorMessage = "Nombre es de max. 30 caracteres")]
        public string Name { get;set; }

        [Required(ErrorMessage ="Disponibles es requerido")]
        [Range(0, 10000, ErrorMessage = "El número de disponibles debe ser positivo")]
        public int Stock {  get; set; }

        [Required(ErrorMessage = "Descuento es requerido")]
        [Range(1, 100, ErrorMessage = "El porcentaje de descuento debe estar entre 1 y 100")]
        public int Percentage { get; set; }

    }
}
