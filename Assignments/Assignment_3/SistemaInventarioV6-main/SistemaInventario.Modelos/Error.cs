using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class Error
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(20)]
        public string Code { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        [MaxLength(150)]
        public string Description { get; set; }
    }
}
