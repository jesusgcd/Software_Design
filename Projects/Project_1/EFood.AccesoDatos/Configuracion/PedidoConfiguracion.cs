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
    public class PedidoConfiguracion : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Cantidad).IsRequired();
            builder.Property(x => x.PrecioProductoID).IsRequired();
            builder.Property(x => x.CarritoID).IsRequired();

            /* Foreign Keys*/

            builder.HasOne(x => x.PrecioProducto).WithMany()
                   .HasForeignKey(x => x.PrecioProductoID)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Carrito).WithMany()
                   .HasForeignKey(x => x.CarritoID)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
