using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.Utilidades;
using Microsoft.AspNetCore.Identity.UI.Services;
using EFood.AccesoDatos.Inicializador;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*
 Aqui se configura la cadena de conexion a la base de datos

     [BASE EN AZUSE] -> Desarrollo

     [Base de datos local] -> Local 

 */

//String de Base de datos  
var connectionString = builder.Configuration.GetConnectionString("Local") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddErrorDescriber<ErrorDescriber>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//Si el usuario no esta registrado, redireccionarlo a estos
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddScoped<IUnidadTrabajo, UnidadTrabajo>();

builder.Services.AddRazorPages();

builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IDbInicializador, DbInicializador>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

//Siempre autenticacion antes de autorizacion
app.UseAuthentication();
app.UseAuthorization();

// Aplicar Migraciones y Datos Iniciales
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();

    try
    {
        var inicializador = services.GetRequiredService<IDbInicializador>();
        inicializador.Inicializar();
    }
    catch (Exception ex)
    {

        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Un Error ocurrio al ejecutar la migracion");
    }
}

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Inventario}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


app.Run();

