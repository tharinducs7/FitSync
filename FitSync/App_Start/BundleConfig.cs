using System.Web.Optimization;

namespace FitSync
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Assets/vendor/jqueryui/js/jquery-ui.min.js",
                        "~/Assets/vendor/global/global.min.js",
                        "~/Assets/vendor/bootstrap-select/dist/js/bootstrap-select.min.js",
                        "~/Assets/vendor/chart.js/Chart.bundle.min.js",
                        "~/Assets/js/custom.min.js",
                        "~/Assets/js/deznav-init.js",
                        "~/Assets/vendor/owl-carousel/owl.carousel.js",
                        "~/Assets/vendor/peity/jquery.peity.min.js",
                        "~/Assets/vendor/apexchart/apexchart.js",
                        "~/Assets/js/dashboard/dashboard-1.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Assets/css").Include(
                      "~/Assets/css/style.css",
                      "~/Assets/vendor/owl-carousel/owl.carousel.cs",
                      "~/Assets/vendor/bootstrap-select/dist/css/bootstrap-select.min.css",
                      "~/Assets/vendor/chartist/css/chartist.min.css",
                      "~/Assets/vendor/LineIcons/LineIcons.css"));
        }
    }
}
