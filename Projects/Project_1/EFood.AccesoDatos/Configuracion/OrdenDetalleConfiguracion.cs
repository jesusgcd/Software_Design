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
    public class OrdenDetalleConfiguracion : IEntityTypeConfiguration<OrdenDetalle>
    {
        public void Configure(EntityTypeBuilder<OrdenDetalle> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.OrdenId).IsRequired();
            builder.Property(x => x.ProductoId).IsRequired();
            builder.Property(x => x.TipoPrecioId).IsRequired();
            builder.Property(x => x.Cantidad).IsRequired();
            builder.Property(x => x.Monto).IsRequired();


            /* Foreign Keys*/
            builder.HasOne(x => x.Orden).WithMany()
            .HasForeignKey(x => x.OrdenId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Producto).WithMany()
                   .HasForeignKey(x => x.ProductoId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.TipoPrecio).WithMany()
                   .HasForeignKey(x => x.TipoPrecioId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}





