using SistemaInventarioV6.Modelos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace SistemaInventarioV6.AccesoDatos.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<Size> Sizes { get; set; }

        public DbSet<FoodCategory> FoodCategories { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<Error> Errors { get; set; }
        public DbSet<DiscountTicket> DiscountTickets { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<PaymentProcessor> PaymentProcessors { get; set; }

        public DbSet<ProcessorCard> ProcessorCards { get; set; }

        public DbSet<Price> Prices { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}