using SistemaInventarioV6.AccesoDatos.Data;
using SistemaInventarioV6.AccesoDatos.Repositorio;
using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using SistemaInventarioV6.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Linq.Expressions;

namespace SistemaInventarioV6.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin+ ","+DS.Role_Consulta)]
    public class LogController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        public LogController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> FiltrarFecha(DateTime fechaInicio, DateTime fechaFinal)
        {
            
            if (fechaInicio != DateTime.MinValue && fechaFinal != DateTime.MinValue)
            {
                Expression<Func<Log, bool>> filtro = x => x.TimeStamp >= fechaInicio && x.TimeStamp <= fechaFinal;
                var logListFiltrado = await _unidadTrabajo.Log.ObtenerTodos(filtro);
                foreach  (var log in logListFiltrado)
                {
                    Console.WriteLine(log.Description);
                }
                return View("Index", logListFiltrado);
            }

            return View();
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Log.ObtenerTodos(incluirPropiedades:"AppUser");
            return Json(new { data = todos });
        }

        #endregion
    }
}
