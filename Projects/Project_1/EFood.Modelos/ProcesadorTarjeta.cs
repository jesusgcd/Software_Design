using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class ProcesadorTarjeta
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Procesador de Pago es Requerido")]
        public int ProcesadorPagoId { get; set; }
        [ForeignKey("ProcesadorPagoId")]
        public ProcesadorPago ProcesadorPago { get; set; }

        [Required(ErrorMessage = "Tipo de Tarjeta es Requerido")]
        public int TipoTarjetaId { get; set; }
        [ForeignKey("TipoTarjetaId")]
        public TipoTarjeta TipoTarjeta { get; set; }
    }
}
