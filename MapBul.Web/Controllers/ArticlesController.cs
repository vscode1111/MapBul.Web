﻿using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.Web.Auth;
using MapBul.Web.Models;
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
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            List<article> model = repo.GetArticles(auth.UserGuid);
            ViewBag.Statuses = repo.GetStatuses(userGuid);
            return PartialView("Partial/_ArticlesTablePartial",model);
        }

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

        public bool AddNewArticle(NewArticleModel model, HttpPostedFileBase articlePhoto, HttpPostedFileBase articleTitlePhoto)
        {
            if (articlePhoto != null)
                model.Photo = FileProvider.FileProvider.SaveArticlePhoto(articlePhoto);
            if(articleTitlePhoto!=null)
                model.TitlePhoto = FileProvider.FileProvider.SaveArticlePhoto(articleTitlePhoto);

            var repo = DependencyResolver.Current.GetService<IRepository>();
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            repo.AddNewArticle(model,userGuid);
            return true;
        }

        public bool ChangeArticleStatus(int articleId, int statusId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            repo.ChangeArticleStatus(articleId, statusId, userGuid);
            return true;
        }

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

        public bool EditArticle(NewArticleModel model, HttpPostedFileBase articlePhoto, HttpPostedFileBase articleTitlePhoto)
        {
            
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var auth = DependencyResolver.Current.GetService<IAuthProvider>();
            var userGuid = auth.UserGuid;
            if (articlePhoto != null)
            {
                FileProvider.FileProvider.DeleteFile(model.Photo);
                model.Photo = FileProvider.FileProvider.SaveArticlePhoto(articlePhoto);
            }
            if (articleTitlePhoto != null)
            {
                FileProvider.FileProvider.DeleteFile(model.Photo);
                model.TitlePhoto = FileProvider.FileProvider.SaveArticlePhoto(articleTitlePhoto);
            }
            repo.EditArticle(model, userGuid);

            return true;
        }
    }
}