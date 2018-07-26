using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    //Banner Object
    public class B_O
    {
        [Key]
        public int B_O_ID { get; set; }
        public int APP_ID { get; set; }

        public string ADDRB1 { get; set; }
        public string ADDRB2 { get; set; }
        public string ADDRB3 { get; set; }
        public string ADDRB4 { get; set; }
        public int BC_ID { get; set; }
        public int B_QTY { get; set; }
        public float B_SIZE { get; set; }
        public float? FEE { get; set; }

        public virtual APPLICATION APPLICATION { get; set; }
        public virtual BC BC { get; set; }
    }
}
