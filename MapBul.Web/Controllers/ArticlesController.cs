using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.SharedClasses;
using MapBul.SharedClasses.Constants;
using MapBul.Web.Auth;
using MapBul.Web.Models;
using MapBul.Web.Repository;

namespace MapBul.Web.Controllers
{
    public class ArticlesController : Controller
    {
        [HttpGet]
        [MyAuth(Roles = UserTypes.Admin+", "+UserTypes.Editor+", "+UserTypes.Journalist)]
        public ActionResult Index()
        {
            return View();
        }

        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor + ", " + UserTypes.Journalist)]
        public ActionResult _ArticlesTablePartial()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            var model = new ArticlesListModel(userGuid);
            ViewBag.Statuses = repo.GetStatuses(userGuid);
            return PartialView("Partial/_ArticlesTablePartial",model);
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor + ", " + UserTypes.Journalist)]
        public ActionResult _NewArticleModalPartial()
        {
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            var repo = DependencyResolver.Current.GetService<IRepository>();
            NewArticleModel model = new NewArticleModel();
            ViewBag.Categories = repo.GetCategories();
            ViewBag.Statuses = repo.GetStatuses(userGuid);
            ViewBag.Markers = repo.GetMarkers();
            return PartialView("Partial/_NewArticleModalPartial",model);
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor + ", " + UserTypes.Journalist)]
        public bool AddNewArticle(NewArticleModel model, HttpPostedFileBase articlePhoto, HttpPostedFileBase articleTitlePhoto)
        {
            if (articlePhoto != null)
                model.Photo = FileProvider.SaveArticlePhoto(articlePhoto);
            if(articleTitlePhoto!=null)
                model.TitlePhoto = FileProvider.SaveArticleTitlePhoto(articleTitlePhoto);

            var repo = DependencyResolver.Current.GetService<IRepository>();
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            repo.AddNewArticle(model,userGuid);
            return true;
        }

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

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor + ", " + UserTypes.Journalist)]
        public ActionResult _EditArticleModalPartial(int articleId)
        {
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            var repo = DependencyResolver.Current.GetService<IRepository>();
            article article=repo.GetArticle(articleId);
            var model=new NewArticleModel(article);
            ViewBag.Categories = repo.GetCategories();
            ViewBag.Statuses = repo.GetStatuses(userGuid);
            ViewBag.Markers = repo.GetMarkers();
            return PartialView("Partial/_EditArticleModalPartial",model);
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin + ", " + UserTypes.Editor + ", " + UserTypes.Journalist)]
        public bool EditArticle(NewArticleModel model,  HttpPostedFileBase articleTitlePhoto, HttpPostedFileBase articlePhoto)
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
            repo.EditArticle(model, userGuid);

            return true;
        }
    }
}