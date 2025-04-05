using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;

namespace EFood.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]

    public class LineaComidaController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;

        public LineaComidaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Upsert(int? id)
        {
            LineaComida lineaComida = new LineaComida();

            if(id== null)
            {
                // Crear una nueva lineaComida
                return View(lineaComida);
            }
            // Actualizamos lineaComida
            lineaComida = await _unidadTrabajo.LineaComida.Obtener(id.GetValueOrDefault());
            if(lineaComida ==null)
            {
                return NotFound();
            }
            return View(lineaComida);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(LineaComida lineaComida)
        {
            if(ModelState.IsValid)
            {
                if(lineaComida.Id == 0)
                {
                    await _unidadTrabajo.LineaComida.Agregar(lineaComida);
                    TempData[DS.Exitosa] = "LineaComida creada Exitosamente";
                }
                else
                {
                    _unidadTrabajo.LineaComida.Actualizar(lineaComida);
                    TempData[DS.Exitosa] = "LineaComida actualizada Exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar LineaComida";
            return View(lineaComida);
        }


        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.LineaComida.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var cuponDb = await _unidadTrabajo.LineaComida.Obtener(id);
            if(cuponDb == null)
            {
                return Json(new { success = false, message = "Error al borrar LineaComida" });
            }
            _unidadTrabajo.LineaComida.Remover(cuponDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "LineaComida borrado exitosamente" });
        }
        
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id =0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.LineaComida.ObtenerTodos();
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
