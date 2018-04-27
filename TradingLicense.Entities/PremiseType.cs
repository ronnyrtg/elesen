using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TradingLicense.Entities
{
    public class PremiseType
    {
        [Key]
        public int PremiseTypeID { get; set; }
        [Required]
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string PremiseDesc { get; set; }
        public bool Active { get; set; }
        public PremiseType()
        {
            Active = true;
        }
    }
}
