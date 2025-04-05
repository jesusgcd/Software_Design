using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos.ViewModels
{
    public class ErrorConsultaVM
    {

        public List<Array> ListaErrores { get; set; }
        public String MensajeError { get; set; }
    }
}
