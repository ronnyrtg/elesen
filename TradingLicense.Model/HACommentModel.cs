using System;

namespace TradingLicense.Model
{
    public class HACommentModel
    {
        public int HACommentID { get; set; }
        public int HawkerApplicationID { get; set; }
        public string Comment { get; set; }
        public int UsersID { get; set; }
        public DateTime CommentDate { get; set; }
        public string UserName { get; set; }
    }
}
