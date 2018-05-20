using System.Collections.Generic;

namespace TradingLicense.Model
{
    public class PageRoleAccessModel
    {
        public int PageID { get; set; }
        public string PageDesc { get; set; }
        public List<AccessPageModel> RoleAccess { get; set; }
    }
}