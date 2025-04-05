using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EFood.AccesoDatos.Data;
using EFood.Modelos;
using EFood.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Inicializador
{
    public class DbInicializador : IDbInicializador
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInicializador(ApplicationDbContext db, UserManager<IdentityUser> userManager,  
                                              RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Inicializar()
        {
            try
            {
                if(_db.Database.GetPendingMigrations().Count() >0)
                {
                    _db.Database.Migrate();  // Ejecuta las migraciones pendientes
                }
            }
            catch (Exception)
            {

                throw;
            }
            Usuario usuario = _db.Usuario.Where(u => u.UserName == "administrador@administrador.com").FirstOrDefault();
            if (usuario != null)
            {
                return;
            }

            // Datos Iniciales
            //if (_db.Roles.Any(r => r.Name == DS.Role_Admin)) return;

            _roleManager.CreateAsync(new IdentityRole(DS.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(DS.Role_Mantenimiento)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(DS.Role_Seguridad)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(DS.Role_Mantenimiento)).GetAwaiter().GetResult();


            // Crea un usuario administrador si no existe uno
            _userManager.CreateAsync(new Usuario
            {
                UserName = "administrador@administrador.com",
                Email = "administrador@administrador.com",
                EmailConfirmed = true,
                Nombre= "Carlos",
                Apellidos = "Piedra",
                PreguntaSeguridad = "Soy admin?",
                RespuestaSeguridad = "Si",

            }, "Admin123*").GetAwaiter().GetResult();

            // Asigna el Rol al usuario
            usuario = _db.Usuario.Where(u => u.UserName == "administrador@administrador.com").FirstOrDefault();
            _userManager.AddToRoleAsync(usuario, DS.Role_Admin).GetAwaiter().GetResult();
        }
    }
}
