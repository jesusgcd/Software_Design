using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.Modelos.ViewModels
{
    public class CarroCompraVM
    {
        public Producto Producto { get; set; }

        public CarroCompra CarroCompra { get; set; }
        public Precio Precio { get; set; }
        public TipoPrecio TipoPrecio { get; set; }
        public IEnumerable<Precio> PrecioLista { get; set; }

        public IEnumerable<CarroCompra> CarroCompraLista { get; set; }
        public Orden Orden { get; set; }
        public int Cantidad {  get; set; }

        public TipoTarjeta TipoTarjeta { get; set; }

        public IEnumerable<SelectListItem> TipoTarjetaDisponiblesLista { get; set; }

        public ProcesadorPago ProcesadorPagoEfectivo {  get; set; }
        public ProcesadorPago ProcesadorPagoTarjeta { get; set; }
        public ProcesadorPago ProcesadorPagoCheque { get; set; }
        public String metodoPagoElegido { get; set; }
    }
}
