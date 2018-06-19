using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingLicense.Model
{
    public class PACommentModel
    {
        public int PACommentID { get; set; }
        public int PremiseApplicationID { get; set; }
        public string Comment { get; set; }
        public int UsersID { get; set; }
        public DateTime CommentDate { get; set; }
        public string UserName { get; set; }
    }
}
