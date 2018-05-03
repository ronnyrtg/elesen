using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
    public class BusinessCode
    {
        [Key]
        public int BusinessCodeID { get; set; }
        [StringLength(5)]
        [Column(TypeName = "VARCHAR2")]
        public string CodeNumber { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string CodeDesc { get; set; }
        public int SectorID { get; set; }
        public float DefaultRate { get; set; }
        public float BaseFee { get; set; }
        public float ExtraFee { get; set; }
        public int ExtraUnit { get; set; }
        [StringLength(1)]
        [Column(TypeName = "VARCHAR2")]
        public string Period { get; set; }
        public int PQuantity { get; set; }
        public bool Express { get; set; }
        public bool Active { get; set; }
        public BusinessCode()
        {
            Express = true;
            Active = true;
        }

        public virtual Sector Sector { get; set; }
    }
}
