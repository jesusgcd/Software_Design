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

    public class ProcesadorTarjetaController : Controller
    {

        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProcesadorTarjetaController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> AdministrarTarjetas(int id)
        {
            var procesadorpago = await _unidadTrabajo.ProcesadorPago.Obtener(id);
            if (procesadorpago == null)
            {
                return NotFound();
            }

            var tarjetas = await _unidadTrabajo.ProcesadorTarjeta.ObtenerTarjetasPorProcesadorPago(id);

            var ProcesadorTarjetaVM = new ProcesadorTarjetaVM
            {
                ProcesadorPago = procesadorpago,
                ProcesadorTarjeta = tarjetas
            };

            return View(ProcesadorTarjetaVM);
        }


		public async Task<IActionResult> Upsert(int id)
        {
            NuevaTargetaVM nuevaTargetaVM = new NuevaTargetaVM()
            {
                procesadorPagoId = id,
                tiposTarjeta = _unidadTrabajo.TipoTarjeta.ObtenerTodosDropdownLista("TipoTarjeta")
            };  

            return View(nuevaTargetaVM);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Upsert(NuevaTargetaVM modelo)
        {
            if (ModelState.IsValid)
            {
			    // Verificar si ya existe el procesador de pago con esa targeta
                List<ProcesadorTarjeta> targetasXprocesador = await _unidadTrabajo.ProcesadorTarjeta.ObtenerTarjetasPorProcesadorPago(modelo.procesadorPagoId);

				bool existeTargetasXprocesador = false;

				// Verificar si ya existe un precio para este producto y tipo de precio
				foreach (var targeta in targetasXprocesador)
				{
					if (targeta.TipoTarjetaId == modelo.tipoTarjetaId)
					{
						existeTargetasXprocesador = true;
						break;
					}
				}

                if (existeTargetasXprocesador)
                {
					modelo.mensajeError = "Este Pocesador ya tiene asignada esta targeta";
					modelo.tiposTarjeta = _unidadTrabajo.TipoTarjeta.ObtenerTodosDropdownLista("TipoTarjeta");
					return View(modelo);
				}

                // crear el nuevo ProcesadorTarjeta
                ProcesadorTarjeta procesadorTarjeta = new ProcesadorTarjeta
                {
					ProcesadorPagoId = modelo.procesadorPagoId,
					TipoTarjetaId = modelo.tipoTarjetaId
				};

				await _unidadTrabajo.ProcesadorTarjeta.Agregar(procesadorTarjeta);
				await _unidadTrabajo.Guardar();

				TempData["Exitoso"] = "Nuevo precio creado correctamente";

				return RedirectToAction("AdministrarTarjetas", new { id = modelo.procesadorPagoId });
			}

			// Si hay errores de validación, recargar la vista con el modelo
			modelo.tiposTarjeta = _unidadTrabajo.TipoTarjeta.ObtenerTodosDropdownLista("TipoTarjeta");
			return View(modelo);
        }


		#region API



		public async Task<IActionResult> EliminarTarjeta(int id)
		{
			var procesadorTagetaDb = await _unidadTrabajo.ProcesadorTarjeta.Obtener(id);
			if (procesadorTagetaDb == null)
			{
				return NotFound();
			}

			try
			{
				_unidadTrabajo.ProcesadorTarjeta.Remover(procesadorTagetaDb);
				await _unidadTrabajo.Guardar();

				TempData["Exitoso"] = "Targeta eliminada correctamente";
				return RedirectToAction("AdministrarTarjetas", new { id = procesadorTagetaDb.ProcesadorPagoId });
			}
			catch (Exception ex)
			{
				// Manejar cualquier error
				TempData["Error"] = $"Error al eliminar la Targeta: {ex.Message}";
				return RedirectToAction("AdministrarTarjetas", new { id = procesadorTagetaDb.ProcesadorPagoId });
			}
		}



		#endregion

	}
}
