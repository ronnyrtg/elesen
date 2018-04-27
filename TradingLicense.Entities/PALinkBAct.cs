using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
   public class PALinkBAct
    {
        [Key]
        public int PALinkBActID { get; set; }

        public int PAID { get; set; }

        public int BusinessActivityID { get; set; }

        public virtual BusinessActivity BusinessActivity { get; set; }
    }
}
