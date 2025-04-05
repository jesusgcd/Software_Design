using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SistemaInventarioV6.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class OrderController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public OrderController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var orderList = await _unidadTrabajo.Order.ObtenerTodos(o=>o.AppUserId == claim.Value, incluirPropiedades: "PaymentProcessor");
            return Json(new { data = orderList });
        }
        #endregion
    }
}
