using System.Web.Optimization;

namespace FashionStore.UI.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/adminstyles").Include(
                "~/AdminTemplate/bootstrap/dist/css/bootstrap.min.css",
                "~/AdminTemplate/plugins/bower_components/bootstrap-extension/css/bootstrap-extension.css",
                "~/AdminTemplate/plugins/bower_components/sidebar-nav/dist/sidebar-nav.min.css",
                "~/AdminTemplate/css/animate.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/adminscripts")
                .Include("~/AdminTemplate/plugins/bower_components/jquery/dist/jquery.min.js",
                    "~/AdminTemplate/bootstrap/dist/js/tether.min.js",
                    "~/AdminTemplate/bootstrap/dist/js/bootstrap.min.js",
                    "~/AdminTemplate/plugins/bower_components/bootstrap-extension/js/bootstrap-extension.min.js",
                    "~/AdminTemplate/plugins/bower_components/sidebar-nav/dist/sidebar-nav.min.js",
                    "~/AdminTemplate/js/jquery.slimscroll.js",
                    "~/AdminTemplate/js/waves.js",
                    "~/AdminTemplate/js/custom.min.js",
                    "~/AdminTemplate/plugins/bower_components/jquery-sparkline/jquery.sparkline.min.js",
                    "~/AdminTemplate/plugins/bower_components/jquery-sparkline/jquery.charts-sparkline.js",
                    "~/AdminTemplate/plugins/bower_components/styleswitcher/jQuery.style.switcher.js"
                    ));

            bundles.Add(new StyleBundle("~/bundles/mainstyles")
                .Include(
                "~/MainTemplate/css/bootstrap.min.css",
                "~/MainTemplate/css/magnific-popup.css",
                "~/MainTemplate/css/font-icons.css",
                "~/MainTemplate/css/sliders.css", 
                "~/MainTemplate/css/style.css", 
                "~/MainTemplate/css/animate.min.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/mainscripts")
            .Include(
                    "~/MainTemplate/js/jquery.min.js",
                    "~/MainTemplate/js/bootstrap.min.js",
                    "~/MainTemplate/js/plugins.js",
                    "~/MainTemplate/js/scripts.js"
                    ));
            BundleTable.EnableOptimizations = true;
        }
    }
}