
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Linea de Comida es Requerido")]
        public int LineaComidaId { get; set; }

        [ForeignKey("LineaComidaId")]
        public LineaComida LineaComida { get; set; }

        [Required(ErrorMessage = "Nombre es Requerido")]
        [MaxLength(50)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Descripcion es Requerido")]
        [MaxLength(500)]
        public string Descripcion { get; set; }

        
        public string ImagenUrl { get; set; }



    }
}
