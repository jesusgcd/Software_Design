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
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.ID).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(60);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ImageURL).IsRequired(false);
            builder.Property(x => x.FoodCategoryId).IsRequired();

            //Relaciones

            builder.HasOne(x => x.FoodCategory).WithMany()
                .HasForeignKey(x => x.FoodCategoryId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
