using Microsoft.AspNetCore.Mvc;
using EFood.Modelos.ErrorViewModels;
using System.Diagnostics;

using EFood.Modelos;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos.Especificaciones;
using EFood.Utilidades;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using EFood.Modelos.ViewModels;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EFood.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
		private readonly ILogger<HomeController> _logger;
		private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IHostApplicationLifetime _appLifetime;

        [BindProperty]
        public CarroCompraVM carroCompraVM { get; set; }

        public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo)
        {
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }

        private string ObtenerSesionUsuario()
        {
            if (HttpContext.Session.GetString("SessionId") == null)
            {
                HttpContext.Session.SetString("SessionId", System.Guid.NewGuid().ToString());
            }
            return HttpContext.Session.GetString("SessionId");
        }


        //Busqueda Actual es el anterior
        public async Task<IActionResult> Index(int pageNumber = 1, string busqueda = "", string busquedaActual = "", int LineaComidaId = 0)
		{
            /*
			// Controlar sesion
			var claimIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
			
			if (claim != null)
			{
				var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(c => c.UsuarioAplicacionId == claim.Value);
				var numeroProductos = carroLista.Count();  // Numero de Registros
				HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos);
			}
			*/
            //
            if (!String.IsNullOrEmpty(busqueda))
            {
                pageNumber = 1;
            }
            else
            {
                busqueda = busquedaActual;
            }


            ViewData["BusquedaActual"] = busqueda;

            if (pageNumber < 1) { pageNumber = 1; }

            Parametros parametros = new Parametros()
            {
                PageNumber = pageNumber,
                PageSize = 4
            };

            //Obtener todos de todos
            var resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros);


			//Linea comida especifica y nombre especifico
			if (!String.IsNullOrEmpty(busqueda) && LineaComidaId > 0)
			{
                resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros, p => p.Nombre.Contains(busqueda) && p.LineaComidaId == LineaComidaId);

            }
            //Solo linea comida especifica
            if (String.IsNullOrEmpty(busqueda) && LineaComidaId > 0)
            {
                resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros, p => p.LineaComidaId == LineaComidaId);
            }

            //Solo nombre especifico
            if (!String.IsNullOrEmpty(busqueda) && LineaComidaId <= 0)
			{
				resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros, p => p.Nombre.Contains(busqueda));
            }
            var lineaComidaList = _unidadTrabajo.LineaComida.ObtenerTodosDropdownLista("LineaComida").ToList();
            lineaComidaList.Insert(0, new SelectListItem { Value = "-1", Text = "Todas las lineas de comida" });

            var inicioVM = new InicioVM
            {
                LineaComidaId = LineaComidaId,
                ProductosPagedList = resultado,
                LineaComidaList = lineaComidaList
            };

            ViewData["TotalPaginas"] = resultado.MetaData.TotalPages;
            ViewData["TotalRegistros"] = resultado.MetaData.TotalCount;
            ViewData["PageSize"] = resultado.MetaData.PageSize;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Previo"] = "disabled";  // clase css para desactivar el boton
            ViewData["Siguiente"] = "";


            if (pageNumber > 1) { ViewData["Previo"] = ""; }
            if (resultado.MetaData.TotalPages <= pageNumber) { ViewData["Siguiente"] = "disabled"; }
            return View(inicioVM);
		}
        public async Task<IActionResult> Detalle(int id)
        {
            carroCompraVM = new CarroCompraVM();
            var sessionID = ObtenerSesionUsuario();
            carroCompraVM.Producto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == id);
            carroCompraVM.PrecioLista = await _unidadTrabajo.Precio.ObtenerPreciosPorProducto(id);

            carroCompraVM.CarroCompra = new CarroCompra()
            {
                Producto = carroCompraVM.Producto,
                ProductoId = carroCompraVM.Producto.Id,
                SesionUsuario = sessionID
            };

            return View(carroCompraVM);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detalle(CarroCompraVM carroCompraVM)
        {
            var sessionID = carroCompraVM.CarroCompra.SesionUsuario;
            CarroCompra carroBD = await _unidadTrabajo.CarroCompra.ObtenerPrimero(c => c.SesionUsuario == sessionID &&
                                                                                      c.ProductoId == carroCompraVM.CarroCompra.ProductoId &&
                                                                                      c.PrecioId == carroCompraVM.CarroCompra.PrecioId);
            
            if (carroBD == null)
            {
                //el producto no ha sido agregado
                await _unidadTrabajo.CarroCompra.Agregar(carroCompraVM.CarroCompra);
            }
            else
            {
                //El producto ha sido agregado previamente
                carroBD.Cantidad += carroCompraVM.CarroCompra.Cantidad;
                _unidadTrabajo.CarroCompra.Actualizar(carroBD);
            }
            await _unidadTrabajo.Guardar();
            TempData[DS.Exitosa] = "Producto agregado al Carro de Compras";

            // Agregar valor a la Sesion
            /*
            var carroLista = await _unidadTrabajo.CarroCompra.ObtenerTodos(c => c.UsuarioAplicacionId == claim.Value);
            var numeroProductos = carroLista.Count();  // Numero de Registros
            HttpContext.Session.SetInt32(DS.ssCarroCompras, numeroProductos);
            */

            return RedirectToAction("Index");

        }
        


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //Redireccionar al 
        /*
        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated && !HttpContext.Session.Keys.Contains("HasRedirected"))
            {
                var usuario = await _signInManager.UserManager.GetUserAsync(User);
                if (usuario != null && await _signInManager.UserManager.IsRememberMeAsync(usuario))
                {
                    // Redirigir a una URL específica después del inicio de sesión
                    HttpContext.Session.SetString("HasRedirected", "true");
                    return LocalRedirect("~/Admin/Home");
                }
            }

            return Page();
        }
        */

    }
}