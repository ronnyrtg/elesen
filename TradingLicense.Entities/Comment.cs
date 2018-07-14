using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public int ApplicationType { get; set; }
        public int ApplicationID { get; set; }
        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string Content { get; set; }
        public int UsersID { get; set; }
        public DateTime CommentDate { get; set; }

        public virtual Users Users { get; set; }
    }
}
