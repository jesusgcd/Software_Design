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
    public class ProcessorCardConfiguration : IEntityTypeConfiguration<ProcessorCard>
    {
        public void Configure(EntityTypeBuilder<ProcessorCard> builder)
        {
            builder.Property(x => x.ID).IsRequired();
            builder.Property(x => x.ProcessorId).IsRequired();
            builder.Property(x => x.CardId).IsRequired();

            builder.HasOne(x => x.PaymentProcessor).WithMany()
                .HasForeignKey(x => x.ProcessorId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Card).WithMany()
                .HasForeignKey(x => x.CardId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
