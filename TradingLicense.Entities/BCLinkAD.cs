using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
   public class BCLinkAD
    {
        public int BCLinkADID { get; set; }

        public int BusinessCodeID { get; set; }

        public int AdditionalDocID { get; set; }

        public virtual AdditionalDoc AdditionalDoc { get; set; }
    }
}
