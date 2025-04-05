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
    public class LineaComidaConfiguracion : IEntityTypeConfiguration<LineaComida>
    {
        public void Configure(EntityTypeBuilder<LineaComida> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(50);
        }
    }
}
