using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;

namespace EFood.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]

    public class TipoPrecioController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public TipoPrecioController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Upsert(int? id)
        {
            TipoPrecio tipoPrecio = new TipoPrecio();

            if(id== null)
            {
                // Crear una nueva tipoPrecio
                return View(tipoPrecio);
            }
            // Actualizamos tipoPrecio
            tipoPrecio = await _unidadTrabajo.TipoPrecio.Obtener(id.GetValueOrDefault());
            if(tipoPrecio ==null)
            {
                return NotFound();
            }
            return View(tipoPrecio);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TipoPrecio tipoPrecio)
        {
            if(ModelState.IsValid)
            {
                if(tipoPrecio.Id == 0)
                {
                    await _unidadTrabajo.TipoPrecio.Agregar(tipoPrecio);
                    TempData[DS.Exitosa] = "TipoPrecio creado Exitosamente";
                }
                else
                {
                    _unidadTrabajo.TipoPrecio.Actualizar(tipoPrecio);
                    TempData[DS.Exitosa] = "TipoPrecio actualizado Exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar TipoPrecio";
            return View(tipoPrecio);
        }


        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.TipoPrecio.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var cuponDb = await _unidadTrabajo.TipoPrecio.Obtener(id);
            if(cuponDb == null)
            {
                return Json(new { success = false, message = "Error al borrar TipoPrecio" });
            }
            _unidadTrabajo.TipoPrecio.Remover(cuponDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "TipoPrecio borrado exitosamente" });
        }
        
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id =0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.TipoPrecio.ObtenerTodos();
            if(id==0)
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && b.Id !=id);
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
