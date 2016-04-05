using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MapBul.Web.Auth;

namespace MapBul.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var authProvider = DependencyResolver.Current.GetService<IAuthProvider>();
            authProvider.RefreshPrincipal();
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            /*if (!Request.IsSecureConnection)
            {
                string path = string.Format("https{0}", Request.Url.AbsoluteUri.Substring(4)); 
                Response.Redirect(path);
            }*/
        }
    }
}