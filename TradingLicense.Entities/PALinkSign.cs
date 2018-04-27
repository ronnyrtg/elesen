using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
   public class PALinkSign
    {
        [Key]
        public int PALinkSignID { get; set; }

        public int PAID { get; set; }

        public int SignboardID { get; set; }

        public virtual Signboard Signboard { get; set; }
    }
}
