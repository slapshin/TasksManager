using System.Web.Optimization;

namespace Web.SPA
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterScriptBundles(bundles);
            RegisterStyleBundles(bundles);
        }

        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                                        "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap").Include(
                                        "~/Content/bootstrap/bootstrap.css",
                                        "~/Content/bootstrap/bootstrap-theme.css"));

            bundles.Add(new StyleBundle("~/Content/angular").Include(
                                        "~/Content/angular-ui.css",
                                        "~/Content/ng-grid.css"));

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
        }

        private static void RegisterScriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                                        "~/Scripts/jquery.unobtrusive*",
                                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                                        "~/Scripts/angular.js",
                                        "~/Scripts/angular-resource.js",
                                        "~/Scripts/ng-grid-{version}.js",
                                        "~/Scripts/angular-ui.js",
                                        "~/Scripts/ui-bootstrap-tpls-{version}.js"
                                        ));

            bundles.Add(new ScriptBundle("~/bundles/ajaxlogin").Include(
                                        "~/Scripts/custom/ajaxlogin.js"));

            bundles.Add(new ScriptBundle("~/bundles/tasks").Include(
                                        "~/app/app.js", // must be first
                                        "~/app/directives.js",
                                        "~/app/route.js",
                                        "~/app/consts.js",
                                        "~/app/services/logger.js",
                                        "~/app/services/common.js",
                                        "~/app/controllers/home.js",
                                        "~/app/admin/controllers/users.js",
                                        "~/app/admin/controllers/projects.js",
                                        "~/app/admin/services/users.js",
                                        "~/app/admin/services/projects.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                       "~/Scripts/bootstrap.js"));
        }
    }
}