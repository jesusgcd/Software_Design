﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos.ViewModels
{
    public class ConsultarProductosVM
    {
        public int LineaComidaId { get; set; }
        public IEnumerable<SelectListItem> LineaComidaLista { get; set; }
        public List<Array> ListaProductos { get; set; } 
        public String MensajeError { get; set; }
    }
}
