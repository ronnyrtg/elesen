using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class BannerObject
    {
        [Key]
        public int BannerObjectID { get; set; }
        public int ApplicationID { get; set; }

        public int LocationID { get; set; }
        public int BC_ID { get; set; }
        public int BQuantity { get; set; }
        public float BSize { get; set; }
        public float Fee { get; set; }

        public virtual APPLICATION Application { get; set; }
        public virtual Location Location { get; set; }
        public virtual BC BC { get; set; }
    }
}
