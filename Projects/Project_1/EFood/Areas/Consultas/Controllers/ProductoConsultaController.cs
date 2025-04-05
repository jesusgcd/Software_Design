using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Modelos.ViewModels;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;

namespace EFood.Areas.Consultas.Controllers
{
    [Area("Consultas")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Consulta)]

    public class ProductoConsultaController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoConsultaController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            ConsultarProductosVM consultarProductosVM = new ConsultarProductosVM()
            {
                LineaComidaId = 0,
                LineaComidaLista = _unidadTrabajo.LineaComida.ObtenerTodosDropdownLista("LineaComida"),
                MensajeError = "Linea de Comida sin seleccionar"

            };
            return View(consultarProductosVM);
        }

        [HttpPost]
        public IActionResult BuscarProductos(ConsultarProductosVM model)
        {
            if (ModelState.IsValid)
            {
                List<Array> listaProductos = new List<Array>();

                // Ejemplo de datos ficticios
                // listaProductos.Add(new object[] { "001", "Producto 1", 10.99 });

                // traer todos los productos donde la linea de comida sea igual a la seleccionada
                IEnumerable<Producto> productos = (IEnumerable<Producto>)_unidadTrabajo.Producto.ObtenerProductosPorLineaComida(model.LineaComidaId);

                // recorrer la lista de productos y agregarlos a la lista de productos
                foreach (var producto in productos)
                {
                    // traer todos los precios del producto usado ObtenerPreciosPorProducto
                    List<Precio> precios = _unidadTrabajo.Precio.ObtenerPreciosPorProducto(producto.Id).Result;

                    if (precios.Count == 0)
                    {
                        listaProductos.Add(new object[] { producto.Id, producto.Nombre, "Sin Precio" });
                    }
                    else
                    {
                        // recorrer la lista de precios y agregarlos a la lista de productos
                        foreach (var precio in precios)
                        {
                            listaProductos.Add(new object[] { producto.Id, producto.Nombre, precio.Monto });
                        }
                    }
                    

                }


                if (listaProductos.Count == 0)
                {
                    model.ListaProductos = null;
                    model.MensajeError = "No se encontraron productos en la linea de comida seleccionada";
                }
                else
                {
                    model.ListaProductos = listaProductos;
                }

                model.LineaComidaLista = _unidadTrabajo.LineaComida.ObtenerTodosDropdownLista("LineaComida");

                return View("Index", model);
            }

            // Si hay errores de validación en el modelo, puedes devolver la vista con los errores.
            return View("Index", model);
        }



        #region API


        #endregion

    }
}
