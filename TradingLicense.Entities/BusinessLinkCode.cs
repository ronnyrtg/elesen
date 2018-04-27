using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
    public class BLinkCode
    {
        [Key]
        public int Id { get; set; }
        public int BusinessID { get; set; }
        public int BusinessCodeID { get; set; }

        public virtual Business Business { get; set; }
        public virtual BusinessCode BusinessCode { get; set; }
    }
}
