using SistemaInventarioV6.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.AccesoDatos.Configuracion
{
    public class ErrorConfiguration : IEntityTypeConfiguration<Error>
    {
        public void Configure(EntityTypeBuilder<Error> builder)
        {
            builder.Property(x => x.ID).IsRequired();
            builder.Property(x => x.Code).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(150);
        }
    }
}
