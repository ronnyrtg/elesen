using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class PAComment
    {
        [Key]
        public int PACommentID { get; set; }

        public int PremiseApplicationID { get; set; }

        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string Comment { get; set; }

        public int UsersID { get; set; }

        public DateTime CommentDate { get; set; }

        public virtual PremiseApplication PremiseApplication { get; set; }

        public virtual Users Users { get; set; }
    }
}
