
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
    public class OrdenConfiguracion : IEntityTypeConfiguration<Orden>
    {
        public void Configure(EntityTypeBuilder<Orden> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.NombreCliente).IsRequired().HasMaxLength(50);
            builder.Property(x => x.ApellidosCliente).IsRequired().HasMaxLength(50);
            builder.Property(x => x.TelefonoCliente).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Direccion).IsRequired().HasMaxLength(500);
            builder.Property(x => x.Estado).IsRequired().HasMaxLength(15);
            builder.Property(x => x.CodigoCupon).HasMaxLength(50);
            builder.Property(x => x.MontoTotal).IsRequired();
            builder.Property(x => x.ProcesadorPagoId).IsRequired();
            builder.Property(x => x.FechaHora).IsRequired();


            /* Foreign Keys*/

            builder.HasOne(x => x.ProcesadorPago).WithMany()
                   .HasForeignKey(x => x.ProcesadorPagoId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

