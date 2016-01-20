using System.Web.Optimization;
using ML.Core;
using ML.Core.Contracts;

namespace ML.Web
{
    public class BundleConfig : IInitializer
    {
        private readonly BundleCollection bundles;

        public BundleConfig(BundleCollection bundles)
        {
            this.bundles = bundles;
        }

        public void Initialize()
        {
            //BundleTable.EnableOptimizations = true;

            this.bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                          "~/Scripts/jquery-{version}.js"));

            this.bundles.Add(new ScriptBundle("~/bundles/jquery.unobtrusive").Include(
              "~/Scripts/jquery.unobtrusive-ajax.js"));
            //modernizr
            this.bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));
            //bootrap + site styles
            this.bundles.Add(new StyleBundle("~/Content/site").Include(
                            "~/Content/bootstrap.min.css",
                            "~/Content/site.css",
                            "~/Content/css/Or.css",
                            "~/Content/PagedList.css"));

            //Registration css
            this.bundles.Add(new StyleBundle("~/Content/Registration").Include(
                            "~/Content/css/SetupWizard.css",
                            "~/Content/css/Products.css",
                            "~/Content/css/Wallet.css"
                ));

            this.bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/css/footable.css"));
            //login style
            this.bundles.Add(new StyleBundle("~/Content/style").Include(
                "~/Content/style.css"));

            //bootstrap javascripts
            this.bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));

            // jQuery validate
            this.bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/Forms.js"));

            // my javascript
            this.bundles.Add(new ScriptBundle("~/bundles/myjs").Include(
                        "~/Scripts/myjs.js"));

            // my javascript
            //this.bundles.Add(new ScriptBundle("~/bundles/createpayment").Include(
            //            "~/Scripts/createpay.js"));

            this.bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            this.bundles.Add(new ScriptBundle("~/bundles/myscript").Include(
                        "~/Scripts/script.js"));
            //Group Controller Script
            this.bundles.Add(new ScriptBundle("~/bundles/Group").Include(
                        "~/Scripts/Group.js",
                        "~/Scripts/Header.js"));

            //Employee Controller Script
            this.bundles.Add(new ScriptBundle("~/bundles/Employee").Include(
                        "~/Scripts/Employee.js",
                        "~/Scripts/Header.js"));

            //User Controller Script
            this.bundles.Add(new ScriptBundle("~/bundles/AccountRegistration").Include("~/Scripts/AccountRegistration.js"));

            //Payment, Home, Report Controller Script
            this.bundles.Add(new ScriptBundle("~/bundles/Payment").Include("~/Scripts/Header.js"));
            this.bundles.Add(new ScriptBundle("~/bundles/footables").Include("~/Scripts/footable.js"));

            this.bundles.Add(new ScriptBundle("~/bundles/myreports").Include(
            "~/Scripts/script.js",
            "~/bootstrap.min.js"));

            //Update Profile
            this.bundles.Add(new ScriptBundle("~/bundles/js/UpdateProfile").Include(
                    "~/Scripts/js/UpdateProfile.js",
                    "~/Scripts/Header.js"));
            //Cancellation
            this.bundles.Add(new ScriptBundle("~/bundles/js/Cancellation").Include(
                    "~/Scripts/footable.js",
                    "~/Scripts/js/Cancellation.js",
                    "~/Scripts/Header.js",
                    "~/Scripts/jquery-ui-{version}.js"));
        }
    }
}