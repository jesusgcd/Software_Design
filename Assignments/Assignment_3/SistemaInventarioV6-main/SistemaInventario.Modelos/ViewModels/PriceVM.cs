using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos.ViewModels
{
    public class PriceVM
    {
        public Price Price { get; set; }

        public IEnumerable<SelectListItem> ProductList { get; set; }

        public IEnumerable<SelectListItem> SizeList { get; set; }
    }
}
