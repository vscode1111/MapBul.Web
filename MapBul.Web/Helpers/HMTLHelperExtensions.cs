using System;
using System.Linq;
using System.Web.Mvc;
using MapBul.Web.Auth;

namespace MapBul.Web.Helpers
{
    public static class HmtlHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null)
        {
            string cssClass = "active";
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

        public static bool IsUserInRoles(this HtmlHelper html, params string[] roles)
        {
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            return roles.Contains(auth.UserType);
        }

    }
}
