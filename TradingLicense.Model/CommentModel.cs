using System;

namespace TradingLicense.Model
{
    public class CommentModel
    {
        public int COMMENTID { get; set; }
        public int APP_ID { get; set; }
        public string CONTENT { get; set; }
        public int USERSID { get; set; }
        public DateTime COMMENTDATE { get; set; }

        public string FullName { get; set; }
    }
}
