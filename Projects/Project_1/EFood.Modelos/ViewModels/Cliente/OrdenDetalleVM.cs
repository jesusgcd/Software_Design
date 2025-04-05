using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos.ViewModels
{
    public class OrdenDetalleVM
    {
        public Orden Orden { get; set; }

        public IEnumerable<OrdenDetalle> OrdenDetalleLista { get; set; }
    }
}
