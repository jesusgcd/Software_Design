using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class TipoPrecio
    {

        [Key] 
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre del Tipo Precio es Requerido")]
        [MaxLength(50, ErrorMessage = "El nombre debe ser Maximo 50 Caracteres")]
        public string Nombre { get; set; }
    }
}
