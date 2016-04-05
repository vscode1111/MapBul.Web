using System.Collections.Generic;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.Web.Models;
using MapBul.Web.Repository;

namespace MapBul.Web.Controllers
{
    public class MarkersController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _MarkersTablePartial()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            List<marker> model = repo.GetMarkers();
            return PartialView("Partial/_MarkersTablePartial",model);
        }

        public ActionResult _NewMarkerModalPartial()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var model=new NewMarkerModel();
            ViewBag.Cities = repo.GetCities();
            return PartialView("Partial/_NewMarkerModalPartial",model);
        }
    }
}