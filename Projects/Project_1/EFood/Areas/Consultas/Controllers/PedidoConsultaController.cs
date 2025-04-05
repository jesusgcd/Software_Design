using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using EFood.Modelos.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace EFood.Areas.Consultas.Controllers
{
    [Area("Consultas")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Consulta)]

    public class PedidoConsultaController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PedidoConsultaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {

            List<Array> listaPedidos = new List<Array>();

            // traer todas las ordenes
            var listaOrdenes = _unidadTrabajo.Orden.ObtenerTodosLista();

            foreach (var item in listaOrdenes.Result)
            {
                if (item.MontoTotal <= 0)
                {
                    // eliminar la orden si el monto total es 0
                    _unidadTrabajo.Orden.Remover(item);
                    _unidadTrabajo.Guardar();
                }
                else
                {
                    listaPedidos.Add(new object[] { item.Id, item.FechaHora, item.MontoTotal, item.Estado });
                }
            }


            // Se crea un objeto de tipo PedidoConsultaVM con todos null o vacios
            PedidoConsultaVM pedidoConsultaVM = new PedidoConsultaVM()
            {
                EstadoSeleccionado = "",
                ListaPedidos = listaPedidos,
                mensajeError = ""
            };

            return View(pedidoConsultaVM);
        }



        [HttpPost]
        public IActionResult FiltrarPedidoFechaEstado(DateTime fechaInicio, DateTime fechaFin, String estadoSeleccionado)
        {


            // Se crea un objeto de tipo PedidoConsultaVM con todos null o vacios
            PedidoConsultaVM pedidoConsultaVM = new PedidoConsultaVM()
            {
                EstadoSeleccionado = estadoSeleccionado,
                ListaPedidos = null,
                mensajeError = ""
            };

            if (fechaInicio >= fechaFin)
            {
                pedidoConsultaVM.mensajeError = "La fecha de inicio debe ser menor a la fecha de fin";
                return View("Index", pedidoConsultaVM);
            }


            List<Array> listaPedidos = new List<Array>();


            // traer todas las ordenes filtradas
            List<Orden> listaOrdenes = _unidadTrabajo.Orden.ObtenerTodosLista(fechaInicio, fechaFin, estadoSeleccionado).Result;

            foreach (var item in listaOrdenes)
            {
                listaPedidos.Add(new object[] { item.Id, item.FechaHora, item.MontoTotal, item.Estado });
            }

            pedidoConsultaVM.ListaPedidos = listaPedidos;


            return View("Index", pedidoConsultaVM);
        }


        public async Task<IActionResult> MostrarPedidos(int id)
        {
            // Obtener los OrdenesDetalles de la orden seleccionada
            var ordenes = await _unidadTrabajo.OrdenDetalle.ObteneOrdenDetallePorOrdenId(id);


            // Verificar si se obtuvo la orden correctamente
            if (ordenes == null)
            {
                // Manejar el caso cuando no se encuentra la orden
                return NotFound("No se encontraron detalles para la orden seleccionada.");
            }

            // Obtener la orden
            Orden orden = await _unidadTrabajo.Orden.Obtener(id);

            // Verificar si se obtuvo la orden correctamente
            if (orden == null)
            {
                // Manejar el caso cuando no se encuentra la orden
                return NotFound("No se encontró la orden seleccionada.");
            }

            List<Array> listaOrdenesDetalles = new List<Array>();

            foreach (var item in ordenes)
            {
                // Obtener el nombre del producto
                var producto = await _unidadTrabajo.Producto.Obtener(item.ProductoId);
                if (producto == null)
                {
                    return NotFound($"Producto con ID {item.ProductoId} no encontrado.");
                }
                String nombreProducto = producto.Nombre;

                // Obtener el tipo de precio
                var tipoPrecioObj = await _unidadTrabajo.TipoPrecio.Obtener(item.TipoPrecioId);
                if (tipoPrecioObj == null)
                {
                    return NotFound($"Tipo de precio con ID {item.TipoPrecioId} no encontrado.");
                }
                String tipoPrecio = tipoPrecioObj.Nombre;

                // Obtener el precio
                var precioObj = await _unidadTrabajo.Precio.ObtenerPrimero(p => p.ProductoID == item.ProductoId && p.TipoPrecioID == item.TipoPrecioId);
                if (precioObj == null)
                {
                    return NotFound($"Precio con ID {item.TipoPrecioId} no encontrado.");
                }
                double precio = precioObj.Monto;

                listaOrdenesDetalles.Add(new object[] { nombreProducto, tipoPrecio, precio, item.Cantidad, item.Monto, item.Id, item.OrdenId });
            }
            

            // Crear el ViewModel
            MostrarPedidosVM mostrarPedidos = new MostrarPedidosVM()
            {
                NumeroOrden = orden.Id,
                NombreUsuario = orden.NombreCliente + orden.ApellidosCliente,
                OrdenesDetalles = listaOrdenesDetalles
            };
            


            return View(mostrarPedidos);
        }




        #region API

      
        public async Task<IActionResult> EliminarPedido(int id)
        {
            var ordenDetalleDb = await _unidadTrabajo.OrdenDetalle.Obtener(id);
            if (ordenDetalleDb == null)
            {
                return Json(new { success = false, message = "Pedido no encontrado." });
            }

            try
            {
                _unidadTrabajo.OrdenDetalle.Remover(ordenDetalleDb);
                // restar el monto del pedido al total de la orden
                var ordenDb = await _unidadTrabajo.Orden.Obtener(ordenDetalleDb.OrdenId);
                ordenDb.MontoTotal -= ordenDetalleDb.Monto;
                _unidadTrabajo.Orden.Actualizar(ordenDb);

                

                await _unidadTrabajo.Guardar();

                TempData["Exitoso"] = "Precio eliminado correctamente";
                return RedirectToAction("MostrarPedidos", new { id = ordenDetalleDb.OrdenId });
            }
            catch (Exception ex)
            {
                // Manejar cualquier error
                TempData["Error"] = $"Error al eliminar el pedido: {ex.Message}";
                return RedirectToAction("MostrarPedidos", new { id = ordenDetalleDb.OrdenId });
            }
        }


        #endregion
    }
}
