using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class Cupon
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El codigo del descuento es requerido")]
        [MaxLength(50, ErrorMessage = "El codigo debe ser de maximo 50 caracteres")]
        public string Codigo { get; set; }
        [Required(ErrorMessage = "La descripcion del descuento es requeridoa")]
        [MaxLength(500, ErrorMessage = "La descripcion del descuento debe ser de maximo 500 caracteres")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Cantidad disponible es requerida")]
        [Range(0,int.MaxValue,ErrorMessage = "La cantidad debe ser positiva o cero")]
        public int CantidadDisponible { get; set; }
        //Estos datos se validan antes de meterlos a la base de datos
        [Required(ErrorMessage = "El porcentaje de descuento es requerido")]
        [Range(1,100,ErrorMessage ="El descuento debe estar entre 1 y 100")]

        public int Descuento { get; set; }

    }
}
