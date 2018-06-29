using System;

namespace TradingLicense.Model
{
    public class BACommentModel
    {
        public int BACommentID { get; set; }
        public int BannerApplicationID { get; set; }
        public string Comment { get; set; }
        public int UsersID { get; set; }
        public DateTime CommentDate { get; set; }
        public string FullName { get; set; }
    }
}
