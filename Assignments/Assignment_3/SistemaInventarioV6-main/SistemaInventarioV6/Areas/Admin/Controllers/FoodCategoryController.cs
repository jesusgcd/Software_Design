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
    public class FoodCategoryController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public FoodCategoryController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            FoodCategory foodCategory = new FoodCategory();

            if (id == null)
            {
                // Crear la nueva categoria
                return View(foodCategory);
            }
            // Actualizar la categoria
            foodCategory = await _unidadTrabajo.FoodCategory.Obtener(id.GetValueOrDefault());
            if (foodCategory == null)
            {
                return NotFound();
            }
            return View(foodCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(FoodCategory foodCategory)
        {
            if (ModelState.IsValid)
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (foodCategory.ID == 0)
                {
                    await _unidadTrabajo.FoodCategory.Agregar(foodCategory);
                    _unidadTrabajo.FoodCategory.RegistrarBitacora(currentUserID, "Crear nuevo categoria de comida");
                    TempData[DS.Exitosa] = "Categoría creada exitosamente";
                }
                else
                {
                    _unidadTrabajo.FoodCategory.Actualizar(foodCategory);
                    _unidadTrabajo.FoodCategory.RegistrarBitacora(currentUserID, "Actualizar categoria de comida");
                    TempData[DS.Exitosa] = "Categoría actualizada exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            _unidadTrabajo.FoodCategory.RegistrarBitacoraErrores("errorCategoria", "Error al grabar categoria de comida");
            TempData[DS.Error] = "Error al grabar categoría";
            return View(foodCategory);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.FoodCategory.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var foodCategoryDB = await _unidadTrabajo.FoodCategory.Obtener(id);
            if (foodCategoryDB == null)
            {
                _unidadTrabajo.FoodCategory.RegistrarBitacoraErrores("errorCategoria", "Error al eliminar categoria de comida");
                return Json(new { success = false, message = "Error al borrar categoría" });
            }
            _unidadTrabajo.FoodCategory.Remover(foodCategoryDB);
            _unidadTrabajo.FoodCategory.RegistrarBitacora(currentUserID, "Eliminar categoria de comida");
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Categoría borrada exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string name, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.FoodCategory.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(reg => reg.Name.ToLower().Trim() == name.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(reg => reg.Name.ToLower().Trim() == name.ToLower().Trim() && reg.ID != id);
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
