using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class Card
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Descripción es requerida")]
        [MaxLength(35, ErrorMessage = "Descripción debe ser de max. 35 caracteres")]
        public string Description { get; set; }
    }
}
