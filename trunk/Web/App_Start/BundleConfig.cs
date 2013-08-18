using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterScriptsBundles(bundles);
            RegisterStylesBundles(bundles);
        }

        private static void RegisterScriptsBundles(BundleCollection bundles)
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

            bundles.Add(new ScriptBundle("~/bundles/modernui").Include(
                        "~/Scripts/jquery.mousewheel.js",
                        "~/Scripts/modern-ui/accordion.js",
                        "~/Scripts/modern-ui/buttonset.js",
                        "~/Scripts/modern-ui/calendar.js",
                        "~/Scripts/modern-ui/carousel.js",
                        "~/Scripts/modern-ui/dialog.js",
                        "~/Scripts/modern-ui/dropdown.js",
                        "~/Scripts/modern-ui/input-control.js",
                        "~/Scripts/modern-ui/pagecontrol.js",
                        "~/Scripts/modern-ui/pagelist.js",
                        "~/Scripts/modern-ui/rating.js",
                        "~/Scripts/modern-ui/slider.js",
                        "~/Scripts/modern-ui/start-menu.js",
                        "~/Scripts/modern-ui/tile-drag.js",
                        "~/Scripts/modern-ui/tile-slider.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/moment_langs.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                        "~/Scripts/custom/common.js",
                        "~/Scripts/custom/custom-calendar.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                        "~/Scripts/custom/admin.js"));

            bundles.Add(new ScriptBundle("~/bundles/customer").Include(
                        "~/Scripts/jquery.dragsort-{version}.js",
                        "~/Scripts/custom/customer.js"));

            bundles.Add(new ScriptBundle("~/bundles/router").Include(
                        "~/Scripts/custom/router.js"));

            bundles.Add(new ScriptBundle("~/bundles/master").Include(
                        "~/Scripts/jquery.dragsort-{version}.js",
                        "~/Scripts/custom/master.js"));

            bundles.Add(new ScriptBundle("~/bundles/executor").Include(
                        "~/Scripts/jquery.dragsort-{version}.js",
                        "~/Scripts/custom/executor.js"));
        }

        private static void RegisterStylesBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            //"~/Content/themes/base/ModernUI/theme-dark.css"
            bundles.Add(new StyleBundle("~/Content/themes/base/modern_ui").Include(
                        "~/Content/themes/base/modern-responsive.css",
                        "~/Content/themes/base/modern.css"));
        }
    }
}