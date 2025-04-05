using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class Product
    {
        [Key]
        public int ID {  get; set; }

        [Required(ErrorMessage ="Nombre es requerido")]
        [MaxLength(60)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Descripcion es requerido")]
        [MaxLength(100)]
        public string Description { get; set; }

        public string ImageURL { get; set; }

        [Required(ErrorMessage ="Categoria es requerido")]
        public int FoodCategoryId {  get; set; }

        [ForeignKey("FoodCategoryId")]
        public FoodCategory FoodCategory {  get; set; }

    }
}
