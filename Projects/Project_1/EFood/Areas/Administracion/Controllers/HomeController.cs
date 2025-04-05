using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Modelos.ViewModels;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;

namespace EFood.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize]
    public class HomeController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }



    }
}
