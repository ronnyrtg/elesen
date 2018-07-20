using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class APP_L_BC
    {
        [Key]
        public int APP_L_BCID { get; set; }
        public int APP_ID { get; set; }
        public int BC_ID { get; set; }

        public virtual BC BC { get; set; }
        public virtual APPLICATION APPLICATION { get; set; }
    }
}
