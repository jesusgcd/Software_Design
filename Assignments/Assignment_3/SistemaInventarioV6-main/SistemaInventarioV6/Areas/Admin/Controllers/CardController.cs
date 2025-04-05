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
    [Authorize(Roles =DS.Role_Admin +","+DS.Role_Mantenimiento )]
    public class CardController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public CardController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Card card = new Card();

            if (id == null)
            {
                // Crear la nueva tarjeta
                return View(card);
            }
            // Actualizar el size
            card = await _unidadTrabajo.Card.Obtener(id.GetValueOrDefault());
            if (card == null)
            {
                return NotFound();
            }
            return View(card);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Card card)
        {
            if (ModelState.IsValid)
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (card.ID == 0)
                {
                    await _unidadTrabajo.Card.Agregar(card);
                    _unidadTrabajo.Card.RegistrarBitacora(currentUserID,"Crear nueva tarjeta");
                    TempData[DS.Exitosa] = "Tarjeta creada exitosamente";
                }
                else
                {
                    _unidadTrabajo.Card.Actualizar(card);
                    _unidadTrabajo.Card.RegistrarBitacora(currentUserID, "Actualizar tarjeta existente");
                    TempData[DS.Exitosa] = "Tarjeta actualizada exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            _unidadTrabajo.Card.RegistrarBitacoraErrores("errorTarjeta", "Error al grabar tarjeta");
            TempData[DS.Error] = "Error al grabar tarjeta";
            return View(card);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Card.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            
            var cardDB = await _unidadTrabajo.Card.Obtener(id);
            if (cardDB == null)
            {
                _unidadTrabajo.Card.RegistrarBitacoraErrores("errorTarjeta", "Error al eliminar tarjeta");
                return Json(new { success = false, message = "Error al borrar tarjeta" });
            }
            _unidadTrabajo.Card.Remover(cardDB);
            _unidadTrabajo.Card.RegistrarBitacora(currentUserID, "Eliminar tarjeta");
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Tarjeta borrada exitosamente" });
        }

        [ActionName("ValidarDescripcion")]
        public async Task<IActionResult> ValidarDescripcion(string description, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Card.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(reg => reg.Description.ToLower().Trim() == description.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(reg => reg.Description.ToLower().Trim() == description.ToLower().Trim() && reg.ID != id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });

        }
        #endregion

    }
}
