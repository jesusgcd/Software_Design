using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class ProcesadorPago
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Nombre es Requerido")]
        [MaxLength(50, ErrorMessage ="Nombre debe ser de Maximo 50 Caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Es Necesario Seleccionar si Requiere Verificacion o No")]
        public bool RequiereVerificacion { get; set; }

        [MaxLength(100, ErrorMessage = "Metodo de Verificacion ser de Maximo 100 Caracteres")]
        public string MetodoVerificacion { get; set; }

        [Required(ErrorMessage = "Es Necesario Seleccionar el Estado")]
        public bool Estado { get; set; }

        [Required(ErrorMessage = "Pasarela de Pago es Requerido")]
		[MaxLength(300, ErrorMessage = "Pasarela de pago ser de Maximo 300 Caracteres")]
		public String PasarelaPago { get; set; }

        [Required(ErrorMessage = "Metodo de Pago es Requerido")]
        public int MetodoPagoId { get; set; }

        [ForeignKey("MetodoPagoId")]
        public MetodoPago MetodoPago { get; set; }
    }
}
