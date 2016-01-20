using System.Web;
using System.Web.Optimization;


namespace MLAccountingWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Contents/css").Include(
                      "~/Content/Site.css",
                      "~/Content/datepicker.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                "~/Content/bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/validation").Include(
                "~/Scripts/jquery-2.1.4.min.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js"));
            //Layout
            bundles.Add(new ScriptBundle("~/bundles/layout").Include(
                "~/Scripts/js/glaccounts.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js"));
            //Home
            bundles.Add(new ScriptBundle("~/bundles/home").Include(
                "~/Scripts/js/bootstrap-datepicker.js",
                "~/Scripts/js/unprocess.js"));
            bundles.Add(new ScriptBundle("~/bundles/unprocess").Include(
                ));
            //Report
            bundles.Add(new ScriptBundle("~/bundles/report").Include(
                "~/Scripts/js/Report.js"));
            //Report Accounts
            bundles.Add(new ScriptBundle("~/bundles/Accounts").Include(
                "~/Scripts/js/Accounts.js"));
            //Beginning Balance
            bundles.Add(new ScriptBundle("~/bundles/GLBalance").Include(
                "~/Scripts/js/begbal.js"));
            bundles.Add(new ScriptBundle("~/bundles/Validations").Include(
                "~/Scripts/jquery.validate.unobtrusive.min.js"));
            //Data Entry Scripts
            bundles.Add(new ScriptBundle("~/bundles/js/entry").Include(
                "~/Scripts/js/bootstrap-datepicker.js",
                "~/Scripts/js/DataEntry.js"));
            bundles.Add(new StyleBundle("~/Content/dataentry").Include(
                      "~/Content/DataEntry.css"));
            //update Entry Script
            bundles.Add(new ScriptBundle("~/bundles/js/updateEntry").Include(
                "~/Scripts/js/updateEntry.js"));

            bundles.Add(new ScriptBundle("~/bundles/js/creategls").Include(
                "~/Scripts/js/main.js"));

        }
    }
}
