using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        private readonly ApplicationDbContext _db;
        public IUsuarioRepositorio Usuario { get; private set; }

        public ICategoriaRepositorio Categoria { get; private set; }
        public IMarcaRepositorio Marca { get; private set; }
        public IProductoSIRepositorio ProductoSI { get; private set; }
        public IProductoRepositorio Producto { get; private set; }
        public IPrecioRepositorio Precio { get; private set; }
        public ITipoPrecioRepositorio TipoPrecio { get; private set; }
        public ICuponRepositorio Cupon { get; private set; }
        public IBitacoraRepositorio Bitacora { get; private set; }
        public IErrorRepositorio Error { get; private set; }
        public IMetodoPagoRepositorio MetodoPago { get; private set; }
        public IOrdenRepositorio Orden {  get; private set; }
        public IOrdenDetalleRepositorio OrdenDetalle { get; private set; }
        public ICarroCompraRepositorio CarroCompra { get; private set; }

        public IProcesadorTarjetaRepositorio ProcesadorTarjeta { get; private set; }
        public IProcesadorPagoRepositorio ProcesadorPago { get; private set; }
        public ITipoTarjetaRepositorio TipoTarjeta { get; private set; }
        public ILineaComidaRepositorio LineaComida { get; private set; }




        //No olvidarse agregar aqui tambien para nuevo crud
        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Usuario = new UsuarioRepositorio(_db);

            Cupon = new CuponRepositorio(_db);
            Producto = new ProductoRepositorio(_db);
            Categoria = new CategoriaRepositorio(_db);
            Marca = new MarcaRepositorio(_db);
            ProductoSI = new ProductoSIRepositorio(_db);
            Error = new ErrorRepositorio(_db);
            ProcesadorTarjeta = new ProcesadorTarjetaRepositorio(_db);
            ProcesadorPago = new ProcesadorPagoRepositorio(_db);
            Precio = new PrecioRepositorio(_db);
            Producto = new ProductoRepositorio(_db);
            Bitacora = new BitacoraRepositorio(_db);
            TipoPrecio = new TipoPrecioRepositorio(_db);
            TipoTarjeta = new TipoTarjetaRepositorio(_db);
            LineaComida = new LineaComidaRepositorio(_db);
            MetodoPago = new MetodoPagoRepositorio(_db);
            Orden = new OrdenRepositorio(_db);
            OrdenDetalle = new OrdenDetalleRepositorio(_db);
            CarroCompra = new CarroCompraRepositorio(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task Guardar()
        {
            await _db.SaveChangesAsync();
        }
    }
}