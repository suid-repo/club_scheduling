using System.Web;
using System.Web.Optimization;

namespace WebApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js",
                        "~/Scripts/jquery-ui-timepicker-addon.js",
                      "~/Scripts/jquery.cookie.js",
                      "~/Scripts/jquery.fancybox.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      //"~/Scripts/popper.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap4-toggle.js"));

            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                      "~/Scripts/front.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/bootstrap4-toggle.css",
                      "~/Content/jquery-ui.css",
                      "~/Content/jquery-ui-timepicker-addon.css",
                      "~/Content/jquery.fancybox.css",
                      "~/Content/fontastic.css",
                      "~/Content/style.blue.css",
                      "~/Content/custom.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-rtl").Include(
           "~/Scripts/bootstrap-rtl.js"));

            bundles.Add(new StyleBundle("~/Content/css-rtl").Include(
                      "~/Content/bootstrap-rtl.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/pagedlist").Include(
                      "~/Content/PagedList.css"));
        }
    }
}
