using System.Collections.Generic;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.Web.Repository;

namespace MapBul.Web.Controllers
{
    public class ArticlesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _ArticlesTablePartial()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            List<article> model = repo.GetArticles();
            return PartialView("Partial/_ArticlesTablePartial",model);
        }
    }
}