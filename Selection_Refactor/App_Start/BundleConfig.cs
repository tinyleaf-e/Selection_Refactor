using System.Web;
using System.Web.Optimization;

namespace Selection_Refactor
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/js/jquery-3.3.1.min.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Content/plugins/modernizr-2.8.3/js/modernizr-2.8.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Content/plugins/bootstrap-3.3.7/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/render").Include(
                      "~/Content/js/render.js"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                      "~/Content/js/global.js",
                      "~/Content/js/gy-alert.js",
                      "~/Content/js/adminlte.min.js",
                      "~/Content/js/changePasswd.js"));

            bundles.Add(new StyleBundle("~/css/bootstrap").Include(
                      "~/Content/plugins/bootstrap-3.3.7/css/bootstrap.min.css"));
            bundles.Add(new StyleBundle("~/css/site").Include(
                      "~/Content/css/global.css",
                      "~/Content/plugins/font-awesome/css/font-awesome.css"));
            bundles.Add(new StyleBundle("~/css/AdminLTE").Include(
                       "~/Content/css/AdminLTE.min.css",
                       "~/Content/css/_all-skins.css"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
        }
    }
}
