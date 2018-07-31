using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    public class APP_L_RD
    {
        [Key]
        public int APP_L_RDID { get; set; }
        public int APP_ID { get; set; }
        public int RD_TYPE { get; set; }
        public int RD_ID { get; set; }
        public int? ATT_ID { get; set; }

        public virtual ATTACHMENT ATTACHMENT { get; set; }
        public virtual APPLICATION APPLICATION { get; set; }
        public virtual RD RD { get; set; }
    }
}
