using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EFood.Modelos;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;

namespace EFood.AccesoDatos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<ProductoSI> ProductoSIs { get; set; }
        public DbSet<Cupon> Cupones { get; set; }
        public DbSet<Error> Errores { get; set; }
        public DbSet<Bitacora> Bitacora { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<LineaComida> LineaComida { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Precio> Precio { get; set; }
        public DbSet<TipoPrecio> TipoPrecio { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<Carrito> Carrito { get; set; }
        public DbSet<MetodoPago> MetodoPago { get; set; }
        public DbSet<TipoTarjeta> TipoTarjeta { get; set; }
        public DbSet<ProcesadorPago> ProcesadorPago { get; set; }
        public DbSet<ProcesadorTarjeta> ProcesadorTarjeta { get; set; }
        public DbSet<PasarelaPago> PasarelaPago { get; set; }
        public DbSet<CarroCompra> CarroCompra { get; set; }
        public DbSet<Orden> Orden { get; set; }        
        public DbSet<OrdenDetalle> OrdenDetalle { get; set; }


		protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			// Agregar las entradas a la tabla "MetodoPago"
			builder.Entity<MetodoPago>().HasData(
		        new MetodoPago { Id = 1, Nombre = "Efectivo" },
		        new MetodoPago { Id = 2, Nombre = "Cheque" },
		        new MetodoPago { Id = 3, Nombre = "Tarjeta" }
			);

		}
	}
}