using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
   public class RoleTemplate
    {
        [Key]
        public int RoleTemplateID { get; set; }

        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string RoleTemplateDesc { get; set; }
        
    }
}
