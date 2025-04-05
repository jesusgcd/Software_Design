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
    public class PrecioConfiguracion : IEntityTypeConfiguration<Precio>
    {
        public void Configure(EntityTypeBuilder<Precio> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Monto).IsRequired();
            builder.Property(x => x.ProductoID).IsRequired();
            builder.Property(x => x.TipoPrecioID).IsRequired();


            /* Foreign Keys*/

            builder.HasOne(x => x.Producto).WithMany()
                   .HasForeignKey(x => x.ProductoID)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.TipoPrecio).WithMany()
                   .HasForeignKey(x => x.TipoPrecioID)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}





