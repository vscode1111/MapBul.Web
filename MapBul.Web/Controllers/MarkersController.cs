using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.Web.Auth;
using MapBul.Web.Models;
using MapBul.Web.Repository;
using Newtonsoft.Json;

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
            ViewBag.Statuses = repo.GetStatuses();
            return PartialView("Partial/_MarkersTablePartial",model);
        }

        public ActionResult _NewMarkerModalPartial()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var model=new NewMarkerModel();
            ViewBag.Cities = repo.GetCities();
            ViewBag.Categories = repo.GetCategories();
            ViewBag.Discounts = repo.GetDiscounts();
            ViewBag.Statuses = repo.GetStatuses();
            ViewBag.WeekDays = repo.GetWeekDays();
            return PartialView("Partial/_NewMarkerModalPartial",model);
        }

        [HttpPost]
        public bool AddNewMarker(NewMarkerModel model, string openTimesString, string closeTimesString, HttpPostedFileBase markerPhoto)
        {
            var openTimes = JsonConvert.DeserializeObject<List<WorkTimeDay>>(openTimesString);
            var closeTimes = JsonConvert.DeserializeObject<List<WorkTimeDay>>(closeTimesString);

            var repo = DependencyResolver.Current.GetService<IRepository>();
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid=auth.UserGuid;
            string filePath = markerPhoto == null ? null : FileProvider.FileProvider.SaveMarkerPhoto(markerPhoto);
            model.Photo = filePath;
            repo.AddMarker(model, openTimes, closeTimes, userGuid);

            return true;
        }

        [HttpPost]
        public bool ChangeMarkerStatus(int markerId, int statusId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            repo.ChangeMarkerStatus(markerId,statusId);
            return true;
        }

    }

    [Serializable]
    public class WorkTimeDay
    {
        public int WeekDayId { get; set; }
        public TimeSpan? Time { get; set; }
    }
}