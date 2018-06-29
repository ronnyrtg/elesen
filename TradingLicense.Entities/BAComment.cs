using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TradingLicense.Entities
{
    public class BAComment
    {
        [Key]
        public int BACommentID { get; set; }

        public int BannerApplicationID { get; set; }

        [StringLength(255)]
        [Column(TypeName = "VARCHAR2")]
        public string Comment { get; set; }

        public int UsersID { get; set; }

        public DateTime CommentDate { get; set; }

        public virtual PremiseApplication BannerApplication { get; set; }

        public virtual Users Users { get; set; }
    }
}
