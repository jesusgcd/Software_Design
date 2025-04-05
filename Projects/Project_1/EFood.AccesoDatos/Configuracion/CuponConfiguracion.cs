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
    public class CuponConfiguracion : IEntityTypeConfiguration<Cupon>
    {
        public void Configure(EntityTypeBuilder<Cupon> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x =>x.Codigo).IsRequired().HasMaxLength(50);
            builder.Property(x =>x.Descripcion).IsRequired().HasMaxLength(500);
            //No se pone el range aqui, eso se valida antes de ingresar los valores a la base de datos
            builder.Property(x => x.CantidadDisponible).IsRequired();
            builder.Property(x => x.Descuento).IsRequired();
        }
    }
}
