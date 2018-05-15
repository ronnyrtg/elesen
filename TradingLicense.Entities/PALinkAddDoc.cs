using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
    public class PALinkAddDoc
    {
        [Key]
        public int PALinkAddDocID { get; set; }

        public int PremiseApplicationID { get; set; }

        public int AdditionalDocID { get; set; }

        public int? AttachmentID { get; set; }

        public virtual PremiseApplication PremiseApplication { get; set; }
    }
}
