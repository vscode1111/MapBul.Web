using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MapBul.Web.Filters
{
    public class CultureAttribute : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var routes = RouteTable.Routes;

            var httpContext = HttpContext.Current.Request.RequestContext.HttpContext;
            if (httpContext == null) return;

            var routeData = routes.GetRouteData(httpContext);

            if (routeData != null)
            {
                var language = routeData.Values["language"] as string;

                if (string.IsNullOrEmpty(language))
                {
                    var cookie = HttpContext.Current.Request.Cookies["language"];
                    language = cookie?.Value;
                    if (string.IsNullOrEmpty(language))
                    {
                        var userLanguages = HttpContext.Current.Request.UserLanguages;
                        if (userLanguages != null)
                        {
                            foreach (var userLanguage in userLanguages)
                            {
                                if (userLanguage.ToLower().Contains("ru"))
                                {
                                    language = "ru";
                                    break;
                                }
                                if (userLanguage.ToLower().Contains("en"))
                                {
                                    language = "en";
                                    break;
                                }
                            }
                            if (string.IsNullOrEmpty(language))
                            {
                                language = "ru";
                            }
                        }
                        else
                        {
                            language = "ru";
                        }
                    }
                }
                else
                {
                    var cookie = new HttpCookie("language", language);
                    cookie.Expires.AddDays(365);
                    HttpContext.Current.Response.SetCookie(cookie);
                }

                var cultureInfo = new CultureInfo(language);
                //Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;

            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {

        }
    }
}