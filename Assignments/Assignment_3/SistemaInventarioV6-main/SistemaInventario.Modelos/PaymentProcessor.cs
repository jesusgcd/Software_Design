using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class PaymentProcessor
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Código es requerido")]
        [MaxLength(15, ErrorMessage = "Código debe ser de max. 15 caracteres")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Procesador es requerido")]
        [MaxLength(30, ErrorMessage = "Procesador debe ser de max. 30 caracteres")]
        public string Processor { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(50, ErrorMessage = "Nombre debe ser de max. 50 caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Tipo es requerido")]
        [MaxLength(25)] 
        public string Type { get; set; }

        [Required(ErrorMessage = "Estado es requerido")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "Verificación es requerida")]
        public bool Verification { get; set; }

        [Required(ErrorMessage = "Método es requerido")]
        [MaxLength(50, ErrorMessage = "Método debe ser de max. 50 caracteres")]
        public string Method { get; set; }
    }
}
