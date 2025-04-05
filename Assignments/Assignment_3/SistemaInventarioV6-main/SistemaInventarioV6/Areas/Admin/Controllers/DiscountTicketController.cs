using SistemaInventarioV6.AccesoDatos.Data;
using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using SistemaInventarioV6.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace SistemaInventarioV6.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    public class DiscountTicketController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public DiscountTicketController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            DiscountTicket discountTicket = new DiscountTicket();

            if (id == null)
            {
                // Crear el nuevo tiquete
                return View(discountTicket);
            }
            // Actualizar el tiquete existente
            discountTicket = await _unidadTrabajo.DiscountTicket.Obtener(id.GetValueOrDefault());
            if (discountTicket == null)
            {
                return NotFound();
            }
            return View(discountTicket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(DiscountTicket discountTicket)
        {
            if (ModelState.IsValid)
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (discountTicket.ID == 0)
                {
                    await _unidadTrabajo.DiscountTicket.Agregar(discountTicket);
                    _unidadTrabajo.DiscountTicket.RegistrarBitacora(currentUserID, "Crear nuevo tiquete de descuento");
                    TempData[DS.Exitosa] = "Tiquete de descuento creado exitosamente";
                }
                else
                {
                    _unidadTrabajo.DiscountTicket.Actualizar(discountTicket);
                    _unidadTrabajo.DiscountTicket.RegistrarBitacora(currentUserID, "Actualizar tiquete de descuento");
                    TempData[DS.Exitosa] = "Tiquete de descuento actualizado exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            _unidadTrabajo.DiscountTicket.RegistrarBitacoraErrores("errorTiquete", "Error al grabar tiquete de descuento");
            TempData[DS.Error] = "Error al grabar tiquete de descuento";
            return View(discountTicket);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.DiscountTicket.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var discountTicketDB = await _unidadTrabajo.DiscountTicket.Obtener(id);
            if (discountTicketDB == null)
            {
                _unidadTrabajo.DiscountTicket.RegistrarBitacoraErrores("errorTiquete", "Error al eliminar tiquete de descuento");
                return Json(new { success = false, message = "Error al borrar tiquete de descuento" });
            }
            _unidadTrabajo.DiscountTicket.Remover(discountTicketDB);
            _unidadTrabajo.DiscountTicket.RegistrarBitacora(currentUserID, "Eliminar tiquete de descuento");
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Tiquete de descuento borrado exitosamente" });
        }

        [ActionName("ValidarCodigo")]
        public async Task<IActionResult> ValidarCodigo(string code, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.DiscountTicket.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(reg => reg.Code.ToLower().Trim() == code.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(reg => reg.Code.ToLower().Trim() == code.ToLower().Trim() && reg.ID != id);
            }
            return Json(new { data = valor });

        }
        #endregion

    }
}
