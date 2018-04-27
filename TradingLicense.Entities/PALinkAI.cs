using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
   public class PALinkAI
    {
        public int PALinkAIID { get; set; }

        public int PAID { get; set; }

        public int AdditionalInfoID { get; set; }

        public virtual AdditionalInfo AdditionalInfo { get; set; }
    }
}
