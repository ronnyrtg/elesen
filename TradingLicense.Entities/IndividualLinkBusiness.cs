using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class ILinkB
    {
        [Key]
        public int Id { get; set; }
        public int IndividualID { get; set; }
        public int BusinessID { get; set; }

        public virtual Individual Individual { get; set; }
        public virtual Business Business { get; set; }
    }
}
