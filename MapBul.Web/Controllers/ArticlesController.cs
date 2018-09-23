using System.Web;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.SharedClasses;
using MapBul.SharedClasses.Constants;
using MapBul.Web.Auth;
using MapBul.Web.Filters;
using MapBul.Web.Models;
using MapBul.Web.Repository;

namespace MapBul.Web.Controllers
{
    [Culture]
    public class ArticlesController : Controller
    {
        /// <summary>
        /// Главная страница раздела статей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor + ", " + UserTypes.Journalist)]
        public ActionResult Index()
        {
            return View();
        }

        #region partials

        /// <summary>
        /// Частичное представление таблицы статей
        /// </summary>
        /// <returns></returns>
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor + ", " + UserTypes.Journalist)]
        public ActionResult _ArticlesTablePartial()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            var model = new ArticlesListModel(userGuid);
            ViewBag.Statuses = repo.GetStatuses(userGuid);
            return PartialView("Partial/_ArticlesTablePartial", model);
        }

        /// <summary>
        /// Частичное представление модального окна добавления новой статьи
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor + ", " + UserTypes.Journalist)]
        public ActionResult _NewArticleModalPartial()
        {
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var model = new NewArticleModel();
            ViewBag.Categories = repo.GetArticleCategories();
            ViewBag.Statuses = repo.GetStatuses(userGuid);
            ViewBag.Markers = repo.GetMarkers();
            ViewBag.Cities = repo.GetCities();
            return PartialView("Partial/_NewArticleModalPartial", model);
        }

        /// <summary>
        /// Частичное представление модального окна редактирования статьи
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor + ", " + UserTypes.Journalist)]
        public ActionResult _EditArticleModalPartial(int articleId)
        {
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var article = repo.GetArticle(articleId);
            var model = new NewArticleModel(article);
            ViewBag.Categories = repo.GetArticleCategories();
            ViewBag.Statuses = repo.GetStatuses(userGuid);
            ViewBag.Markers = repo.GetMarkers();
            ViewBag.Cities = repo.GetCities();
            return PartialView("Partial/_EditArticleModalPartial", model);
        }

        #endregion

        #region actions

        /// <summary>
        ///  метод добавления новой статьи
        /// </summary>
        /// <param name="model"></param>
        /// <param name="articlePhoto"></param>
        /// <param name="articleTitlePhoto"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor + ", " + UserTypes.Journalist)]
        public bool AddNewArticle(NewArticleModel model, HttpPostedFileBase articlePhoto,
            HttpPostedFileBase articleTitlePhoto)
        {
            if (articlePhoto != null)
                model.Photo = FileProvider.SaveArticlePhoto(articlePhoto);
            if (articleTitlePhoto != null)
                model.TitlePhoto = FileProvider.SaveArticleTitlePhoto(articleTitlePhoto);

            if (string.IsNullOrEmpty(model.Title) && !string.IsNullOrEmpty(model.TitleEn))
            {
                model.Title = model.TitleEn;
            }
            if (string.IsNullOrEmpty(model.Description) && !string.IsNullOrEmpty(model.DescriptionEn))
            {
                model.Description= model.DescriptionEn;
            }
            if (string.IsNullOrEmpty(model.Text) && !string.IsNullOrEmpty(model.TextEn))
            {
                model.Text = model.TextEn;
            }
            if (string.IsNullOrEmpty(model.SourcePhoto) && !string.IsNullOrEmpty(model.SourcePhotoEn))
            {
                model.SourcePhoto = model.SourcePhotoEn;
            }
            if (string.IsNullOrEmpty(model.SourceUrl) && !string.IsNullOrEmpty(model.SourceUrlEn))
            {
                model.SourceUrl = model.SourceUrlEn;
            }

            var repo = DependencyResolver.Current.GetService<IRepository>();
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            repo.AddNewArticle(model, userGuid);
            return true;
        }

        /// <summary>
        /// метод изменения статуса статьи
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor + ", " + UserTypes.Journalist)]
        public bool ChangeArticleStatus(int articleId, int statusId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            repo.ChangeArticleStatus(articleId, statusId, userGuid);
            return true;
        }

        /// <summary>
        /// метод сохранения изменений в статье
        /// </summary>
        /// <param name="model"></param>
        /// <param name="articleTitlePhoto"></param>
        /// <param name="articlePhoto"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor + ", " + UserTypes.Journalist)]
        public bool EditArticle(NewArticleModel model, HttpPostedFileBase articleTitlePhoto,
            HttpPostedFileBase articlePhoto)
        {

            var repo = DependencyResolver.Current.GetService<IRepository>();
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            if (articlePhoto != null)
            {
                FileProvider.DeleteFile(model.Photo);
                model.Photo = FileProvider.SaveArticlePhoto(articlePhoto);
            }
            if (articleTitlePhoto != null)
            {
                FileProvider.DeleteFile(model.TitlePhoto);
                model.TitlePhoto = FileProvider.SaveArticleTitlePhoto(articleTitlePhoto);
            }

            if (string.IsNullOrEmpty(model.Title) && !string.IsNullOrEmpty(model.TitleEn))
            {
                model.Title = model.TitleEn;
            }
            if (string.IsNullOrEmpty(model.Description) && !string.IsNullOrEmpty(model.DescriptionEn))
            {
                model.Description = model.DescriptionEn;
            }
            if (string.IsNullOrEmpty(model.Text) && !string.IsNullOrEmpty(model.TextEn))
            {
                model.Text = model.TextEn;
            }
            if (string.IsNullOrEmpty(model.SourcePhoto) && !string.IsNullOrEmpty(model.SourcePhotoEn))
            {
                model.SourcePhoto = model.SourcePhotoEn;
            }
            if (string.IsNullOrEmpty(model.SourceUrl) && !string.IsNullOrEmpty(model.SourceUrlEn))
            {
                model.SourceUrl = model.SourceUrlEn;
            }

            repo.EditArticle(model, userGuid);

            return true;
        }

        /// <summary>
        /// метод удаления статьи
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor + ", " + UserTypes.Journalist)]
        public bool DeleteArticle(int articleId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            repo.DeleteArticle(articleId);
            return true;
        }

        #endregion
    }
}