using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SistemaInventarioV6.Modelos.ViewModels;

namespace SistemaInventarioV6.Modelos
{
    public class ShoppingCart
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        public int PriceId { get; set; }

        
        [ForeignKey("PriceId")]
        public Price Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [NotMapped]
        public double Cost { get; set; }
    }
}
