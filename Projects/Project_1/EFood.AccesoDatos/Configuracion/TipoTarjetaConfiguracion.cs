using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFood.Modelos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EFood.AccesoDatos.Configuracion
{
        public class TipoTarjetaConfiguracion : IEntityTypeConfiguration<TipoTarjeta>
        {
            public void Configure(EntityTypeBuilder<TipoTarjeta> builder)
            {
                builder.Property(x => x.Id).IsRequired();
                builder.Property(x => x.Nombre).IsRequired().HasMaxLength(50);
            }
        }
    }
