using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class EAComment
    {
        [Key]
        public int EACommentID { get; set; }

        public int EntmtApplicationID { get; set; }

        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string Comment { get; set; }

        public int UsersID { get; set; }

        public DateTime CommentDate { get; set; }

        public virtual EntmtApplication EntmtApplication { get; set; }

        public virtual Users Users { get; set; }
    }
}
