using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class Order
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }

        [Required]
        public int PaymentProcessorId { get; set; }

        [ForeignKey("PaymentProcessorId")]
        public PaymentProcessor PaymentProcessor { get; set; }

        public double Subtotal { get; set; }

        public double Discount { get; set; }

        public double Total { get; set; }

        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Estado es requerido")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Nombre es requerido")]
        [MaxLength(30, ErrorMessage = "Nombre debe ser de max. 30 caracteres")]
        public string ClientName { get; set; }

        [Required(ErrorMessage = "Apellido es requerido")]
        [MaxLength(50, ErrorMessage = "Apellido/s debe ser de max. 50 caracteres")]
        public string ClientSurname { get; set; }

        [Required(ErrorMessage = "Télefono es requerido")]
        [MaxLength(40, ErrorMessage = "Télefono debe ser de max. 40 caracteres")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Dirección es requerida")]
        [MaxLength(200, ErrorMessage = "Dirección debe ser de max. 200 caracteres")]
        public string Address { get; set;}

        [MaxLength(15, ErrorMessage = "Código debe ser de max. 15 caracteres")]
        public string DiscountCode { get; set; }

        [Required(ErrorMessage = "Método de pago es requerido")]
        [NotMapped]
        public string PaymentMethod { get; set; }

    }
}
