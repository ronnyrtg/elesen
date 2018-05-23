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
        public int Period { get; set; }
        public int PeriodQuantity { get; set; }
        public int Mode { get; set; }
        public bool Active { get; set; }
        public BusinessCode()
        {
            Active = true;
        }

        public virtual Sector Sector { get; set; }
        
    }
}
