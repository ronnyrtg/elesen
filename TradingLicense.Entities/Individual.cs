using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string AddressIC { get; set; }

        [StringLength(20)]
        [Column(TypeName = "VARCHAR2")]
        public string PhoneNo { get; set; }

        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string IndividualEmail { get; set; }

        //Profile Picture
        public int? AttachmentID { get; set; }

        public int Gender { get; set; }
        public bool Active { get; set; }
        public Individual()
        {
            Active = true;
        }

        public virtual Attachment Attachment { get; set; }
    }
}
