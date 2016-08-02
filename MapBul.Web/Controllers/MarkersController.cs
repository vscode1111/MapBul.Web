using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.SharedClasses;
using MapBul.SharedClasses.Constants;
using MapBul.Web.Auth;
using MapBul.Web.Models;
using MapBul.Web.Repository;
using Newtonsoft.Json;

namespace MapBul.Web.Controllers
{
    public class MarkersController : Controller
    {
        /// <summary>
        /// Главная страница раздела маркеров
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public ActionResult Index()
        {
            return View();
        }


#region partials

        /// <summary>
        /// Частичное представление списка маркеров
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// частичное представление модального окна формы добавления нового маркера
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _NewMarkerModalPartial()
        {
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var model = new NewMarkerModel();
            ViewBag.Cities = repo.GetCities();
            ViewBag.Categories = repo.GetMarkerCategories();
            ViewBag.Discounts = repo.GetDiscounts();
            ViewBag.Statuses = repo.GetStatuses(userGuid);
            ViewBag.WeekDays = repo.GetWeekDays();
            return PartialView("Partial/_NewMarkerModalPartial", model);
        }

        /// <summary>
        /// частичное представление модального окна формы редактирования маркера
        /// </summary>
        /// <param name="markerId"></param>
        /// <returns></returns>
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
            ViewBag.Categories = repo.GetMarkerCategories();
            ViewBag.Discounts = repo.GetDiscounts();
            ViewBag.Statuses = repo.GetStatuses(userGuid);
            ViewBag.WeekDays = repo.GetWeekDays();
            return PartialView("Partial/_EditMarkerModalPartial", model);

        }

#endregion

#region actions


        /// <summary>
        /// метод добавления нового маркера
        /// </summary>
        /// <param name="model"></param>
        /// <param name="openTimesString"></param>
        /// <param name="closeTimesString"></param>
        /// <param name="markerPhoto"></param>
        /// <param name="markerLogo"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool AddNewMarker(NewMarkerModel model, string openTimesString, string closeTimesString, HttpPostedFileBase markerPhoto, HttpPostedFileBase markerLogo)
        {
            var openTimes = JsonConvert.DeserializeObject<List<WorkTimeDay>>(openTimesString);
            var closeTimes = JsonConvert.DeserializeObject<List<WorkTimeDay>>(closeTimesString);
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            string photoPath = markerPhoto == null ? null : FileProvider.SaveMarkerPhoto(markerPhoto);
            string logoPath = markerPhoto == null ? null : FileProvider.SaveMarkerLogo(markerLogo);
            model.Photo = photoPath;
            model.Logo = logoPath;
            repo.AddMarker(model, openTimes, closeTimes, userGuid);

            return true;
        }

        /// <summary>
        /// метод сохранения изменений маркера
        /// </summary>
        /// <param name="model"></param>
        /// <param name="openTimesString"></param>
        /// <param name="closeTimesString"></param>
        /// <param name="markerPhoto"></param>
        /// <param name="markerLogo"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public bool EditMarker(NewMarkerModel model, string openTimesString, string closeTimesString,
            HttpPostedFileBase markerPhoto,  HttpPostedFileBase markerLogo)
        {
            var openTimes = JsonConvert.DeserializeObject<List<WorkTimeDay>>(openTimesString);
            var closeTimes = JsonConvert.DeserializeObject<List<WorkTimeDay>>(closeTimesString);

            var repo = DependencyResolver.Current.GetService<IRepository>();
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            if (markerPhoto != null)
            {
                FileProvider.DeleteFile(model.Photo);
                string filePath = FileProvider.SaveMarkerPhoto(markerPhoto);
                model.Photo = filePath;
            }
            if (markerLogo != null)
            {
                FileProvider.DeleteFile(model.Logo);
                string filePath = FileProvider.SaveMarkerLogo(markerLogo);
                model.Logo = filePath;
            }
            repo.EditMarker(model, openTimes, closeTimes, userGuid);

            return true;
        }

        /// <summary>
        /// Метод изменения статуса маркера
        /// </summary>
        /// <param name="markerId"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public bool ChangeMarkerStatus(int markerId, int statusId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            repo.ChangeMarkerStatus(markerId, statusId);
            return true;
        }

        /// <summary>
        /// метод удаления маркера
        /// </summary>
        /// <param name="markerId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor)]
        public bool DeleteMarker(int markerId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            repo.DeleteMarker(markerId);
            return true;
        }

#endregion
  
    }

}