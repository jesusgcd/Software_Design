using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Modelos.ViewModels;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;

namespace EFood.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]

    public class ProcesadorPagoController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProcesadorPagoController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
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
            ProcesadorPagoVM procesadorpagoVM = new ProcesadorPagoVM()
            {
                ProcesadorPago = new ProcesadorPago(),
                TipoTarjeta = _unidadTrabajo.ProcesadorPago.ObtenerTodosDropdownLista("TipoTarjeta"),
                MetodoPagoLista = _unidadTrabajo.ProcesadorPago.ObtenerTodosDropdownLista("MetodoPago")

            };
            // Procesador de reserva
            if (id == null)
            {
                return View(procesadorpagoVM);
            }
            else
            {
                procesadorpagoVM.ProcesadorPago = await _unidadTrabajo.ProcesadorPago.Obtener(id.GetValueOrDefault());

                if (procesadorpagoVM.ProcesadorPago == null)
                {
                    return NotFound();
                }
            }
            return View(procesadorpagoVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(ProcesadorPagoVM model)
        {
            if (ModelState.IsValid)
            {
                //Para crear un nuevo procesador de pago
                if (model.ProcesadorPago.Id == 0)
                {
                    /*
                     ‐	Solamente debe de haber un medio de pago que sea efectivo. Si se ingresa otro que sea del mismo tipo, 
                    indicarle al usuario que ya existe ese tipo de pago
                    */
                    if (model.ProcesadorPago.MetodoPagoId == 1)
                    {
                        // validar si ya existe un procesador de pago con el metodo de pago efectivo
                        var procesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerTodos(p => p.MetodoPagoId == 1);
                        if (procesadorPago.Count() > 0)
                        {
                            TempData[DS.Error] = "Ya existe un procesador de pago con el metodo de pago efectivo";
                            model.TipoTarjeta = _unidadTrabajo.ProcesadorPago.ObtenerTodosDropdownLista("TipoTarjeta");
                            model.MetodoPagoLista = _unidadTrabajo.ProcesadorPago.ObtenerTodosDropdownLista("MetodoPago");
                            model.mensajeError = "Ya existe un procesador de pago con el metodo de pago efectivo";
                            return View(model);
                        }
                    }
                    /*
                       ‐Solo puede haber un procesador activo para tarjetas de crédito o débito y otro para cheques electrónicos.
                       Si alguno estuviera activo y se desea poner otro en ese estado se debe de actualizar los demás en inactivo 
                       y el nuevo ponerlo en activo.
                       */
                    else
                    {
                        // validar si el estado es activo
                        if (model.ProcesadorPago.Estado)
                        {
                            // traer todos los procesadores de pago con el mismo metodo de pago del modelo
                            var procesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerTodos(p => p.MetodoPagoId == model.ProcesadorPago.MetodoPagoId);

                            // colocar el estado de los procesadores de pago en inactivo
                            foreach (var procesador in procesadorPago)
                            {
                                procesador.Estado = false;
                                _unidadTrabajo.ProcesadorPago.Actualizar(procesador);
                            }
                        }
                    }

                    await _unidadTrabajo.ProcesadorPago.Agregar(model.ProcesadorPago);

                }
                else
                {
                    // traer el procesador de pago de la base de datos
                    var procesadorPagoDb = await _unidadTrabajo.ProcesadorPago.Obtener(model.ProcesadorPago.Id);

                    // validar si el metodo de pago de procesadorPagoDb igual al metodo de pago del modelo
                    if (procesadorPagoDb.MetodoPagoId == model.ProcesadorPago.MetodoPagoId)
                    {

                        /*
                      Solo puede haber un procesador activo para tarjetas de crédito o débito y otro para cheques electrónicos.
                      Si alguno estuviera activo y se desea poner otro en ese estado se debe de actualizar los demás en inactivo 
                      y el nuevo ponerlo en activo.
                      */
                        if (model.ProcesadorPago.MetodoPagoId != 1)
                        {
                            // validar si el estado es activo
                            if (model.ProcesadorPago.Estado)
                            {
                                // traer todos los procesadores de pago con el mismo metodo de pago del modelo
                                var procesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerTodos(p => p.MetodoPagoId == model.ProcesadorPago.MetodoPagoId);

                                // colocar el estado de los procesadores de pago en inactivo
                                foreach (var procesador in procesadorPago)
                                {
                                    procesador.Estado = false;
                                    _unidadTrabajo.ProcesadorPago.Actualizar(procesador);
                                }
                            }
                        }

                    }
                    // caso cuando le cambio el tipo de metodo de pago del modelo con respecto al de la base de datos
                    else
                    {

                        // caso pasar de cheque/targeta a efectivo
                        if (model.ProcesadorPago.MetodoPagoId == 1)
                        {
                            // validar si ya existe un procesador de pago con el metodo de pago efectivo
                            var procesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerTodos(p => p.MetodoPagoId == 1);
                            if (procesadorPago.Count() > 0)
                            {
                                TempData[DS.Error] = "Ya existe un procesador de pago con el metodo de pago efectivo";
                                model.TipoTarjeta = _unidadTrabajo.ProcesadorPago.ObtenerTodosDropdownLista("TipoTarjeta");
                                model.MetodoPagoLista = _unidadTrabajo.ProcesadorPago.ObtenerTodosDropdownLista("MetodoPago");
                                model.mensajeError = "Ya existe un procesador de pago con el metodo de pago efectivo";
                                return View(model);
                            }
                            else
                            {
                                // validar si el procesador de pago de la base de datos es targeta 
                                if (procesadorPagoDb.MetodoPagoId == 3)
                                {
                                    // eliminar todas los ProcesadorTarjeta asociadas
                                    var procesadorTarjeta = await _unidadTrabajo.ProcesadorTarjeta.ObtenerTodos(p => p.ProcesadorPagoId == procesadorPagoDb.Id);
                                    foreach (var tarjeta in procesadorTarjeta)
                                    {
                                        _unidadTrabajo.ProcesadorTarjeta.Remover(tarjeta);
                                    }
                                }

                            }
                        }
                        else
                        {
                            // validar si el estado es activo
                            if (model.ProcesadorPago.Estado)
                            {
                                // traer todos los procesadores de pago con el mismo metodo de pago del modelo
                                var procesadorPago = await _unidadTrabajo.ProcesadorPago.ObtenerTodos(p => p.MetodoPagoId == model.ProcesadorPago.MetodoPagoId);

                                // colocar el estado de los procesadores de pago en inactivo
                                foreach (var procesador in procesadorPago)
                                {
                                    procesador.Estado = false;
                                    _unidadTrabajo.ProcesadorPago.Actualizar(procesador);
                                }
                            }

                            // pasar de targeta a cheque
                            if (model.ProcesadorPago.MetodoPagoId == 2)
                            {
                                // eliminar todas los ProcesadorTarjeta asociadas
                                var procesadorTarjeta = await _unidadTrabajo.ProcesadorTarjeta.ObtenerTodos(p => p.ProcesadorPagoId == procesadorPagoDb.Id);
                                foreach (var tarjeta in procesadorTarjeta)
                                {
                                    _unidadTrabajo.ProcesadorTarjeta.Remover(tarjeta);
                                }
                            }
                        }
                    }
                    _unidadTrabajo.ProcesadorPago.Actualizar(model.ProcesadorPago);
                }
                TempData[DS.Exitosa] = "Operación exitosa";
                await _unidadTrabajo.Guardar();
                return View("Index");
            }
            model.TipoTarjeta = _unidadTrabajo.ProcesadorPago.ObtenerTodosDropdownLista("TipoTarjeta");
            model.MetodoPagoLista = _unidadTrabajo.ProcesadorPago.ObtenerTodosDropdownLista("MetodoPago");

            return View(model);
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.ProcesadorPago.ObtenerTodos(incluirPropiedades: "MetodoPago");
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var procesadorPagoDb = await _unidadTrabajo.ProcesadorPago.Obtener(id);
            if (procesadorPagoDb == null)
            {
                return Json(new { success = false, message = "Error al borrar el Procesador de Pago" });
            }

            // Buscar todas las targetas asociadas a este procesador de pago en ProcesadorTarjeta y borrarlas
            var procesadorTarjeta = await _unidadTrabajo.ProcesadorTarjeta.ObtenerTodos(p => p.ProcesadorPagoId == id);
            foreach (var tarjeta in procesadorTarjeta)
            {
                _unidadTrabajo.ProcesadorTarjeta.Remover(tarjeta);
            }

            _unidadTrabajo.ProcesadorPago.Remover(procesadorPagoDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Cupon borrada exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.ProcesadorPago.ObtenerTodos();
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


        #endregion


    }
}
