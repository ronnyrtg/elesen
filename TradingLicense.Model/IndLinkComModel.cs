using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class IndLinkComModel
    {
        public int IndLinkComID { get; set; }
        public int IndividualID { get; set; }

        public string IndPosition { get; set; }
        public int CompanyID { get; set; }

        public string FullName { get; set; }
        public string CompanyName { get; set; }

        public virtual IndividualModel Individual { get; set; }
        public virtual CompanyModel Company { get; set; }
    }
}
