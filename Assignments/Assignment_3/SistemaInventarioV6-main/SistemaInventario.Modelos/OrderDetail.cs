using SistemaInventarioV6.Modelos.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class OrderDetail
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public int PriceId { get; set; }

        [ForeignKey("PriceId")]
        public Price Price { get; set; }

        public int Quantity { get; set; }

        public double Cost { get; set; }
    }
}
