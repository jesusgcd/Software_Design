using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {
        ICategoriaRepositorio Categoria { get; }
        IMarcaRepositorio Marca { get; }
        IProductoSIRepositorio ProductoSI { get; }
        IProductoRepositorio Producto { get; }
        IPrecioRepositorio Precio { get; }
        ITipoPrecioRepositorio TipoPrecio { get; }
        ICuponRepositorio Cupon { get; }
        IErrorRepositorio Error { get; }
        IUsuarioRepositorio Usuario { get; }
        IBitacoraRepositorio Bitacora { get; }
        //Con relaciones raras
        IMetodoPagoRepositorio MetodoPago { get; }
        IOrdenRepositorio Orden { get; }
        IOrdenDetalleRepositorio OrdenDetalle { get; }

        ICarroCompraRepositorio CarroCompra {  get; }
        IProcesadorTarjetaRepositorio ProcesadorTarjeta { get; }
        IProcesadorPagoRepositorio ProcesadorPago { get; }
        ITipoTarjetaRepositorio TipoTarjeta { get; }
        ILineaComidaRepositorio LineaComida { get; }


        Task Guardar();
    }
}