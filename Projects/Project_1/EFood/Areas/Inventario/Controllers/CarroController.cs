using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Modelos.ViewModels;
using EFood.Utilidades;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace EFood.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class CarroController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private string _webUrl;

        [BindProperty]
        public CarroCompraVM carroCompraVM { get; set; }

        public CarroController(IUnidadTrabajo unidadTrabajo, IConfiguration configuration)
        {
            _unidadTrabajo = unidadTrabajo;
            _webUrl = configuration.GetValue<string>("DomainUrls:WEB_URL");
        }
        private string ObtenerSesionUsuario()
        {
            if (HttpContext.Session.GetString("SessionId") == null)
            {
                HttpContext.Session.SetString("SessionId", System.Guid.NewGuid().ToString());
            }
            return HttpContext.Session.GetString("SessionId");
        }


        public async Task<IActionResult> Index()
        {
            var sessionID = ObtenerSesionUsuario();

            carroCompraVM = new CarroCompraVM();
            carroCompraVM.Orden = new Orden();
            carroCompraVM.CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(u => u.SesionUsuario == sessionID,
                                                                incluirPropiedades: "Producto");
            carroCompraVM.Orden.MontoTotal = 0;
            foreach (var carro in carroCompraVM.CarroCompraLista)
            {
                //lista.Precio = lista.Producto.Precio;  // Siempre mostrar el Precio actual del Producto
                var cantidad = carro.Cantidad;
                var precio = await _unidadTrabajo.Precio.ObtenerPrimero(p=>p.Id == carro.PrecioId);
                carroCompraVM.Orden.MontoTotal += (precio.Monto * cantidad);
            }
            return View(carroCompraVM);
        }
        
        public async Task<IActionResult> mas(int carroId)
        {
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c=>c.Id== carroId);
            carroCompras.Cantidad += 1;
            await _unidadTrabajo.Guardar();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> menos(int carroId)
        {
            var carroCompras = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);
           
            if(carroCompras.Cantidad ==1)
            {
                // Removemos el Registro del Carro de Compras y actualizamos la sesion
                var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                                                c => c.SesionUsuario == carroCompras.SesionUsuario);
                //var numeroProductos = carroLista.Count();
                _unidadTrabajo.CarroCompra.Remover(carroCompras);
                await _unidadTrabajo.Guardar();
                //HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos - 1);
            }
            else
            {
                carroCompras.Cantidad -= 1;
                await _unidadTrabajo.Guardar();
            }
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> remover(int carroId)
        {
            // Remueve el Registro del Carro de Compras y Actualiza la sesion
            var carroCompra = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.Id == carroId);
            var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                                               c => c.SesionUsuario == carroCompra.SesionUsuario);
            //var numeroProductos = carroLista.Count();
            _unidadTrabajo.CarroCompra.Remover(carroCompra);
            await _unidadTrabajo.Guardar();
            //HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos - 1);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AplicarCupon(CarroCompraVM carroCompraVM)
        {
            var CuponCodigo = carroCompraVM.Orden.CodigoCupon;
            //TempData[DS.Error] = "codigo de cupon a procesar es: " + CuponCodigo;

            int? porcentajeDescuento = null;
            // Buscar porcentaje de descuento según el código del cupón en la base de datos
            if (CuponCodigo.IsNullOrEmpty())
            {
                TempData[DS.Error] = "El codigo de cupon ingresado es invalido.";
                return RedirectToAction("Proceder", new { porcentajeDescuento, CuponCodigo});
            }
            var cupon = await _unidadTrabajo.Cupon.ObtenerPrimero(c => c.Codigo== CuponCodigo
                                        && c.CantidadDisponible > 0);
            // Simulamos un cupón válido con 10% de descuento
            if (cupon != null)
            {
                porcentajeDescuento = cupon.Descuento;
            }
            else
            {
                TempData[DS.Error] = "El codigo de cupon ingresado no existe.";

            }

            return RedirectToAction("Proceder", new { porcentajeDescuento, CuponCodigo});
        }
        public async Task<IActionResult> Proceder(int? porcentajeDescuento, string? CuponCodigo)
        {

            var sessionId = ObtenerSesionUsuario();

            carroCompraVM = new CarroCompraVM()
            {
                Orden = new Orden(),
                CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                             c => c.SesionUsuario == sessionId, incluirPropiedades: "Producto"),
            };
            
            carroCompraVM.Orden.MontoTotal = 0;

            foreach (var carro in carroCompraVM.CarroCompraLista)
            {
                var cantidad = carro.Cantidad;
                var precio = await _unidadTrabajo.Precio.ObtenerPrimero(p => p.Id == carro.PrecioId);
                carroCompraVM.Orden.MontoTotal += (precio.Monto * cantidad);
            }
            if (porcentajeDescuento.HasValue)
            {
                TempData[DS.Exitosa] = "Descuento aplicado";
                carroCompraVM.Orden.CodigoCupon = CuponCodigo;
                carroCompraVM.Orden.MontoTotal -= carroCompraVM.Orden.MontoTotal * porcentajeDescuento.Value / 100;
            }
            //Aqui obtener procesadores de pago

            
            var ProcesadorPagoEfectivo = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.MetodoPagoId == 1 && p.Estado == true);
            var ProcesadorPagoCheque = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.MetodoPagoId == 2 && p.Estado == true);
            var ProcesadorPagoTarjeta = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.MetodoPagoId == 3 && p.Estado == true);

            if (ProcesadorPagoEfectivo == null && ProcesadorPagoCheque == null && ProcesadorPagoTarjeta == null)
            {
                TempData[DS.Error] = "No hay procesadores de pago disponibles actualmente, intente mas tarde";
                return RedirectToAction("Index");
            }
            carroCompraVM.ProcesadorPagoEfectivo = ProcesadorPagoEfectivo;
            carroCompraVM.ProcesadorPagoCheque = ProcesadorPagoCheque;
            carroCompraVM.ProcesadorPagoTarjeta = ProcesadorPagoTarjeta;
            


            if (ProcesadorPagoTarjeta != null)
            {
                //Aqui obtener las tarjetas 
                var listaProcesadorTarjeta = await _unidadTrabajo.ProcesadorTarjeta.ObtenerTarjetasPorProcesadorPago(ProcesadorPagoTarjeta.Id);
                var listaTarjetasDisponibles = await _unidadTrabajo.TipoTarjeta.ObtenerTodosDropdownListaAsync("TipoTarjeta",listaProcesadorTarjeta);     //De momento obtener todos

                //LineaComidaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("LineaComida")
                carroCompraVM.TipoTarjetaDisponiblesLista = listaTarjetasDisponibles;
            }

            return View(carroCompraVM);
        }
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Proceder(CarroCompraVM carroCompraVM)
        {
            int procesadorPagoElegidoId;
            

            if (carroCompraVM.metodoPagoElegido == "Efectivo")
            {
                var ProcesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.MetodoPagoId == 1 && p.Estado == true);
                procesadorPagoElegidoId = ProcesadorPago.Id;
            }
            else if (carroCompraVM.metodoPagoElegido == "Cheque")
            {
                var ProcesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.MetodoPagoId == 2 && p.Estado == true);
                procesadorPagoElegidoId = ProcesadorPago.Id;
            }
            else
            {
                var ProcesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerPrimero(p => p.MetodoPagoId == 3 && p.Estado == true);
                procesadorPagoElegidoId = ProcesadorPago.Id;
            }
            
            var sessionID = ObtenerSesionUsuario();

            carroCompraVM.CarroCompraLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(
                                                 c=>c.SesionUsuario == sessionID,
                                                 incluirPropiedades:"Producto");
            carroCompraVM.Orden.FechaHora = DateTime.Now;
            carroCompraVM.Orden.Estado = DS.EstadoEnCurso;
            carroCompraVM.Orden.ProcesadorPagoId = procesadorPagoElegidoId;


            await _unidadTrabajo.Orden.Agregar(carroCompraVM.Orden);
            await _unidadTrabajo.Guardar();

            //quitar el descuento
            var cupon = await _unidadTrabajo.Cupon.ObtenerPrimero(c => c.Codigo == carroCompraVM.Orden.CodigoCupon);
            if (cupon != null)
            {
                cupon.CantidadDisponible -= 1;
                _unidadTrabajo.Cupon.Actualizar(cupon);
                await _unidadTrabajo.Guardar();
            }


            int ordenGuardadaId = (await _unidadTrabajo.Orden.ObtenerPrimero(o => o.FechaHora == carroCompraVM.Orden.FechaHora)).Id;

            foreach (var carro in carroCompraVM.CarroCompraLista)
            {
                OrdenDetalle ordenDetalle = new OrdenDetalle();
                ordenDetalle.OrdenId = ordenGuardadaId;
                ordenDetalle.ProductoId = carro.ProductoId;
                ordenDetalle.Cantidad = carro.Cantidad;
                //tipo precio
                var precioActual = await _unidadTrabajo.Precio.ObtenerPrimero(p => p.Id == carro.PrecioId);
                ordenDetalle.TipoPrecioId = precioActual.TipoPrecioID;
                ordenDetalle.Monto = carro.Cantidad * precioActual.Monto;
                await _unidadTrabajo.OrdenDetalle.Agregar(ordenDetalle);
                _unidadTrabajo.CarroCompra.Remover(carro);
                await _unidadTrabajo.Guardar();

            }
            //borrar todos



            // Grabar Detalle Orden
            /*
            foreach (var lista in carroCompraVM.CarroCompraLista)
            {
                OrdenDetalle ordenDetalle = new OrdenDetalle()
                {
                    ProductoId = lista.ProductoId,
                    OrdenId = carroCompraVM.Orden.Id,
                    Precio = lista.Precio,
                    Cantidad = lista.Cantidad
                };
                await _unidadTrabajo.OrdenDetalle.Agregar(ordenDetalle);
                await _unidadTrabajo.Guardar();
            }
            // Stripe
            
            await _unidadTrabajo.Guardar();
            TempData[DS.Exitosa] = "Pedido " + "almacenado. ";
            */

            return RedirectToAction("OrdenConfirmacion", new {id = ordenGuardadaId });
        }

        
        public async Task<IActionResult> OrdenConfirmacion(int id)
        {
            return View(id);
        }
    }
}
