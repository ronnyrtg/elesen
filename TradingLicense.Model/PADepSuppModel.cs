using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class PADepSuppModel
    {
        public int PADepSuppID { get; set; }
        public int PremiseApplicationID { get; set; }
        public int DepartmentID { get; set; }
        public bool IsSupported { get; set; }
        public string Comment { get; set; }
        public int UserID { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime SubmittedDate { get; set; }
        public bool IsActive { get; set; }
        public string Department { get; set; }
    }
}
