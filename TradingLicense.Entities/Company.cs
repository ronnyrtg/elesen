using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }

        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string RegistrationNo { get; set; }
        [StringLength(20)]
        [Column(TypeName = "VARCHAR2")]
        public string CompanyPhone { get; set; }
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string CompanyName { get; set; }

        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string CompanyAddress { get; set; }
        public bool Active { get; set; }
        public Company()
        {
            Active = true;
        }
    }
}
