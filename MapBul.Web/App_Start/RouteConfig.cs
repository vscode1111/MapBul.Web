using System.Web.Mvc;
using System.Web.Routing;

namespace MapBul.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "ArticlePhoto",
                "ArticlePhoto/{fileName}",
                new { controller = "ArticlePhoto", action = "Index", fileName = UrlParameter.Optional }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}