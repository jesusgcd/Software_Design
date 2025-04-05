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
    public class LogConfiguration : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.Property(x => x.ID).IsRequired();
            builder.Property(x => x.UserID).IsRequired();
            builder.Property(x => x.TimeStamp).IsRequired();
            builder.Property(x => x.Description).IsRequired();

            builder.HasOne(x => x.AppUser).WithMany()
               .HasForeignKey(x => x.UserID)
               .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
