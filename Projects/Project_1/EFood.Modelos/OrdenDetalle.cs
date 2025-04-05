using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class OrdenDetalle
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "OrdenId es Requerida")]
        public int OrdenId { get; set; }

        [ForeignKey("OrdenId")]
        public Orden Orden { get; set; }

        [Required(ErrorMessage = "ProductoId es Requerido")]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }

        [Required(ErrorMessage = "TipoPrecioId es Requerido")]
        public int TipoPrecioId { get; set; }

        [ForeignKey("TipoPrecioId")]
        public TipoPrecio TipoPrecio { get; set; }

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El Monto es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "El Monto debe ser positiva o cero")]
        public double Monto { get; set; }
    }
}
