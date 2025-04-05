using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos.ViewModels
{
    public class CheckVM
    {
        [Required(ErrorMessage = "Número de cheque es requerido")]
        public string CheckNumber { get; set; }

        [Required(ErrorMessage = "Número de cuenta es requerido")]
        public string Account { get; set; }
    }
}
