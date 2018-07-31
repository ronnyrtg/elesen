using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    class CitizenModel
    {
        [Key]
        public int CITIZENID { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string CITIZEN_CODE { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string CITIZEN_DESC { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ID_STAMP { get; set; }
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string TIME_STAMP { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string REC_STATUS { get; set; }
    }
}
