using System.Web.Optimization;

namespace MapBul.XIsland
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/jquery").Include(
                        "~/Scripts/jquery-3.1.0.min.js"));
            bundles.Add(new ScriptBundle("~/css/style").Include(
                        "~/Content/style.css"));
            bundles.Add(new ScriptBundle("~/css/style").Include(
                        "~/Content/mediaqueries.css"));
        }
    }
}