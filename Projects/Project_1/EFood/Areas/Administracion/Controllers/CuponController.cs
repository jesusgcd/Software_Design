using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;

namespace EFood.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]


    public class CuponController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public CuponController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Upsert(int? id)
        {
            Cupon cupon = new Cupon();

            if(id== null)
            {
                // Crear una nueva cupon
                return View(cupon);
            }
            // Actualizamos cupon
            cupon = await _unidadTrabajo.Cupon.Obtener(id.GetValueOrDefault());
            if(cupon ==null)
            {
                return NotFound();
            }
            return View(cupon);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Cupon cupon)
        {
            if(ModelState.IsValid)
            {
                if(cupon.Id == 0)
                {
                    await _unidadTrabajo.Cupon.Agregar(cupon);
                    TempData[DS.Exitosa] = "Cupon creado Exitosamente";
                }
                else
                {
                    _unidadTrabajo.Cupon.Actualizar(cupon);
                    TempData[DS.Exitosa] = "Cupon actualizado Exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar Cupon";
            return View(cupon);
        }


        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Cupon.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var cuponDb = await _unidadTrabajo.Cupon.Obtener(id);
            if(cuponDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Cupon" });
            }
            _unidadTrabajo.Cupon.Remover(cuponDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Cupon borrada exitosamente" });
        }
        
        [ActionName("ValidarCodigo")]
        public async Task<IActionResult> ValidarCodigo(string codigo, int id =0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Cupon.ObtenerTodos();
            if(id==0)
            {
                valor = lista.Any(b => b.Codigo.ToLower().Trim() == codigo.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Codigo.ToLower().Trim() == codigo.ToLower().Trim() && b.Id !=id);
            }
            if(valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });

        }
        

        #endregion
        

    }
}
