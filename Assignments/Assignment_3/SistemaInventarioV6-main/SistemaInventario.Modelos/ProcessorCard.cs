using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class ProcessorCard
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Procesador es requerido")]
        public int ProcessorId { get; set; }

        [ForeignKey("ProcessorId")]
        public PaymentProcessor PaymentProcessor { get; set; }

        [Required(ErrorMessage = "Tarjeta es requerida")]
        public int CardId { get; set; }

        [ForeignKey("CardId")]
        public Card Card { get; set; }
    }
}
