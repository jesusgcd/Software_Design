using SistemaInventarioV6.AccesoDatos.Data;
using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using SistemaInventarioV6.Modelos.ViewModels;
using SistemaInventarioV6.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace SistemaInventarioV6.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento+","+DS.Role_Consulta)]
    public class ProductController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
        public async Task<IActionResult> Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                FoodCategoryList = _unidadTrabajo.Product.ObtenerTodosDropDownList("FoodCategory")
            };

            if (id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = await _unidadTrabajo.Product.Obtener(id.GetValueOrDefault());
                if (productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProductVM productVM)
        {
            ModelState.Clear(); //Limpiar el ModelState porque si no, siempre lo considera inválido
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (productVM.Product.ID == 0)
                {
                    string upload = webRootPath + DS.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productVM.Product.ImageURL = fileName + extension;
                    await _unidadTrabajo.Product.Agregar(productVM.Product);
                    _unidadTrabajo.Product.RegistrarBitacora(currentUserID, "Crear nuevo producto");
                }
                else
                {
                    var objProduct = await _unidadTrabajo.Product.ObtenerPrimero(p => p.ID == productVM.Product.ID, isTracking: false);
                    if(files.Count>0)
                    {
                        string upload = webRootPath + DS.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var anteriorFile = Path.Combine(upload, objProduct.ImageURL);
                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productVM.Product.ImageURL = fileName + extension;

                    }
                    else
                    {
                        productVM.Product.ImageURL = objProduct.ImageURL;
                    }
                    _unidadTrabajo.Product.Actualizar(productVM.Product);
                    _unidadTrabajo.Product.RegistrarBitacora(currentUserID, "Actualizar producto");
                }
                TempData[DS.Exitosa] = "Trasacción exitosa";
                await _unidadTrabajo.Guardar();
                return RedirectToAction("Index");
            }
            productVM.FoodCategoryList = _unidadTrabajo.Product.ObtenerTodosDropDownList("FoodCategory");
            _unidadTrabajo.Product.RegistrarBitacoraErrores("errorProducto", "Error al grabar producto");
            return View(productVM);
            
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Product.ObtenerTodos(incluirPropiedades:"FoodCategory");
            return Json(new { data = todos });
        }

        [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var productDB = await _unidadTrabajo.Product.Obtener(id);
            if (productDB == null)
            {
                _unidadTrabajo.Product.RegistrarBitacoraErrores("errorProducto", "Error al eliminar producto");
                return Json(new { success = false, message = "Error al borrar producto" });
            }
            string upload = _webHostEnvironment.WebRootPath + DS.ImagenRuta;
            var anteriorFile = Path.Combine(upload, productDB.ImageURL);
            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile);
            }
            _unidadTrabajo.Product.Remover(productDB);
            _unidadTrabajo.Product.RegistrarBitacora(currentUserID, "Eliminar producto");
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Producto borrado exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Product.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(reg => reg.Name.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(reg => reg.Name.ToLower().Trim() == nombre.ToLower().Trim() && reg.ID != id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });

        }
        #endregion

    }
}
