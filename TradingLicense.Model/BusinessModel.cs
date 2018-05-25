using System;
using System.ComponentModel.DataAnnotations;

namespace TradingLicense.Model
{
    public class BusinessModel
    {
        public int BusinessID { get; set; }

        [StringLength(150)]
        public string BusinessName { get; set; }

        [StringLength(50)]
        public string OfficePhone { get; set; }

        [StringLength(255)]
        public string PremiseAddress { get; set; }

        [StringLength(200)]
        public string PremiseMap { get; set; }

        [StringLength(200)]
        public string PremisePic { get; set; }

        [StringLength(150)]
        public string OwnRent { get; set; }

        public DateTime? RentFrom { get; set; }

        public DateTime? RentUntil { get; set; }

        public float FloorArea { get; set; }

        public string FloorSketch { get; set; }
        
        [StringLength(200)]
        public string WhichFloor { get; set; }

        public bool Active { get; set; }

        public int PremiseTypeID { get; set; }
    }
}
