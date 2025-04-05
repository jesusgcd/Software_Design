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
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Mantenimiento)]
    public class ProcessorCardController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public ProcessorCardController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Insert()
        {
            ProcessorCardVM processorCardVM = new ProcessorCardVM
            {
                ProcessorCard = new ProcessorCard(),
                ProcessorList = _unidadTrabajo.ProcessorCard.ObtenerTodosDropDownList("PaymentProcessor"),
                CardList = _unidadTrabajo.ProcessorCard.ObtenerTodosDropDownList("Card")
            };
            return View(processorCardVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Insert(ProcessorCardVM processorCardVM)
        {
            ModelState.Clear(); //Limpiar el ModelState porque si no, siempre lo considera inválido
            if (ModelState.IsValid)
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                await _unidadTrabajo.ProcessorCard.Agregar(processorCardVM.ProcessorCard);
                _unidadTrabajo.ProcessorCard.RegistrarBitacora(currentUserID, "Crear nuevo procesador tarjeta");
                TempData[DS.Exitosa] = "Tarjeta asignada exitosamente";
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            processorCardVM.ProcessorList = _unidadTrabajo.ProcessorCard.ObtenerTodosDropDownList("PaymentProcessor");
            processorCardVM.CardList = _unidadTrabajo.ProcessorCard.ObtenerTodosDropDownList("Card");
            _unidadTrabajo.ProcessorCard.RegistrarBitacoraErrores("errorProcesadorTarjeta", "Error al grabar procesador tarjeta");
            return View(processorCardVM);
        }


        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.ProcessorCard.ObtenerTodos(incluirPropiedades:"PaymentProcessor,Card");
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var processorCardDB = await _unidadTrabajo.ProcessorCard.Obtener(id);
            if (processorCardDB == null)
            {
                _unidadTrabajo.ProcessorCard.RegistrarBitacoraErrores("errorProcesadorTarjeta", "Error al eliminar procesador tarjeta");
                return Json(new { success = false, message = "Error al desasignar tarjeta" });
            }
            _unidadTrabajo.ProcessorCard.Remover(processorCardDB);
            _unidadTrabajo.ProcessorCard.RegistrarBitacora(currentUserID, "Eliminar procesador tarjeta");
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Tarjeta desasignada exitosamente" });
        }

        [ActionName("ValidarRepetidos")]
        public async Task<IActionResult> ValidarRepetidos(int processorId, int cardId)
        {
            var lista = await _unidadTrabajo.ProcessorCard.ObtenerTodos();
            bool valor = lista.Any(reg => reg.ProcessorId == processorId && reg.CardId == cardId);
            return Json(new { data = valor });

        }
        #endregion

    }
}
