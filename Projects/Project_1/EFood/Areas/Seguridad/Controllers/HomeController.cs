using Microsoft.AspNetCore.Mvc;

namespace EFood.Areas.Seguridad.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
