using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
    public class PALinkReqDoc
    {
        [Key]
        public int PALinkReqDocID { get; set; }

        public int PremiseApplicationID { get; set; }

        public int RequiredDocID { get; set; }

        public int? AttachmentID { get; set; }

        public virtual PremiseApplication PremiseApplication { get; set; }

    }
}
