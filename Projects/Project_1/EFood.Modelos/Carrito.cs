using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos
{
    public class Carrito
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "ProcesadorPagoID es Requerido")]
        public int ProcesadorPagoID { get; set; }

        [ForeignKey("ProcesadorPagoID")]
        public ProcesadorPago ProcesadorPago { get; set; }

        [Required(ErrorMessage = "NombreCliente es Requerido")]
        [MaxLength(50, ErrorMessage = "NombreCliente debe ser Maximo 50 Caracteres")]
        public string NombreCliente { get; set; }

        [Required(ErrorMessage = "ApellidoCliente es Requerido")]
        [MaxLength(50, ErrorMessage = "ApellidoCliente debe ser Maximo 50 Caracteres")]
        public string ApellidosCliente { get; set; }

        [Required(ErrorMessage = "TelefonoCliente es Requerido")]
        [MaxLength(50, ErrorMessage = "TelefonoCliente debe ser Maximo 50 Caracteres")]
        public string TelefonoCliente { get; set; }

        [Required(ErrorMessage = "Direccion es Requerido")]
        [MaxLength(500, ErrorMessage = "Direccion debe ser Maximo 50 Caracteres")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Estado es Requerido")]
        [MaxLength(15, ErrorMessage = "Direccion debe ser Maximo 15 Caracteres")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "CodigoCupon es Requerido")]
        [MaxLength(50, ErrorMessage = "CodigoCupon debe ser Maximo 50 Caracteres")]
        public string CodigoCupon { get; set; }

        [Required(ErrorMessage = "El MontoTotal es requerido")]
        [Range(0, double.MaxValue, ErrorMessage = "EL MontoTotal debe ser positiva o cero")]
        public double MontoTotal { get; set; }

        [Required(ErrorMessage = "La fecha del error es requerida")]
        public DateTime FechaHora { get; set; }
    }
}
