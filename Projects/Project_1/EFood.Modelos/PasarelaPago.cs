using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class PasarelaPago
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nombre es Requerido")]
        [MaxLength(50, ErrorMessage = "Nombre debe ser de Maximo 50 Caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Descripcion es Requerido")]
        [MaxLength(500, ErrorMessage = "Descripcion debe ser de Maximo 500 Caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "ApiKey es Requerido")]
        [MaxLength(100, ErrorMessage = "ApiKey debe ser de Maximo 100 Caracteres")]
        public string ApiKey { get; set; }

    }
}
