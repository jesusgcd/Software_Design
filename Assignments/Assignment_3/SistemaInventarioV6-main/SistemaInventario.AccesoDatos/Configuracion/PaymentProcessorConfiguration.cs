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
    public class PaymentProcessorConfiguration : IEntityTypeConfiguration<PaymentProcessor>
    {
        public void Configure(EntityTypeBuilder<PaymentProcessor> builder)
        {
            builder.Property(x => x.ID).IsRequired();
            builder.Property(x => x.Code).IsRequired().HasMaxLength(15);
            builder.Property(x => x.Processor).IsRequired().HasMaxLength(30);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Type).IsRequired().HasMaxLength(25);
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Verification).IsRequired();
            builder.Property(x => x.Method).IsRequired().HasMaxLength(50);
        }
    }
}
