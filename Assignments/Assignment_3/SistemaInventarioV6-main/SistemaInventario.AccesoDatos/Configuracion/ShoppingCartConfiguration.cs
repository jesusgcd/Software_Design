using SistemaInventarioV6.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.AccesoDatos.Configuracion
{
    public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.Property(x => x.ID).IsRequired();
            builder.Property(x => x.AppUserId).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.PriceId).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();

            /* Relaciones*/
            builder.HasOne(x => x.AppUser).WithMany()
                   .HasForeignKey(x => x.AppUserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Product).WithMany()
                  .HasForeignKey(x => x.ProductId)
                  .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Price).WithMany()
                  .HasForeignKey(x => x.PriceId)
                  .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
