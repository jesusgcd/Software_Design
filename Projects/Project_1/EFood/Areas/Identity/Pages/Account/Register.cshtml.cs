// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using EFood.Modelos;
using EFood.Utilidades;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace EFood.Areas.Identity.Pages.Account
{
    [Authorize(Roles = DS.Role_Admin + "," + DS.Role_Seguridad)]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string PhoneNumber { get; set; }

            [Required]
            public string Nombre { get; set; }

            [Required]
            public string Apellidos { get; set; }

            [Required]
            public string PreguntaSeguridad { get; set; }

            [Required]
            public string RespuestaSeguridad { get; set; }

            public string Role { get; set; }

            public IEnumerable<SelectListItem> ListaRol { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            var roles = _roleManager.Roles.Select(r => r.Name);

            if (User.IsInRole(DS.Role_Seguridad))
            {
                //Si es seguridad, quitarle la opcion de dar rol admin
                roles = roles.Where(r => r != DS.Role_Admin);
            }

            Input = new InputModel
            {
                ListaRol = roles.Select(r => new SelectListItem
                {
                    Text = r,
                    Value = r
                }).ToList()
            };

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/Administracion/Home");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var user = new Usuario
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    PhoneNumber = Input.PhoneNumber,
                    Nombre = Input.Nombre,
                    Apellidos = Input.Apellidos,
                    PreguntaSeguridad = Input.PreguntaSeguridad,
                    RespuestaSeguridad = Input.RespuestaSeguridad,
                    Role = Input.Role,
                };

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Crear los roles del sistema si no existen
                    await EnsureRolesExist();

                    if (user.Role == null)
                    {
                        // Que el default sea Role_Consulta
                        await _userManager.AddToRoleAsync(user, DS.Role_Consulta);
                    }
                    else
                    {
                        // Cuando alguien de seguridad o admin crea el usuario, puede escoger su rol, y aquí lo asigna
                        await _userManager.AddToRoleAsync(user, user.Role);
                    }

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        if (user.Role == null)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                        else
                        {
                            // Administrador o seguridad está registrando un nuevo Usuario
                            return RedirectToAction("Index", "Usuario", new { Area = "Administracion" });
                        }
                    }
                }

                Input.ListaRol = GetRolesForUser().Select(r => new SelectListItem
                {
                    Text = r,
                    Value = r
                }).ToList();

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }

        private async Task EnsureRolesExist()
        {
            if (!await _roleManager.RoleExistsAsync(DS.Role_Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(DS.Role_Admin));
            }
            if (!await _roleManager.RoleExistsAsync(DS.Role_Consulta))
            {
                await _roleManager.CreateAsync(new IdentityRole(DS.Role_Consulta));
            }
            if (!await _roleManager.RoleExistsAsync(DS.Role_Mantenimiento))
            {
                await _roleManager.CreateAsync(new IdentityRole(DS.Role_Mantenimiento));
            }
            if (!await _roleManager.RoleExistsAsync(DS.Role_Seguridad))
            {
                await _roleManager.CreateAsync(new IdentityRole(DS.Role_Seguridad));
            }
        }

        private IEnumerable<string> GetRolesForUser()
        {
            var roles = _roleManager.Roles.Select(r => r.Name);

            if (User.IsInRole(DS.Role_Seguridad))
            {
                roles = roles.Where(r => r != DS.Role_Admin);
            }

            return roles;
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
