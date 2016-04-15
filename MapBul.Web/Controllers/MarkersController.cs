using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.SharedClasses.Constants;
using MapBul.Web.Auth;
using MapBul.Web.Models;
using MapBul.Web.Repository;
using Newtonsoft.Json;

namespace MapBul.Web.Controllers
{
    public class MarkersController : Controller
    {
        [HttpGet]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public ActionResult Index()
        {
            return View();
        }


#region partials

        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public ActionResult _MarkersTablePartial()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            var model = new MarkersListModel(userGuid);
            ViewBag.Statuses = repo.GetStatuses(userGuid);
            return PartialView("Partial/_MarkersTablePartial",model);
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _NewMarkerModalPartial()
        {
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var model = new NewMarkerModel();
            ViewBag.Cities = repo.GetCities();
            ViewBag.Categories = repo.GetCategories();
            ViewBag.Discounts = repo.GetDiscounts();
            ViewBag.Statuses = repo.GetStatuses(userGuid);
            ViewBag.WeekDays = repo.GetWeekDays();
            return PartialView("Partial/_NewMarkerModalPartial", model);
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public ActionResult _EditMarkerModalPartial(int markerId)
        {
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            var repo = DependencyResolver.Current.GetService<IRepository>();
            marker marker = repo.GetMarker(markerId);
            NewMarkerModel model = new NewMarkerModel(marker);
            ViewBag.Cities = repo.GetCities();
            ViewBag.Categories = repo.GetCategories();
            ViewBag.Discounts = repo.GetDiscounts();
            ViewBag.Statuses = repo.GetStatuses(userGuid);
            ViewBag.WeekDays = repo.GetWeekDays();
            return PartialView("Partial/_EditMarkerModalPartial", model);

        }

#endregion

#region actions

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool AddNewMarker(NewMarkerModel model, string openTimesString, string closeTimesString, HttpPostedFileBase markerPhoto)
        {
            var openTimes = JsonConvert.DeserializeObject<List<WorkTimeDay>>(openTimesString);
            var closeTimes = JsonConvert.DeserializeObject<List<WorkTimeDay>>(closeTimesString);
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            string filePath = markerPhoto == null ? null : FileProvider.FileProvider.SaveMarkerPhoto(markerPhoto);
            model.Photo = filePath;
            repo.AddMarker(model, openTimes, closeTimes, userGuid);

            return true;
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public bool EditMarker(NewMarkerModel model, string openTimesString, string closeTimesString,
            HttpPostedFileBase markerPhoto)
        {
            var openTimes = JsonConvert.DeserializeObject<List<WorkTimeDay>>(openTimesString);
            var closeTimes = JsonConvert.DeserializeObject<List<WorkTimeDay>>(closeTimesString);

            var repo = DependencyResolver.Current.GetService<IRepository>();
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            if (markerPhoto != null)
            {
                FileProvider.FileProvider.DeleteFile(model.Photo);
                string filePath = FileProvider.FileProvider.SaveMarkerPhoto(markerPhoto);
                model.Photo = filePath;
            }
            repo.EditMarker(model, openTimes, closeTimes, userGuid);

            return true;
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public bool ChangeMarkerStatus(int markerId, int statusId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            repo.ChangeMarkerStatus(markerId, statusId);
            return true;
        }

#endregion
  
    }


    [Serializable]
    public class WorkTimeDay
    {
        public int WeekDayId { get; set; }
        public TimeSpan? Time { get; set; }
    }
}