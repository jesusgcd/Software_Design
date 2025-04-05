using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos.ViewModels
{
    public class ProductVM
    {
        public Product Product {  get; set; }

        public IEnumerable<SelectListItem> FoodCategoryList { get; set; }
    }
}
