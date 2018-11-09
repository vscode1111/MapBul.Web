using System.Web.Mvc;
using MapBul.XIsland.Models;
using MapBul.XIsland.Repository;

namespace MapBul.XIsland.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult _EventsListPartial(EventsListModel model)
        {
            if(model==null)
                model=new EventsListModel();
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult _EventInfoPartial(int id)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var model = repo.GetArticle(id);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult _ArticleInfoPartial(int id)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var model = repo.GetArticle(id);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult _ArticlesListPartial()
        {
            var model=new ArticlesListModel();
            return PartialView(model);
        }
    }
}