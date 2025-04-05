using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using SistemaInventarioV6.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SistemaInventarioV6.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Consulta)]
    public class OrderController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public OrderController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Update(int id)
        {
            Order order = await _unidadTrabajo.Order.Obtener(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Order order)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            _unidadTrabajo.Order.Actualizar(order);
            _unidadTrabajo.Order.RegistrarBitacora(currentUserID, "Actualizar orden");
            TempData[DS.Exitosa] = "Estado de la orden actualizado";
            await _unidadTrabajo.Guardar();
            return RedirectToAction(nameof(Index));
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var orderList = await _unidadTrabajo.Order.ObtenerTodos(incluirPropiedades: "PaymentProcessor,AppUser");
            return Json(new { data = orderList });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var orderDB = await _unidadTrabajo.Order.Obtener(id);
            if (orderDB == null)
            {
                _unidadTrabajo.Order.RegistrarBitacoraErrores("errorOrden", "Error al eliminar orden");
                return Json(new { success = false, message = "Error al borrar orden" });
            }
            var orderDetails = await _unidadTrabajo.OrderDetail.ObtenerTodos(d=>d.OrderId == id);
            List<OrderDetail> detailsList = orderDetails.ToList();
            _unidadTrabajo.OrderDetail.RemoverRango(detailsList);
            _unidadTrabajo.Order.Remover(orderDB);
            _unidadTrabajo.Order.RegistrarBitacora(currentUserID, "Eliminar orden");
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Orden borrada exitosamente" });
        }
        #endregion
    }
}
