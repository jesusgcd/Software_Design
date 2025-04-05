using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class Precio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "ProductoID es Requerido")]
        public int ProductoID { get; set; }

        [ForeignKey("ProductoID")]
        public Producto Producto { get; set; }

        [Required(ErrorMessage = "TipoPrecioID es Requerido")]
        public int TipoPrecioID { get; set; }

        [ForeignKey("TipoPrecioID")]
        public TipoPrecio TipoPrecio { get; set; }

        [Required(ErrorMessage = "El Monto es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El Monto debe ser positiva o cero")]
        public double Monto { get; set; }
    }
}
