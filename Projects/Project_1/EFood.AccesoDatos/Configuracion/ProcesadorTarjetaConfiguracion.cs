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
    public class ProcesadorTarjetaConfiguracion : IEntityTypeConfiguration<ProcesadorTarjeta>
    {
        public void Configure(EntityTypeBuilder<ProcesadorTarjeta> builder)
        {

            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.ProcesadorPagoId).IsRequired();
            builder.Property(x => x.TipoTarjetaId).IsRequired();

            /* Foreign Keys*/
            // Cada ProcesadorTarjeta tiene un ProcesadorPago
            builder.HasOne(x => x.ProcesadorPago).WithMany()
                   .HasForeignKey(x => x.ProcesadorPagoId)
                   .OnDelete(DeleteBehavior.NoAction);
            // Cada ProcesadorTarjeta tiene un TipoTarjeta

            builder.HasOne(x => x.TipoTarjeta).WithMany()
                   .HasForeignKey(x => x.TipoTarjetaId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}





