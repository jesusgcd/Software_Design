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
    public class BitacoraConfiguracion : IEntityTypeConfiguration<Bitacora>
    {
        public void Configure(EntityTypeBuilder<Bitacora> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.UsuarioId).IsRequired();
            builder.Property(x => x.FechaHora).IsRequired();
            builder.Property(x =>x.Descripcion).IsRequired().HasMaxLength(500);


            /* Foreign Keys*/

            builder.HasOne(x => x.Usuario).WithMany()
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

       
}
