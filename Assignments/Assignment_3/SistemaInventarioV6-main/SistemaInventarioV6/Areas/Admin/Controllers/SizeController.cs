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
    public class SizeController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public SizeController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            Size size = new Size();

            if (id == null)
            {
                // Crear el nuevo size
                return View(size);
            }
            // Actualizar el size
            size = await _unidadTrabajo.Size.Obtener(id.GetValueOrDefault());
            if (size == null)
            {
                return NotFound();
            }
            return View(size);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Size size)
        {
            if (ModelState.IsValid)
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (size.ID == 0)
                {
                    await _unidadTrabajo.Size.Agregar(size);
                    _unidadTrabajo.Size.RegistrarBitacora(currentUserID, "Crear nuevo tamaño");
                    TempData[DS.Exitosa] = "Tamaño creado exitosamente";
                }
                else
                {
                    _unidadTrabajo.Size.Actualizar(size);
                    _unidadTrabajo.Size.RegistrarBitacora(currentUserID, "Actualizar tamaño");
                    TempData[DS.Exitosa] = "Tamaño actualizado exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            _unidadTrabajo.Size.RegistrarBitacoraErrores("errorTamaño", "Error al grabar tamaño");
            TempData[DS.Error] = "Error al grabar tamaño";
            return View(size);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Size.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var sizeDB = await _unidadTrabajo.Size.Obtener(id);
            if (sizeDB == null)
            {
                _unidadTrabajo.Size.RegistrarBitacoraErrores("errorTamaño", "Error al eliminar tamaño");
                return Json(new { success = false, message = "Error al borrar tamaño" });
            }
            _unidadTrabajo.Size.Remover(sizeDB);
            _unidadTrabajo.Size.RegistrarBitacora(currentUserID, "Eliminar tamaño");
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Tamaño borrado exitosamente" });
        }

        [ActionName("ValidarDescripcion")]
        public async Task<IActionResult> ValidarDescripcion(string description, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Size.ObtenerTodos();
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
