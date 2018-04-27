using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
   public class SupportDocs
    {
        [Key]
        public int SupportDocsID { get; set; }

        [Required]
        public int BusinessCodeID { get; set; }

        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string SuppDocDesc { get; set; }

        public virtual BusinessCode BusinessCode { get; set; }
    }
}
