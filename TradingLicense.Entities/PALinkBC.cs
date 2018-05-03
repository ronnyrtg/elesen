using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
   public class PALinkBC
    {
        [Key]
        public int PALinkBCID { get; set; }

        public int PremiseApplicationID { get; set; }

        public int BusinessCodeID { get; set; }

        public virtual BusinessCode BusinessCode { get; set; }
        public virtual PremiseApplication PremiseApplication { get; set; }
    }
}
