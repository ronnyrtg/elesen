using System.ComponentModel.DataAnnotations;

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

        public virtual BannerApplication BannerApplication { get; set; }

    }
}
