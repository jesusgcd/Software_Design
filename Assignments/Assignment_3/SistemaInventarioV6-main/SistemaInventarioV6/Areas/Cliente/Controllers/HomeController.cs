using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using SistemaInventarioV6.Modelos.Especificaciones;
using SistemaInventarioV6.Modelos.ViewModels;
using SistemaInventarioV6.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace SistemaInventarioV6.Areas.Cliente.Controllers
{
    [Area("Cliente")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnidadTrabajo _unidadTrabajo;
        [BindProperty]
        public ShoppingCartVM shoppingCartVM { get; set; }

        public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo)
        {
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, string busqueda="", string busquedaActual="")
        {
            if (!String.IsNullOrEmpty(busqueda))
            {
                pageNumber = 1;
            }
            else
            {
                busqueda = busquedaActual;
            }
            ViewData["BusquedaActual"] = busqueda;
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            Parametros parametros = new Parametros()
            {
                PageNumber = pageNumber,
                PageSize = 8
            };

            //Controlar sesión
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim != null)
            {
                var cartList = await _unidadTrabajo.ShoppingCart.ObtenerTodos(c => c.AppUserId == claim.Value);
                var productCount = cartList.Count();  //Cantidad de registros
                HttpContext.Session.SetInt32(DS.ssShoppingCart, productCount);
            }
            
            
            var resultado = _unidadTrabajo.Product.ObtenerTodosPaginado(parametros);

            if (!String.IsNullOrEmpty(busqueda))
            {
                resultado = _unidadTrabajo.Product.ObtenerTodosPaginado(parametros,p=>p.Description.Contains(busqueda));

            }

            ViewData["TotalPaginas"] = resultado.MetaData.TotalPages;
            ViewData["TotalRegistros"] = resultado.MetaData.TotalCount;
            ViewData["PageSize"] = resultado.MetaData.PageSize;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Previo"] = "disabled";
            ViewData["Siguiente"] = "";

            if (pageNumber > 1) { ViewData["Previo"] = ""; }
            if (resultado.MetaData.TotalPages <= pageNumber) { ViewData["Siguiente"] = "disabled"; }
            return View(resultado);
        }

        public async Task<IActionResult> Detail(int id)
        {
            shoppingCartVM = new ShoppingCartVM();
            shoppingCartVM.Product = await _unidadTrabajo.Product.ObtenerPrimero(p => p.ID == id, incluirPropiedades: "FoodCategory");
            shoppingCartVM.PriceList = _unidadTrabajo.ShoppingCart.ObtenerTodosDropDownList("Price", id);
            shoppingCartVM.ShoppingCart = new ShoppingCart()
            {
                ProductId = id,
                Product = shoppingCartVM.Product
            };
            return View(shoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Detail(ShoppingCartVM shoppingCartVM)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVM.ShoppingCart.AppUserId = claim.Value;
            ShoppingCart cartDB = await _unidadTrabajo.ShoppingCart.ObtenerPrimero(c => c.AppUserId == claim.Value &&
                                                                                      c.PriceId == shoppingCartVM.ShoppingCart.PriceId);
            if (cartDB == null)
            {
                await _unidadTrabajo.ShoppingCart.Agregar(shoppingCartVM.ShoppingCart);
            }
            else
            {
                cartDB.Quantity += shoppingCartVM.ShoppingCart.Quantity;
                _unidadTrabajo.ShoppingCart.Actualizar(cartDB);
            }
            await _unidadTrabajo.Guardar();
            TempData[DS.Exitosa] = "Producto agregado al carrito de compras";
            //Agregar valor a la sesión
            var cartList = await _unidadTrabajo.ShoppingCart.ObtenerTodos(c => c.AppUserId == claim.Value);
            var productNumber = cartList.Count();  //Cantidad de registros
            HttpContext.Session.SetInt32(DS.ssShoppingCart, productNumber);
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}