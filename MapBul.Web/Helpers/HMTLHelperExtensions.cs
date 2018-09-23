using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MapBul.Web.Auth;

namespace MapBul.Web.Helpers
{
    public static class HmtlHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null)
        {
            var cssClass = "active";
            var currentAction = (string)html.ViewContext.RouteData.Values["action"];
            var currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            var currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

        public static bool IsUserInRoles(this HtmlHelper html, params string[] roles)
        {
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            return roles.Contains(auth.UserType);
        }

        public static string MapImage(this HtmlHelper html, string path)
        {
            return "/" + path;
        }

    }
}
