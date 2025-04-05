using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class Bitacora
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "UsuarioId es Requerido")]
        public String UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        [Required(ErrorMessage = "FechaHora es Requerido")]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "Descripcion es Requerido")]
        [MaxLength(500, ErrorMessage = "Descripcion debe ser Maximo 500 Caracteres")]
        public string Descripcion { get; set; }
    }
}