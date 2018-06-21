using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class HAComment
    {
        [Key]
        public int HACommentID { get; set; }

        public int HawkerApplicationID { get; set; }

        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string Comment { get; set; }

        public int UsersID { get; set; }

        public DateTime CommentDate { get; set; }

        public virtual HawkerApplication HawkerApplication { get; set; }

        public virtual Users Users { get; set; }
    }
}
