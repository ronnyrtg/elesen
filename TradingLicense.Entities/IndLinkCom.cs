using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
   public class IndLinkCom
    {
        [Key]
        public int IndLinkComID { get; set; }
        public int IndividualID { get; set; }
        public int CompanyID { get; set; }


        public virtual Individual Individual { get; set; }
        public virtual Company Company { get; set; }
    }
}
