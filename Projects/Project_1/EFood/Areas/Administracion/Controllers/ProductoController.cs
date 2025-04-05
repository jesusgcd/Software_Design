using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Hosting;
using EFood.Modelos.ViewModels;
using EFood.AccesoDatos.Repositorio;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EFood.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]

    public class ProductoController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(int? id)
        {

            ProductoVM productoVM = new ProductoVM()
            {
                Producto = new Producto(),
                LineaComidaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("LineaComida")

            };

            if (id == null)
            {
                // Crear nuevo ProductoSI
                return View(productoVM);
            }
            else
            {
                productoVM.Producto = await _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
                if (productoVM.Producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productoVM.Producto.Id == 0)
                {
                    // Crear
                    string upload = webRootPath + DS.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productoVM.Producto.ImagenUrl = fileName + extension;
                    await _unidadTrabajo.Producto.Agregar(productoVM.Producto);

					var claimIdentity = (ClaimsIdentity)User.Identity;
					var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                    /*
					await _unidadTrabajo.Bitacora.Agregar(new Bitacora
                    {
                        // aqui va el usuario logueado
                        UsuarioId = claim.Value,
                        FechaHora = DateTime.Now,
                        Descripcion = "Nuevo Producto Creado, de nombre: " + productoVM.Producto.Nombre
                    });
                    */
                }
                else
                {
                    // Actualizar
                    var objProducto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoVM.Producto.Id, isTracking: false);
                    if (files.Count > 0)  // Si se carga una nueva Imagen para el Producto existente
                    {
                        string upload = webRootPath + DS.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        //Borrar la imagen anterior
                        var anteriorFile = Path.Combine(upload, objProducto.ImagenUrl);
                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productoVM.Producto.ImagenUrl = fileName + extension;
                    } // Caso contrario no se carga una nueva imagen
                    else
                    {
                        productoVM.Producto.ImagenUrl = objProducto.ImagenUrl;
                    }
                    _unidadTrabajo.Producto.Actualizar(productoVM.Producto);
                }
                TempData[DS.Exitosa] = "Transaccion Exitosa!";
                await _unidadTrabajo.Guardar();
                return View("Index");

            }  // If not Valid
            productoVM.LineaComidaLista = _unidadTrabajo.Producto.ObtenerTodosDropdownLista("LineaComida");

            return View(productoVM);
        }

        public async Task<IActionResult> MostrarPrecios(int id)
        {
            var producto = await _unidadTrabajo.Producto.Obtener(id);
            if (producto == null)
            {
                return NotFound();
            }

            var precios = await _unidadTrabajo.Precio.ObtenerPreciosPorProducto(id);

            var mostrarPreciosVM = new MostrarPreciosVM
            {
                Producto = producto,
                Precios = precios
            };

            return View(mostrarPreciosVM);
        }

        public async Task<IActionResult> NuevoPrecio(int id)
        {
            var producto = await _unidadTrabajo.Producto.Obtener(id);
            if (producto == null)
            {
                return NotFound();
            }

            var tiposPrecio = await _unidadTrabajo.TipoPrecio.ObtenerTodos();

            var modelo = new NuevoPrecioVM
            {
                ProductoId = producto.Id,
                TiposPrecio =  _unidadTrabajo.TipoPrecio.ObtenerTodosDropdownLista("TiposPrecio")
            };

            return View(modelo);
        }


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> NuevoPrecio(NuevoPrecioVM modelo)
		{
			if (ModelState.IsValid)
			{
                // Verificar si ya existe un precio para este producto y tipo de precio
                List<Precio> precios = await _unidadTrabajo.Precio.ObtenerPreciosPorProducto(modelo.ProductoId);

                bool existePrecio = false;

                // Verificar si ya existe un precio para este producto y tipo de precio
                foreach (var precio in precios)
                {
                    if (precio.TipoPrecioID == modelo.TipoPrecioId)
                    {
                        existePrecio = true;
                        break;
                    }
                }

                if (modelo.Valor < 1)
                {
                    modelo.mensajeError = "El Valor no puede ser menor a 1";
                    // retorna la vista de creación de precio con el mensaje de error
                    modelo.TiposPrecio = _unidadTrabajo.TipoPrecio.ObtenerTodosDropdownLista("TiposPrecio");
                    return View(modelo);
                }
                if (existePrecio)
                {
                    modelo.mensajeError = "Ya existe un precio para este producto con ese tipo de precio";
                    // retorna la vista de creación de precio con el mensaje de error
                    modelo.TiposPrecio = _unidadTrabajo.TipoPrecio.ObtenerTodosDropdownLista("TiposPrecio");
                    return View(modelo);
                }
                // Crear un nuevo precio
                var nuevoPrecio = new Precio
				{
					ProductoID = modelo.ProductoId,
					TipoPrecioID = modelo.TipoPrecioId,
					Monto = modelo.Valor
				};

				await _unidadTrabajo.Precio.Agregar(nuevoPrecio);
				await _unidadTrabajo.Guardar();

				TempData["Exitoso"] = "Nuevo precio creado correctamente";

				return RedirectToAction("MostrarPrecios", new { id = modelo.ProductoId });
			}

			// Si hay errores de validación, recargar la vista con el modelo
			modelo.TiposPrecio = _unidadTrabajo.TipoPrecio.ObtenerTodosDropdownLista("TiposPrecio");
			return View(modelo);
		}

        public async Task<IActionResult> ActualizarPrecio(int id)
        {
           var precio = await _unidadTrabajo.Precio.Obtener(id);
            if (precio == null)
            {
                return NotFound();
            }
            TipoPrecio tipoPrecio = await _unidadTrabajo.TipoPrecio.Obtener(precio.TipoPrecioID);

            var modelo = new ActualizarPrecioVM
            {
                ProductoId = precio.ProductoID,
                PrecioId = precio.Id,
                TipoPrecioId = precio.TipoPrecioID,
                Valor = precio.Monto,
                //TiposPrecio = _unidadTrabajo.TipoPrecio.ObtenerTodosDropdownLista("TiposPrecio")
                nombreTipoPrecio = tipoPrecio.Nombre
            };

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarPrecio(ActualizarPrecioVM modelo)
        {
            if (ModelState.IsValid)
            {
                if (modelo.Valor < 1)
                {
                    modelo.mensajeError = "El Valor no puede ser menor a 1";
                    // retorna la vista de creación de precio con el mensaje de error
                    return View(modelo);
                }
                // Actualizar el precio
                Precio precioNuevo = await _unidadTrabajo.Precio.Obtener(modelo.PrecioId);
                if (precioNuevo == null)
                {
                    modelo.mensajeError = "Error al obtener precio";
                    // retorna la vista de creación de precio con el mensaje de error
                    return View(modelo);
                }
                precioNuevo.Monto = modelo.Valor;
                _unidadTrabajo.Precio.Actualizar(precioNuevo);
                await _unidadTrabajo.Guardar();

                TempData[DS.Exitosa] = "Precio actualizado exitosamente";

                return RedirectToAction("MostrarPrecios", new { id = modelo.ProductoId });
            }

            // Si hay errores de validación, recargar la vista con el modelo
            return View(modelo);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropiedades: "LineaComida");
            return Json(new { data = todos });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var ProductoDb = await _unidadTrabajo.Producto.Obtener(id);
            if (ProductoDb == null)
            {
                return Json(new { success = false, message = "Error al borrar Producto" });
            }

            // eliminar la imagen del servidor
            string webRootPath = _webHostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, DS.ImagenRuta, ProductoDb.ImagenUrl);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            // eliminar los precios del producto
            var precios = await _unidadTrabajo.Precio.ObtenerPreciosPorProducto(id);
            foreach (var precio in precios)
            {
                _unidadTrabajo.Precio.Remover(precio);
            }

            // eliminar el Producto de la base de datos
            _unidadTrabajo.Producto.Remover(ProductoDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Producto borrado exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Producto.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.Nombre.ToLower().Trim() == nombre.ToLower().Trim() && b.Id != id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });

        }


		public async Task<IActionResult> EliminarPrecio(int id)
		{
			var precioDb = await _unidadTrabajo.Precio.Obtener(id);
			if (precioDb == null)
			{
				return NotFound();
			}

			try
			{
				_unidadTrabajo.Precio.Remover(precioDb);
				await _unidadTrabajo.Guardar();

				TempData["Exitoso"] = "Precio eliminado correctamente";
				return RedirectToAction("MostrarPrecios", new { id = precioDb.ProductoID });
			}
			catch (Exception ex)
			{
				// Manejar cualquier error
				TempData["Error"] = $"Error al eliminar el precio: {ex.Message}";
				return RedirectToAction("MostrarPrecios", new { id = precioDb.ProductoID });
			}
		}


		#endregion
	}
}
