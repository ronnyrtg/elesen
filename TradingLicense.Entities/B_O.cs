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

        public string ADDRA1 { get; set; }
        public string ADDRA2 { get; set; }
        public string ADDRA3 { get; set; }
        public string ADDRA4 { get; set; }
        public int BC_ID { get; set; }
        public int B_QTY { get; set; }
        public float B_SIZE { get; set; }
        public float? FEE { get; set; }

        public virtual APPLICATION Application { get; set; }
        public virtual ADDRESS Address { get; set; }
        public virtual BC BC { get; set; }
    }
}
