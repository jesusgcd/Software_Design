using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos.ViewModels
{
    public class ProcessorCardVM
    {
        public ProcessorCard ProcessorCard { get; set; }

        public IEnumerable<SelectListItem> ProcessorList { get; set; }

        public IEnumerable<SelectListItem> CardList { get; set; }
    }
}
