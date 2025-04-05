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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.ID).IsRequired();
            builder.Property(x => x.AppUserId).IsRequired();
            builder.Property(x => x.PaymentProcessorId).IsRequired();
            builder.Property(x => x.Subtotal).IsRequired();
            builder.Property(x => x.Discount).IsRequired();
            builder.Property(x => x.Total).IsRequired();
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.ClientName).IsRequired().HasMaxLength(30);
            builder.Property(x => x.ClientSurname).IsRequired().HasMaxLength(50);
            builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(40);
            builder.Property(x => x.Address).IsRequired().HasMaxLength(200);
            builder.Property(x => x.DiscountCode).IsRequired(false);

            /* Relaciones*/
            builder.HasOne(x => x.AppUser).WithMany()
                   .HasForeignKey(x => x.AppUserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.PaymentProcessor).WithMany()
                  .HasForeignKey(x => x.PaymentProcessorId)
                  .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
