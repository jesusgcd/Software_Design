using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class Price
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Producto es requerido")]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required(ErrorMessage = "Tamaño es requerido")]
        public int SizeId { get; set; }

        [ForeignKey("SizeId")]
        public Size Size { get; set; }

        [Required(ErrorMessage = "Precio es requerido")]
        public double Cost { get; set; }
    }
}
