using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EFood.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFood.AccesoDatos.Configuracion
{
    public class ProductoConfiguracion : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.LineaComidaId).IsRequired();
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(500);
            builder.Property(x => x.ImagenUrl).IsRequired(false);

            /* Foreign Keys*/

            builder.HasOne(x => x.LineaComida).WithMany()
                   .HasForeignKey(x => x.LineaComidaId)
                   .OnDelete(DeleteBehavior.NoAction);


        }
    }
}





