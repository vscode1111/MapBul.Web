using System.Web.Mvc;
using MapBul.Web.Auth;
using MapBul.Web.Models;

namespace MapBul.Web.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            LoginModel model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            IAuthProvider auth = DependencyResolver.Current.GetService<IAuthProvider>();
            if (auth.Login(model.Login, model.Password))
                return RedirectToAction("Index", "Home");
            ViewBag.errorMessage = "Неправильные логин/пароль";
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            IAuthProvider auth = DependencyResolver.Current.GetService<IAuthProvider>();
            auth.Logout();
            return RedirectToAction("Index");
        }
    }

    
}