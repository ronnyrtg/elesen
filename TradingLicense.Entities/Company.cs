﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }
        public int? BusinessTypeID { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string RegistrationNo { get; set; }
        [StringLength(100)]
        [Column(TypeName = "VARCHAR2")]
        public string CompanyName { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string CompanyPhone { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string Addra1 { get; set; }
        [StringLength(40)]
        [Column(TypeName = "VARCHAR2")]
        public string Addra2 { get; set; }
        [StringLength(40)]
        [Column(TypeName = "VARCHAR2")]
        public string Addra3 { get; set; }
        [StringLength(40)]
        [Column(TypeName = "VARCHAR2")]
        public string Addra4 { get; set; }
        [StringLength(10)]
        [Column(TypeName = "VARCHAR2")]
        public string PcodeA { get; set; }
        [StringLength(50)]
        [Column(TypeName = "VARCHAR2")]
        public string StateA { get; set; }
        public DateTime? SSMRegDate { get; set; }
        public DateTime? SSMExpDate { get; set; }
        public float? AuthorisedCapital { get; set; }
        public float? IssuedCapital { get; set; }
        public float? PaidUpCapitalCash { get; set; }
        public float? PaidUpCapitalOther { get; set; }
        public float? BankSource  { get; set; }
        public float? DepositSource { get; set; }
        public float? LoanSource { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string LoanSourceName { get; set; }
        public float? OtherSource { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string OtherSourceName { get; set; }
        public bool Active { get; set; }

        public virtual BusinessType BusinessType { get; set; }

        public Company()
        {
            Active = true;
        }
    }
}
