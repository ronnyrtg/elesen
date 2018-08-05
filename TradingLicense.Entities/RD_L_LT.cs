using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class RD_L_LT
    {
        [Key]
        public int RD_L_LTID { get; set; }
        public int RD_ID { get; set; }
        public int LIC_TYPEID { get; set; }

        public virtual LIC_TYPE LIC_TYPE { get; set; }
        public virtual RD RD { get; set; }
    }
}
