using System.Web;
using System.Web.Optimization;

namespace Squirrel.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive-ajax.js",
            //            "~/Scripts/jquery.validate.js",
            //            "~/Scripts/jquery.validate.unobtrusive.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/General/CommonMethods.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-1.11.2.js"));

            bundles.Add(new StyleBundle("~/Content/Autocomplete").Include(
                "~/Content/themes/base/theme.css",
                "~/Content/themes/base/core.css",
                "~/Content/themes/base/menu.css",
                "~/Content/themes/base/autocomplete.css"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive-ajax.js",
            //            "~/Scripts/jquery.validate.js",
            //            "~/Scripts/jquery.validate.unobtrusive.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-rtl/bootstrap-rtl.css"));

            bundles.Add(new StyleBundle("~/Content/vone").Include(
                      "~/Content/themes/vone/main.css",
                      "~/Content/themes/vone/fonts.css",
                      "~/Content/themes/vone/loading.css"));

            // Persian DateTimePicker
            bundles.Add(new StyleBundle("~/Content/PersianDatatimePicker").Include(
                      "~/Content/MdBootstrapPersianDateTimePicker/jquery.Bootstrap-PersianDateTimePicker.css"));

            bundles.Add(new ScriptBundle("~/bundles/PersianDatatimePicker").Include(
                "~/Scripts/MdBootstrapPersianDateTimePicker/calendar.js",
                "~/Scripts/MdBootstrapPersianDateTimePicker/jquery.Bootstrap-PersianDateTimePicker.js"));

            // Post Page Bundles
            bundles.Add(new StyleBundle("~/content/PostPage").Include(
                      "~/ckeditor/plugins/codesnippet/lib/highlight/styles/solarized_light.css"));

            bundles.Add(new ScriptBundle("~/bundles/PostPage").Include(
                "~/ckeditor/plugins/codesnippet/lib/highlight/highlight.pack.js"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
