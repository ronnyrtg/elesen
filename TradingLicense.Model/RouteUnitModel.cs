using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Model
{
    public class RouteUnitModel
    {
        public int RouteUnitID { get; set; }
        public int ApplicationType { get; set; }
        public int ApplicationID { get; set; }
        public int DepartmentID { get; set; }
        public bool IsSupported { get; set; }
        public string Comment { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsersID { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public bool Active { get; set; }

        public string DepartmentDesc { get; set; }
        public string FullName { get; set; }
    }
}
