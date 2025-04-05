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
    public class TipoPrecioConfiguracion : IEntityTypeConfiguration<TipoPrecio>
    {
        public void Configure(EntityTypeBuilder<TipoPrecio> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(50);
        }
    }
}
