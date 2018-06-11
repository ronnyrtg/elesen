using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class BannerObject
    {
        [Key]
        public int BannerObjectID { get; set; }
        public int BannerApplicationID { get; set; }

        public int LocationID { get; set; }
        public int ZoneID { get; set; }
        public int BannerCodeID { get; set; }
        public int BQuantity { get; set; }
        public float BSize { get; set; }

        public virtual BannerApplication BannerApplication { get; set; }
        public virtual Location Location { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual BannerCode BannerCode { get; set; }
    }
}
