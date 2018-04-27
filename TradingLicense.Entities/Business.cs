using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Entities
{
    public class Business
    {
        [Key]
        public int BusinessID { get; set; }
        public string BusinessName { get; set; }
        public string OfficePhone { get; set; }
        public string PremiseAddress { get; set; }
        public string PremiseMap { get; set; }
        public string PremisePic { get; set; }
        public string OwnRent { get; set; }
        public DateTime? RentFrom { get; set; }
        public DateTime? RentUntil { get; set; }
        public float FloorArea { get; set; }
        public string FloorSketch { get; set; }
        public int PremiseTypeID { get; set; }
        public string WhichFloor { get; set; }
        public bool Active { get; set; }

        public Business(bool Active)
        {
            Active = true;
        }
        public virtual ICollection<Attachment> Attachments { get; set; }
        public virtual PremiseType PremiseType { get; set; }
    }
}
