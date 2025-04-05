using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos.ViewModels
{
    public class ShoppingCartVM
    {
        public ShoppingCart ShoppingCart { get; set; }

        public Product Product { get; set; }

        public IEnumerable<SelectListItem> PriceList { get; set; }

        public IEnumerable<ShoppingCart> ShoppingCartList { get; set; }

        public Order Order { get; set; }

        public CardVM CardVM { get; set; }

        public CheckVM CheckVM { get; set; }

        public IEnumerable<SelectListItem> CardList { get; set; }

    }
}
