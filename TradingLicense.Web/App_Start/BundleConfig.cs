using System.Web.Optimization;

namespace TradingLicense.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                            "~/Scripts/Bootstrap/bootstrap.js",
                            "~/Scripts/Datatables/jquery.dataTables.js",
                            "~/Scripts/Datatables/jquery.dataTables.min.js",
                            "~/Scripts/Datatables/dataTables.bootstrap.js",
                            "~/Scripts/Datatables/dataTables.responsive.min.js",
                            "~/Scripts/moment/moment.js",
                            "~/Scripts/TradingLicenseCommon.js",
                            "~/Scripts/respond.js",
                            "~/Scripts/toastr.min.js",
                            "~/Scripts/general.js"));

            bundles.Add(new ScriptBundle("~/bundles/datetime").Include(
                        "~/Scripts/moment*",
                        "~/Scripts/jquery-ui-1.12.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                        "~/Scripts/select2/select2.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                       // "~/Content/Datatables/dataTables.bootstrap4.css",
                       "~/Content/Datatables/dataTables.bootstrap.css",
                       "~/Content/Datatables/responsive.dataTables.min.css",
                      //  "~/Content/site.css",
                      "~/Content/toastr.css",
                      "~/Content/select2/select2.min.css",
                      "~/Content/themes/base/jquery-ui.min.css",
                      "~/Content/Custom.css"));


        }
    }
}
