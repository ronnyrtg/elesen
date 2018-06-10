using System;
using System.Collections.Generic;
using TradingLicense.Model;
using System.Linq;
using System.Web;

namespace TradingLicense.Web.Models
{
    public class ManageIndividualModel
    {
        public IndividualModel Individual { get; set; }
        public List<CheckBoxListItem> Companies { get; set; }
    }
}