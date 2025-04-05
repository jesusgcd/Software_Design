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
    public class ErrorController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        public ErrorController(IUnidadTrabajo unidadTrabajo)
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
                Expression<Func<Error, bool>> filtro = x => x.Date >= fechaInicio && x.Date <= fechaFinal;
                var errorListFiltrado = await _unidadTrabajo.Error.ObtenerTodos(filtro);
                foreach  (var error in errorListFiltrado)
                {
                    Console.WriteLine(error.Description);
                }
                return View("Index", errorListFiltrado);
            }

            return View();
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Error.ObtenerTodos();
            return Json(new { data = todos });
        }

        #endregion
    }
}
