using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TradingLicense.Web.Reports
{
    public partial class reportview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["dtPrint"] != null)
                {
                    CrystalDecisions.CrystalReports.Engine.ReportDocument report =
                    new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                    report.Load(System.IO.Path.Combine(Server.MapPath("~/Reports"), "crpTable.rpt"));
                    report.SetDataSource(Session["dtPrint"]);
                    CrystalReportViewer1.PrintMode = CrystalDecisions.Web.PrintMode.ActiveX;
                    CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                    CrystalReportViewer1.ReportSource = report;
                    lblMsg.Text = "";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
            
        }
    }
}