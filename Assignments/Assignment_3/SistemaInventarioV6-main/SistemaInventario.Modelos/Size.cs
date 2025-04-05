using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class Size
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Descripción es requerida")]
        [MaxLength(25, ErrorMessage = "Descripción debe ser de max. 25 caracteres")]
        public string Description { get; set; }
    }
}
