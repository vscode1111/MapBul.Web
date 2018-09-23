using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MapBul.DBContext;
using MapBul.SharedClasses;
using MapBul.SharedClasses.Constants;
using MapBul.Web.Repository;

namespace MapBul.Web.Auth
{
    public class DefaultAuthProvider : IAuthProvider
    {
        public bool Login(string email, string password)
        {
            try
            {
                var db = DependencyResolver.Current.GetService<IRepository>();
                var user = db.GetUserByLoginAndPassword(email, password);
                IIdentity identity = new GenericIdentity(user.Guid);
                HttpContext.Current.User = new GenericPrincipal(identity, new[] {user.usertype.Tag});
                FormsAuthentication.SetAuthCookie(user.Guid, true);
                return true;

            }
            catch (MyException)
            {
                return false;
            }
        }


        public void Logout()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.User = null;
        }

        public void RefreshPrincipal()
        {
            var db = DependencyResolver.Current.GetService<IRepository>();
            if (!IsAuthenticated) return;
            var guid = HttpContext.Current.User.Identity.Name;
            var user = db.GetUserByGuid(guid);
            if (user == null)
            {
                HttpContext.Current.User = null;
                return;
            }
            IIdentity identity = new GenericIdentity(user.Guid);
            HttpContext.Current.User = new GenericPrincipal(identity, new[] {UserTypes.Admin});
        }

        public bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current.User != null
                       && HttpContext.Current.User.Identity != null
                       && HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        public string UserType
        {
            get
            {
                var db = DependencyResolver.Current.GetService<IRepository>();
                var user = db.GetUserByGuid(HttpContext.Current.User.Identity.Name);
                return user.usertype.Tag;
            }
        }

        public string UserGuid
        {
            get
            {
                if(!IsAuthenticated)
                    throw new MyException(Errors.UserNotAuthorized);
                return HttpContext.Current.User.Identity.Name;

            }
        }

        public int UserId
        {
            get
            {
                var db = DependencyResolver.Current.GetService<IRepository>();
                var user = db.GetUserByGuid(HttpContext.Current.User.Identity.Name);
                return user.Id;
            }
        }

        public bool IsSuperAdmin
        {
            get
            {
                var repo = DependencyResolver.Current.GetService<IRepository>();
                return repo.GetAdmins().First(a => a.user.Guid == HttpContext.Current.User.Identity.Name).Superuser;
            }
        }
    }
}