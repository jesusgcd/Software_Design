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
    public class BitacoraConsultaController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BitacoraConsultaController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var bitacoras = _unidadTrabajo.Bitacora.ObtenerBitacoras();
            var bitacoraConsultaVM = CrearBitacoraConsultaVM(bitacoras, "No existen Bitacoras en el Sistema");
            return View(bitacoraConsultaVM);
        }

        [HttpPost]
        public IActionResult FiltrarBitacoraFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio >= fechaFin)
            {
                var bitacoraConsultaVM = CrearBitacoraConsultaVM(null, "La fecha de inicio no puede ser mayor o igual a la fecha fin");
                return View("Index", bitacoraConsultaVM);
            }

            var bitacoras = _unidadTrabajo.Bitacora.ObtenerBitacoras();
            var bitacorasFiltradas = FiltrarBitacorasPorFecha(bitacoras, fechaInicio, fechaFin);
            var bitacoraConsultaVMFiltrada = CrearBitacoraConsultaVM(bitacorasFiltradas, "No existen Bitacoras en el rango de tiempo seleccionado");

            return View("Index", bitacoraConsultaVMFiltrada);
        }

        private BitacoraConsultaVM CrearBitacoraConsultaVM(IEnumerable<Bitacora> bitacoras, string mensajeError)
        {
            var listaBitacoras = ProcesarBitacoras(bitacoras);
            return new BitacoraConsultaVM
            {
                ListaBitacoras = listaBitacoras.Any() ? listaBitacoras : null,
                MensajeError = listaBitacoras.Any() ? null : mensajeError
            };
        }

        private List<Array> ProcesarBitacoras(IEnumerable<Bitacora> bitacoras)
        {
            var listaBitacoras = new List<Array>();

            if (bitacoras != null)
            {
                foreach (var bitacora in bitacoras)
                {
                    listaBitacoras.Add(new object[] { bitacora.Id, bitacora.UsuarioId, bitacora.FechaHora, bitacora.Descripcion });
                }
            }

            return listaBitacoras;
        }

        private IEnumerable<Bitacora> FiltrarBitacorasPorFecha(IEnumerable<Bitacora> bitacoras, DateTime fechaInicio, DateTime fechaFin)
        {
            return bitacoras.Where(b => b.FechaHora >= fechaInicio && b.FechaHora <= fechaFin);
        }

        #region API

        #endregion
    }
}