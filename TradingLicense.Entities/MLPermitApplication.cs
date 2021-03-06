﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class MLPermitApplication
    {
        [Key]
        public int MLPermitApplicationID { get; set; }
        public int LenderApplicationID { get; set; }
        public int Brochure { get; set; }
        public int Newspaper { get; set; }
        public int SignBoard { get; set; }
        public int Radio { get; set; }
        public int Internet { get; set; }
        public int Television { get; set; }
        public int VCD { get; set; }
        public int Cinema { get; set; }
        public int Others { get; set; }
        public string SpecifyOthers { get; set; }
        public int AppStatusID { get; set; }

        //The user who creates this application
        public int UsersID { get; set; }
        public DateTime DateSubmitted { get; set; }

        //The staff processing this application
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string UpdatedBy { get; set; }
        public DateTime? DateApproved { get; set; }
        public float? TotalFee { get; set; }
        public DateTime? DatePaid { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string ReferenceNo { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string LicenseStatus { get; set; }
        public DateTime? LExpireDate { get; set; }

        public virtual AppStatus AppStatus { get; set; }
        public virtual Users Users { get; set; }

    }
}
