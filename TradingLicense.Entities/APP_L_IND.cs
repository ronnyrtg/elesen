using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class APP_L_IND
    {
        [Key]
        public int APP_L_INDID { get; set; }
        public int APP_ID { get; set; }
        public int IND_ID { get; set; }

        public virtual INDIVIDUAL INDIVIDUAL { get; set; }
        public virtual APPLICATION APPLICATION { get; set; }
    }
}
