using System;

namespace TradingLicense.Model
{
    public class PACommentModel
    {
        public int PACommentID { get; set; }
        public int PremiseApplicationID { get; set; }
        public string Comment { get; set; }
        public int UsersID { get; set; }
        public DateTime CommentDate { get; set; }
        public string FullName { get; set; }
    }
}
