using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
    public class BTLinkReqDoc
    {
        [Key]
        public int BTLinkReqDocID { get; set; }

        public int BusinessTypeID { get; set; }

        public int RequiredDocID { get; set; }

        public virtual BusinessType BusinessType { get; set; }

        public virtual RequiredDoc RequiredDoc { get; set; }
    }
}
