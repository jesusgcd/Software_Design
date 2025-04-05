using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class FoodCategory
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(30, ErrorMessage = "Nombre debe ser de max. 30 caracteres")]
        public string Name { get; set; }
    }
}
