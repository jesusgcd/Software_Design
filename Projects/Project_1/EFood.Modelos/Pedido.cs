using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "PrecioProductoID es Requerido")]
        public int PrecioProductoID { get; set; }

        [ForeignKey("PrecioProductoID")]
        public Precio PrecioProducto { get; set; }

        [Required(ErrorMessage = "Carrito es Requerido")]
        public int CarritoID { get; set; }

        [ForeignKey("CarritoID")]
        public Carrito Carrito { get; set; }

        [Required(ErrorMessage = "La Cantidad es requerido")]
        [Range(0, int.MaxValue, ErrorMessage = "La Cantidad debe ser positiva o cero")]
        public int Cantidad { get; set; }
    }
}
