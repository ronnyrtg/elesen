using System;

namespace TradingLicense.Model
{
    public class CommentModel
    {
        public int CommentID { get; set; }
        public int ApplicationType { get; set; }
        public int ApplicationID { get; set; }
        public string Content { get; set; }
        public int UsersID { get; set; }
        public DateTime CommentDate { get; set; }

        public string FullName { get; set; }
    }
}
