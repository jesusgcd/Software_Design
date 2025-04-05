using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class CarroCompra
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "PrecioID es Requerido")]
        public int PrecioId { get; set; }

        [ForeignKey("PrecioId")]
        public Precio Precio { get; set; }

        [Required(ErrorMessage = "ProductoId es Requerido")]
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public Producto Producto { get; set; }
        

        [Required(ErrorMessage = "La cantidad es requerida")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "La fecha del error es requerida")]
        public String SesionUsuario { get; set; }
    }
}
