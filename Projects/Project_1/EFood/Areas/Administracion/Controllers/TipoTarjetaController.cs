using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;

namespace EFood.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]

    public class TipoTarjetaController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public TipoTarjetaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Upsert(int? id)
        {
            TipoTarjeta tipoTarjeta = new TipoTarjeta();

            if(id== null)
            {
                // Crear una nueva tipoTarjeta
                return View(tipoTarjeta);
            }
            // Actualizamos tipoTarjeta
            tipoTarjeta = await _unidadTrabajo.TipoTarjeta.Obtener(id.GetValueOrDefault());
            if(tipoTarjeta ==null)
            {
                return NotFound();
            }
            return View(tipoTarjeta);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TipoTarjeta tipoTarjeta)
        {
            if(ModelState.IsValid)
            {
                if(tipoTarjeta.Id == 0)
                {
                    await _unidadTrabajo.TipoTarjeta.Agregar(tipoTarjeta);
                    TempData[DS.Exitosa] = "TipoTarjeta creado Exitosamente";
                }
                else
                {
                    _unidadTrabajo.TipoTarjeta.Actualizar(tipoTarjeta);
                    TempData[DS.Exitosa] = "TipoTarjeta actualizado Exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar TipoTarjeta";
            return View(tipoTarjeta);
        }


        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.TipoTarjeta.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var cuponDb = await _unidadTrabajo.TipoTarjeta.Obtener(id);
            if(cuponDb == null)
            {
                return Json(new { success = false, message = "Error al borrar TipoTarjeta" });
            }
            _unidadTrabajo.TipoTarjeta.Remover(cuponDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "TipoTarjeta borrada exitosamente" });
        }
        
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id =0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.TipoTarjeta.ObtenerTodos();
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
