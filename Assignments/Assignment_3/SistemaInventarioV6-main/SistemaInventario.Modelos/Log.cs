using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventarioV6.Modelos
{
    public class Log
    {
        [Key]
        public int ID { get; set; }
        
        [Required]
        public string UserID {  get; set; }
        
        [Required]
        public DateTime TimeStamp { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey("UserID")]
        public AppUser AppUser { get; set; }

    }
}
