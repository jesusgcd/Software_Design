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
    public class DiscountTicketConfiguration : IEntityTypeConfiguration<DiscountTicket>
    {
        public void Configure(EntityTypeBuilder<DiscountTicket> builder)
        {
            builder.Property(x => x.ID).IsRequired();
            builder.Property(x => x.Code).IsRequired().HasMaxLength(15);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(30);
            builder.Property(x => x.Stock).IsRequired();
            builder.Property(x => x.Percentage).IsRequired();
        }
    }
}
