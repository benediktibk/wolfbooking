using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Frontend
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            var uiGridBundle = new ScriptBundle("~/bundles/uiGrid").Include(
                "~/Scripts/ui-bootstrap-tpls-2.0.1.min.js",
                "~/Scripts/ui-grid.min.js");
            bundles.Add(uiGridBundle);

            var bootstrapBundle = (new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));
            bundles.Add(bootstrapBundle);

            var angularBundle = (new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/angular.min.js",
                "~/Scripts/angular-route.min.js",
                "~/Scripts/angular-animate.min.js",
                "~/Scripts/angular-sanitize.min.js",
                "~/Scripts/angular-translate.min.js",
                "~/Scripts/angular-translate-loader-static-files.min.js",
                "~/Scripts/angular-cookies.min.js"));
            bundles.Add(angularBundle);
            
            var wolfBookingBundle = (new ScriptBundle("~/bundles/wolfBooking").Include(
                "~/app/Wolfbooking.js"));
            bundles.Add(wolfBookingBundle);

            var servicesBundle = (new ScriptBundle("~/bundles/services").Include(
                "~/app/services/Breads.js",
                "~/app/services/Authentication.js",
                "~/app/services/Users.js",
                "~/app/services/PageHistory.js",
                "~/app/services/Roles.js",
                "~/app/services/Tables.js",
                "~/app/services/Rooms.js",
                "~/app/services/Breadbookings.js",
                "~/app/services/Accounting.js"));
            bundles.Add(servicesBundle);

            var controllersBundle = (new ScriptBundle("~/bundles/controllers").Include(
                "~/app/controllers/HomeCtrl.js",
                "~/app/controllers/BreadsCtrl.js",
                "~/app/controllers/LoginCtrl.js",
                "~/app/controllers/UsersCtrl.js",
                "~/app/controllers/RoomsCtrl.js",
                "~/app/controllers/BreadbookingsCtrl.js",
                "~/app/controllers/AccountingCtrl.js"));
            bundles.Add(controllersBundle);

            var styleBundle = (new StyleBundle("~/Content/css").Include(
                "~/Content/Site.css",
                "~/Content/accounting.css",
                "~/Content/breads.css",
                "~/Content/common.css",
                "~/Content/login.css",
                "~/Content/users.css"));
            bundles.Add(styleBundle);

            var thirdPartyStyles = (new StyleBundle("~/Content/other_css").Include(
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/bootstrap.css",
                "~/Content/ui-grid.css",
                "~/Content/font-awesome.css"));
            bundles.Add(thirdPartyStyles);

            // necessary to exclude these from minification, otherwise the SPA is unable to load correctly :/
            var excludeFromMinification = new [] { controllersBundle, servicesBundle };
            excludeFromMinification.ToList().ForEach(b => b.Transforms.Clear());

#if !DEBUG
            BundleTable.EnableOptimizations = true;
#endif
        }
    }
}