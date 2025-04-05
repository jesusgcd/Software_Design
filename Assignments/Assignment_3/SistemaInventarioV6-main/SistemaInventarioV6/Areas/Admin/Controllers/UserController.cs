using SistemaInventarioV6.AccesoDatos.Data;
using SistemaInventarioV6.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventarioV6.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace SistemaInventarioV6.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Seguridad)]
    public class UserController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ApplicationDbContext _db;

        public UserController(IUnidadTrabajo unidadTrabajo, ApplicationDbContext db)
        {
            _unidadTrabajo = unidadTrabajo;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var userList = await _unidadTrabajo.AppUser.ObtenerTodos();
            var userRole = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();

            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            }
            return Json(new { data = userList });
        }

        [HttpPost]
        public async Task<IActionResult> BloquearDesbloquear([FromBody] string id)
        {
            var user = await _unidadTrabajo.AppUser.ObtenerPrimero(u => u.Id == id);
            if (user == null)
            {
                return Json(new { success = false, message = "Error de Usuario" });
            }
            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                // Usuario se encuentra bloqueado, se desea desbloquear
                user.LockoutEnd = DateTime.Now;
            }
            else
            {
                // Bloquear al usuario
                user.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Operación Exitosa" });

        }

        #endregion
    }
}
