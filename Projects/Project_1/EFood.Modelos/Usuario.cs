using System;
using Microsoft.AspNetCore.Identity;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class Usuario : IdentityUser
    {
        /* IdentityUser ya le crea el name
        [Key]
        public int Id { get; set; }
        */
        [Required(ErrorMessage ="Nombre es Requerido")]
        [MaxLength(50, ErrorMessage ="Nombre debe ser de Maximo 50 Caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Apellidos son Requeridos")]
        [MaxLength(50, ErrorMessage = "Apellidos debe ser de Maximo 50 Caracteres")]
        public string Apellidos { get; set; }
        /* IdentityUser ya trae una columna llamada "PhoneNumber"

        [Required(ErrorMessage = "Telefono es Requerido")]
        [MaxLength(50, ErrorMessage = "Telefono debe ser de Maximo 50 Caracteres")]
        public string Telefono { get; set; }
        
        IdentityUser ya trae una columna llamada "Email"
        [Required(ErrorMessage = "Email es Requerido")]
        [MaxLength(50, ErrorMessage = "Email debe ser de Maximo 50 Caracteres")]
        public string Email { get; set; }
        
        La contraseña la maneja .net
        [Required(ErrorMessage = "Contraseña es Requerida")]
        [MaxLength(50, ErrorMessage = "Contraseña debe ser de Maximo 50 Caracteres")]
        public string Contraseña { get; set; }
        */
        [Required(ErrorMessage = "PreguntaSeguridad es Requerido")]
        [MaxLength(100, ErrorMessage = "PreguntaSeguridad debe ser de Maximo 100 Caracteres")]
        public string PreguntaSeguridad { get; set; }

        [Required(ErrorMessage = "RespuestaSeguridad es Requerido")]
        [MaxLength(100, ErrorMessage = "RespuestaSeguridad debe ser de Maximo 100 Caracteres")]
        public string RespuestaSeguridad { get; set; }

        [NotMapped]  // No se agrega a la tabla
        public string Role { get; set; }
        /* el rol lo maneja .net
        [Required(ErrorMessage = "Rol es Requerido")]
        public int RolId { get; set; }
        [ForeignKey("RolId")]
        public Rol Rol { get; set; }
        */
    }
}
