using Microsoft.AspNetCore.Mvc;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Modelos;
using EFood.Utilidades;
using EFood.Modelos.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace EFood.Areas.Consultas.Controllers
{
    [Area("Consultas")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Consulta)]

    public class ErrorConsultaController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ErrorConsultaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            var errores = _unidadTrabajo.Error.ObtenerErrores();
            var errorConsultaVM = CrearErrorConsultaVM(errores, "No existen Errores en el Sistema");
            return View(errorConsultaVM);
        }


        [HttpPost]
        public IActionResult FiltrarErrorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio >= fechaFin)
            {
                var errorConsultaVM = CrearErrorConsultaVM(null, "La fecha de inicio no puede ser mayor o igual a la fecha fin");
                return View("Index", errorConsultaVM);
            }

            var errores = _unidadTrabajo.Error.ObtenerErrores();
            var erroresFiltradas = FiltrarErroresPorFecha(errores, fechaInicio, fechaFin);
            var erroresConsultaVMFiltrada = CrearErrorConsultaVM(erroresFiltradas, "No existen Errores en el rango de tiempo seleccionado");

            return View("Index", erroresConsultaVMFiltrada);
        }
        private ErrorConsultaVM CrearErrorConsultaVM(IEnumerable<Error> errores, string mensajeError)
        {
            var listaErrores = ProcesarErrores(errores);
            return new ErrorConsultaVM
            {
                ListaErrores = listaErrores.Any() ? listaErrores : null,
                MensajeError = listaErrores.Any() ? null : mensajeError
            };
        }

        private List<Array> ProcesarErrores(IEnumerable<Error> errores)
        {
            var listaErrores = new List<Array>();

            if (errores != null)
            {
                foreach (var error in errores)
                {
                    listaErrores.Add(new object[] { error.Codigo, error.FechaHora, error.Descripcion });
                }
            }

            return listaErrores;
        }

        private IEnumerable<Error> FiltrarErroresPorFecha(IEnumerable<Error> errores, DateTime fechaInicio, DateTime fechaFin)
        {
            return errores.Where(b => b.FechaHora >= fechaInicio && b.FechaHora <= fechaFin);
        }

        /* ESTO DE EMERGENCY
        public IActionResult Index()
        {
            // crear el ErrorConsultaVM 
            ErrorConsultaVM errorConsultaVM = new ErrorConsultaVM()
            {
                ListaErrores = null,
                MensajeError = "No existen Errores en el Sistema"
            };


            // crear una lista de error
            List<Array> listaErrores = new List<Array>();

            // obtener todas los Errores
            IEnumerable<Error> errores = (IEnumerable<Error>)_unidadTrabajo.Error.ObtenerErrores();

            // validar que existan errores
            if (errores.Count() == 0)
            {
                return View(errorConsultaVM);
            }
            else
            {
                // recorrer la lista de errores y agregarlas a la lista de errores
                foreach (var error in errores)
                {
                    listaErrores.Add(new object[] { error.Codigo, error.FechaHora, error.Descripcion });
                }

                // agregar la lista de error al errorConsultaVM
                errorConsultaVM.ListaErrores = listaErrores;
            }
            return View(errorConsultaVM);
        }
        */

    }
}
