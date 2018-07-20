using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class RD_L_BC
    {
        [Key]
        public int RD_L_BCID { get; set; }
        public int RD_ID { get; set; }
        public int BC_ID { get; set; }

        public virtual BC BC { get; set; }
        public virtual RD RD { get; set; }
    }
}
