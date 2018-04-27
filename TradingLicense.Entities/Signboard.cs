using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
   public class Signboard
    {
        [Key]
        public int SignboardID { get; set; }
        public float Length { get; set; }
        public float Width { get; set; }
        public int WithLamp { get; set; }
        public int Quantity { get; set; }
        public int FaceQty { get; set; }
        [StringLength(15)]
        [Column(TypeName = "VARCHAR2")]
        public string DisplayMethod { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string Location { get; set; }
    }
}
