using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using System;
using EFood.Modelos;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace EFood.Areas.Administracion.Controllers
{
    [Area("Administracion")]
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Seguridad)]
    public class UsuarioController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public UsuarioController(IUnidadTrabajo unidadTrabajo, ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _unidadTrabajo = unidadTrabajo;
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        /*
        public IActionResult Index()
        {
            return View();
        }
        */
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Edit(string id, string rol)
        {
            var usuario = await _unidadTrabajo.Usuario.ObtenerPrimero(u => u.Id == id);
            var currentRole = await _userManager.GetRolesAsync(usuario);
            if (!currentRole.IsNullOrEmpty())
            {
                usuario.Role = currentRole[0];

            }
            return View(usuario);
        }

        #region API
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(IdentityUser identityUser, string rolNuevo)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _unidadTrabajo.Usuario.ObtenerPrimero(u => u.Id == identityUser.Id);
                if (usuario == null)
                {
                    TempData[DS.Error] = "Error al obtener el usario";
                    return View(usuario);
                }
                var currentRole = await _userManager.GetRolesAsync(usuario);
                if (currentRole.IsNullOrEmpty())
                {
                    TempData[DS.Error] = "Error al obtener el rol del usuario";
                    return View(usuario);
                }
                usuario.Role = currentRole[0];
                if (rolNuevo == null)
                {
                    TempData[DS.Error] = "Error al obtener el rol seleccionado";
                    return View(usuario);
                }
                
                await _userManager.RemoveFromRoleAsync(usuario, currentRole[0]);
                await _userManager.AddToRoleAsync(usuario, rolNuevo);
                TempData[DS.Exitosa] = "Rol del usuario actualizado exitosamente";
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Modelo invalido somehow";
            return View(identityUser);
        }



        [HttpPost]
        public async Task<IActionResult> CheckEdit([FromBody] string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Error de Usuario + " + id });
            }
            var currentRole = await _userManager.GetRolesAsync(usuario);
            if (currentRole.Contains(DS.Role_Admin) && User.IsInRole(DS.Role_Seguridad))
            {
                return Json(new { success = false, message = "No puedes editar a un Admin" });
            }

            return Json(new { success = true, message = "Puede editar el usuario." });
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarRol(string id, string nuevoRol)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Usuario no encontrado." });
            }

            var rolesUsuario = await _userManager.GetRolesAsync(usuario);
            var resultado = await _userManager.RemoveFromRolesAsync(usuario, rolesUsuario);
            if (!resultado.Succeeded)
            {
                return Json(new { success = false, message = "Error al actualizar el rol del usuario." });
            }

            resultado = await _userManager.AddToRoleAsync(usuario, nuevoRol);
            if (resultado.Succeeded)
            {
                return Json(new { success = true, message = "Rol actualizado correctamente." });
            }
            else
            {
                // Si falla la asignación del nuevo rol, es posible que necesites revertir los cambios aquí
                return Json(new { success = false, message = "Error al actualizar el rol del usuario." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var usuarioLista = await _unidadTrabajo.Usuario.ObtenerTodos();
            var userRole = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();

            foreach (var usuario in usuarioLista)
            {
                var UserRoleEntry = userRole.FirstOrDefault(u => u.UserId == usuario.Id);
                if (UserRoleEntry != null)
                {
                    var roleId = UserRoleEntry.RoleId;
                    usuario.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                }
                else
                {
                    usuario.Role = "RolInvalido";
                }
            }
            return Json(new { data = usuarioLista });
        }

        [HttpPost]
        public async Task<IActionResult> BloquearDesbloquear([FromBody] string id)
        {
            var usuario = await _unidadTrabajo.Usuario.ObtenerPrimero(u => u.Id == id);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Error de Usuario" });
            }

            var currentRole = await _userManager.GetRolesAsync(usuario);
            if (currentRole.Contains(DS.Role_Admin) && User.IsInRole(DS.Role_Seguridad))
            {
                return Json(new { success = false, message = "No puedes bloquear a un Admin" });
            }

            if (usuario.LockoutEnd != null && usuario.LockoutEnd > DateTime.Now)
            {
                // Usuario Bloqueado
                usuario.LockoutEnd = DateTime.Now;
            }
            else
            {
                usuario.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Operación Exitosa" });
        }

        #endregion
    }
}