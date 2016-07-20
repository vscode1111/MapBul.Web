using System.Web.Optimization;

namespace MapBul.XIsland
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/js/custom/index").Include(
                        "~/Scripts/Custom/Index.js"));
            bundles.Add(new ScriptBundle("~/js/custom/events").Include(
                        "~/Scripts/Custom/Events.js"));
            bundles.Add(new ScriptBundle("~/js/custom/articles").Include(
                        "~/Scripts/Custom/Articles.js"));


            bundles.Add(new ScriptBundle("~/js/jquery").Include(
                        "~/Scripts/jquery-3.1.0.min.js", "~/Scripts/jquery.unobtrusive-ajax.min.js"));


            bundles.Add(new ScriptBundle("~/plugins/js/datePicker").Include(
                        "~/Scripts/jquery.datepicker.min.js"));
            bundles.Add(new StyleBundle("~/plugins/css/datePicker").Include(
                        "~/Content/pickmeup.css"));

            bundles.Add(new StyleBundle("~/css/style").Include("~/Content/style.css",
                        "~/Content/mediaqueries.css"));

        }
    }
}