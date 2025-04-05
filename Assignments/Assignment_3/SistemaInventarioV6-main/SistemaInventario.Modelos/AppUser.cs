using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class AppUser : IdentityUser
    {
        [Required(ErrorMessage = "Username es requerido")]
        [MaxLength(20, ErrorMessage = "Nombre de usuario debe ser de max. 20 caracteres")]
        public string myUsername { get; set; }

        [MaxLength(100, ErrorMessage = "Pregunta debe ser de max. 100 caracteres")]
        public string security_question { get; set; }

        [MaxLength(40, ErrorMessage = "Respuesta debe ser de max. 40 caracteres")]
        public string security_answer { get; set; }

        [NotMapped]  // No se agrega a la tabla
        public string Role { get; set; }
    }
}
