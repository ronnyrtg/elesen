using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class RD_L_LT
    {
        [Key]
        public int RD_L_LTID { get; set; }
        public int APP_ID { get; set; }
        public int RD_ID { get; set; }
        public int? ATT_ID { get; set; }

        public virtual APPLICATION APPLICATION { get; set; }
        public virtual RD RD { get; set; }
    }
}
