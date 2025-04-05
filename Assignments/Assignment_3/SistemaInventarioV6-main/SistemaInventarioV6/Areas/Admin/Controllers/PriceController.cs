using SistemaInventarioV6.AccesoDatos.Data;
using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using SistemaInventarioV6.Modelos.ViewModels;
using SistemaInventarioV6.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace SistemaInventarioV6.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    public class PriceController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public PriceController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            PriceVM priceVM = new PriceVM()
            {
                Price = new Price(),
                SizeList = _unidadTrabajo.Price.ObtenerTodosDropDownList("Size"),
                ProductList = _unidadTrabajo.Price.ObtenerTodosDropDownList("Product")
            };
            if (id == null)
            {
                return View(priceVM);
            }
            else
            {
                priceVM.Price = await _unidadTrabajo.Price.Obtener(id.GetValueOrDefault());
                if (priceVM.Price == null)
                {
                    return NotFound();
                }
                return View(priceVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(PriceVM priceVM)
        {
            ModelState.Clear(); //Limpiar el ModelState porque si no, siempre lo considera inválido
            if (ModelState.IsValid)
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (priceVM.Price.ID == 0)
                {
                    await _unidadTrabajo.Price.Agregar(priceVM.Price);
                    _unidadTrabajo.Price.RegistrarBitacora(currentUserID, "Crear nuevo precio");
                    TempData[DS.Exitosa] = "Precio creado exitosamente";
                }
                else
                {
                    _unidadTrabajo.Price.Actualizar(priceVM.Price);
                    _unidadTrabajo.Price.RegistrarBitacora(currentUserID, "Actualizar precio");
                    TempData[DS.Exitosa] = "Precio actualizado exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            priceVM.SizeList = _unidadTrabajo.Price.ObtenerTodosDropDownList("Size");
            priceVM.ProductList = _unidadTrabajo.Price.ObtenerTodosDropDownList("Product");
            _unidadTrabajo.Price.RegistrarBitacoraErrores("errorPrecio", "Error al grabar precio");
            return View(priceVM);

        }


        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Price.ObtenerTodos(incluirPropiedades: "Product,Size");
            return Json(new { data = todos });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var priceDB = await _unidadTrabajo.Price.Obtener(id);
            if (priceDB == null)
            {
                _unidadTrabajo.Price.RegistrarBitacoraErrores("errorPrecio", "Error al eliminar precio");
                return Json(new { success = false, message = "Error al eliminar precio" });
            }
            _unidadTrabajo.Price.Remover(priceDB);
            _unidadTrabajo.Price.RegistrarBitacora(currentUserID, "Eliminar precio");
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Precio eliminado exitosamente" });
        }

        //Validación para asegurar que solo haya un precio asociado a un tamaño
        [ActionName("ValidarRepetidos")]
        public async Task<IActionResult> ValidarRepetidos(int productId, int sizeId)
        {
            var lista = await _unidadTrabajo.Price.ObtenerTodos();
            bool valor = lista.Any(reg => reg.ProductId == productId && reg.SizeId == sizeId);
            return Json(new { data = valor });

        }
        #endregion

    }
}
