using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MapBul.SharedClasses;
using MapBul.Web.Repository;

namespace MapBul.Web.Auth
{
    public class MyAuthAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            try
            {
                var user = repo.GetUserByGuid(httpContext.User.Identity.Name);
                return Roles.Contains(user.usertype.Tag);
            }
            catch (MyException e)
            {
                return false;
            }

        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                    {"action", "Index"},
                    {"controller", "Login"}
                });
        }
    }
}