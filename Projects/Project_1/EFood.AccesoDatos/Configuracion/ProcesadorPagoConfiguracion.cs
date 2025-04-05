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
    public class ProcesadorPagoConfiguracion : IEntityTypeConfiguration<ProcesadorPago>
    {
        public void Configure(EntityTypeBuilder<ProcesadorPago> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            
            builder.Property(x => x.MetodoPagoId).IsRequired(); 

            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(50);
			builder.Property(x => x.PasarelaPago).IsRequired();
			builder.Property(x => x.RequiereVerificacion).IsRequired();
            builder.Property(x => x.MetodoVerificacion).IsRequired().HasMaxLength(100); 
            builder.Property(x => x.Estado).IsRequired(); 


            /* Foreign Keys*/

            builder.HasOne(x => x.MetodoPago).WithMany()
                   .HasForeignKey(x => x.MetodoPagoId)
                   .OnDelete(DeleteBehavior.NoAction);

        }
    }
}





