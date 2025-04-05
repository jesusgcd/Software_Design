using SistemaInventarioV6.AccesoDatos.Data;
using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Modelos;
using SistemaInventarioV6.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace SistemaInventarioV6.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    public class PaymentProcessorController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public PaymentProcessorController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Upsert(int? id)
        {
            PaymentProcessor paymentProcessor = new PaymentProcessor();

            if (id == null)
            {
                // Crear el nuevo procesador
                paymentProcessor.Status = false;
                paymentProcessor.Verification = false;
                return View(paymentProcessor);
            }
            // Actualizar el procesador
            paymentProcessor = await _unidadTrabajo.PaymentProcessor.Obtener(id.GetValueOrDefault());
            if (paymentProcessor == null)
            {
                return NotFound();
            }
            return View(paymentProcessor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(PaymentProcessor paymentProcessor)
        {
            if (ModelState.IsValid)
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                if (paymentProcessor.ID == 0)
                {
                    await _unidadTrabajo.PaymentProcessor.Agregar(paymentProcessor);
                    _unidadTrabajo.PaymentProcessor.RegistrarBitacora(currentUserID, "Crear nuevo procesador de pago");
                    TempData[DS.Exitosa] = "Procesador creado exitosamente";
                }
                else
                {
                    _unidadTrabajo.PaymentProcessor.Actualizar(paymentProcessor);
                    _unidadTrabajo.PaymentProcessor.RegistrarBitacora(currentUserID, "Actualizar procesador de pago");
                    TempData[DS.Exitosa] = "Procesador actualizado exitosamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            _unidadTrabajo.PaymentProcessor.RegistrarBitacoraErrores("errorProcesador", "Error al grabar procesador de pago");
            TempData[DS.Error] = "Error al grabar procesador";
            return View(paymentProcessor);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.PaymentProcessor.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            var paymentProcessorDB = await _unidadTrabajo.PaymentProcessor.Obtener(id);
            if (paymentProcessorDB == null)
            {
                _unidadTrabajo.PaymentProcessor.RegistrarBitacoraErrores("errorProcesador", "Error al eliminar procesador de pago");
                return Json(new { success = false, message = "Error al borrar procesador" });
            }
            _unidadTrabajo.PaymentProcessor.Remover(paymentProcessorDB);
            _unidadTrabajo.PaymentProcessor.RegistrarBitacora(currentUserID, "Eliminar procesador de pago");
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Procesador borrado exitosamente" });
        }

        //Validar que el código, que no es PK, sea único
        [ActionName("ValidarCodigo")]
        public async Task<IActionResult> ValidarCodigo(string code, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.PaymentProcessor.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(reg => reg.Code.ToLower().Trim() == code.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(reg => reg.Code.ToLower().Trim() == code.ToLower().Trim() && reg.ID != id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { data = false });

        }

        //Validar que solo haya un procesador de tipo efectivo
        [ActionName("ValidarTipoEfectivo")]
        public async Task<IActionResult> ValidarTipoEfectivo(string type, int id = 0)
        {
            bool esEfectivo = type == "Efectivo";
            bool valor = false;
            var lista = await _unidadTrabajo.PaymentProcessor.ObtenerTodos();
            if (esEfectivo)
            {
                if (id == 0)
                {
                    valor = lista.Any(reg => reg.Type == "Efectivo");
                }
                else
                {
                    valor = lista.Any(reg => reg.Type == "Efectivo" && reg.ID != id);
                }
            }
            return Json(new { data = valor });

        }

        //Validación para asegurar que solo haya un procesador de tarjeta y de cheque activos
        [ActionName("ValidarTiposActivos")]
        public async Task<IActionResult> ValidarTiposActivos(bool status, string type, int id = 0)
        {
            bool debeValidar = (type == "Tarjeta" || type == "Cheque");
            bool valor = false;
            var lista = await _unidadTrabajo.PaymentProcessor.ObtenerTodos();
            if (debeValidar && status)
            {
                if (id == 0)
                {
                    valor = lista.Any(reg => reg.Type == type && reg.Status);
                }
                else
                {
                    valor = lista.Any(reg => reg.Type == type && reg.ID != id && reg.Status);
                }
            }
            return Json(new { data = valor });

        }
        #endregion

    }
}
