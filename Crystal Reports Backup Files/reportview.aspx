<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reportview.aspx.cs" Inherits="TradingLicense.Web.Reports.reportview" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function Print() {
            var dvReport = document.getElementById("dvReport");
            var frame1 = dvReport.getElementsByTagName("iframe")[0];
            if (navigator.appName.indexOf("Internet Explorer") != -1) {
                frame1.name = frame1.id;
                window.frames[frame1.id].focus();
                window.frames[frame1.id].print();
            }
            else {
                var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                frameDoc.print();
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">

        <input style="background-color:white;border-radius:3px;width:60px;height:28px;cursor:pointer"  type="button" id="btnPrint" value="Print" onclick="Print()" />
         <br />
         <hr />
       
        <asp:Label ID="lblMsg" Text="" ForeColor="Red" Font-Size="Large" runat="server" ></asp:Label>
       
        <div id="dvReport">
            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
        </div>
    </form>
</body>
</html>
