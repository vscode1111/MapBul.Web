using System;
using System.Linq;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.SharedClasses;
using MapBul.SharedClasses.Constants;

namespace MapBul.XIsland.Helpers
{
    public static class HmtlHelper
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

        /*public static bool IsUserInRoles(this HtmlHelper html, params string[] roles)
        {
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            return roles.Contains(auth.UserType);
        }*/

        public static string MapImage(this HtmlHelper html, string path)
        {
            return "/" + path;
        }

        public static string GetUserName(this HtmlHelper html, user user)
        {
            switch (user.usertype.Tag)
            {
                case UserTypes.Editor:
                    return user.editor.First().LastName+user.editor.First().FirstName+user.editor.First().MiddleName;
                case UserTypes.Journalist:
                    return user.journalist.First().LastName + user.journalist.First().FirstName + user.journalist.First().MiddleName;
                case UserTypes.Tenant:
                    return user.tenant.First().LastName + user.tenant.First().FirstName + user.tenant.First().MiddleName;
                case UserTypes.Admin:
                    return "Администратор"; 
                default:
                    throw new MyException(Errors.NotFound);
            }
        }

    }
}