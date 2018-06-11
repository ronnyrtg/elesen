using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class LAComment
    {
        [Key]
        public int LACommentID { get; set; }

        public int LiquorApplicationID { get; set; }

        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string Comment { get; set; }

        public int UsersID { get; set; }

        public DateTime CommentDate { get; set; }

        public virtual LiquorApplication LiquorApplication { get; set; }

        public virtual Users Users { get; set; }
    }
}
