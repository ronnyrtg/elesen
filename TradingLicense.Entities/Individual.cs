using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
    public class Individual
    {
        [Key]
        public int IndividualID { get; set; }

        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string FullName { get; set; }

        [StringLength(30)]
        [Column(TypeName = "VARCHAR2")]
        public string MykadNo { get; set; }

        public int NationalityID { get; set; }

        [StringLength(200)]
        [Column(TypeName = "VARCHAR2")]
        public string AddressIC { get; set; }

        [StringLength(200)]
        [Column(TypeName = "VARCHAR2")]
        public string PhoneNo { get; set; }

        [StringLength(200)]
        [Column(TypeName = "VARCHAR2")]
        public string IndividualEmail { get; set; }

        public int Gender { get; set; }
        public float Rental { get; set; }
        public float Assessment { get; set; }
        public float Compound { get; set; }
        public bool Active { get; set; }
        public Individual()
        {
            Active = true;
        }

    }
}
