using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class Error
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El codigo del error es requerido")]
        [MaxLength(50, ErrorMessage = "El codigo del error debe ser de maximo 50 caracteres")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "La descripcion del descuento es requeridoa")]
        [MaxLength(500, ErrorMessage = "La descripcion del descuento debe ser de maximo 500 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha del error es requerida")]
        public DateTime FechaHora { get; set; }

    }
}
