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
    public class PasarelaPagoConfiguracion : IEntityTypeConfiguration<PasarelaPago>
    {
        public void Configure(EntityTypeBuilder<PasarelaPago> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(500);
            builder.Property(x => x.ApiKey).IsRequired().HasMaxLength(100);
        }
    }
}