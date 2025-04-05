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
    public class PriceConfiguration : IEntityTypeConfiguration<Price>
    {
        public void Configure(EntityTypeBuilder<Price> builder)
        {
            builder.Property(x => x.ID).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.SizeId).IsRequired();
            builder.Property(x => x.Cost).IsRequired();

            builder.HasOne(x => x.Product).WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Size).WithMany()
                .HasForeignKey(x => x.SizeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
