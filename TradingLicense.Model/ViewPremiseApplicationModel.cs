using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class ViewPremiseApplicationModel
    {
        public int PremiseApplicationId { get; set; }
        public int AppStatusID { get; set; }
        public int UserRollTemplate { get; set; }
        public string Sector { get; set; }
        public string BusinessType { get; set; }
        public string NamaPemohon { get; set; }
        public string ICPaspot { get; set; }
        public string NamaSyarikat { get; set; }
        public List<string> BusinessCodes { get; set; }
        public List<string> Individuals { get; set; }
        public string Company { get; set; }
        public float PremiseArea { get; set; }
        public string PremiseType { get; set; }
        public string PremiseStatus { get; set; }
        public string PremiseOwnership { get; set; }
        public List<string> RequiredDocs { get; set; }
        public List<string> AdditionalDocs { get; set; }
    }
}
