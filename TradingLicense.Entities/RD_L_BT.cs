using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Entities
{
    //Business Type Link Required Document
    public class RD_L_BT
    {
        [Key]
        public int BT_L_RD_ID { get; set; }
        //Business Type ID
        public int BT_ID { get; set; }
        //Required Document ID
        public int RD_ID { get; set; }

        //Business Type table
        public virtual BT BT { get; set; }
        //Required Document table
        public virtual RD RD { get; set; }
    }
}
