using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    [Serializable]
    public class NestableElement
    {
        public int id { get; set; }
        public List<NestableElement> children { get; set; }
    }

    public class DictionariesController : Controller
    {
        [HttpGet]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult Index()
        {
            return View();
        }

#region partials

        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _CitiesPartial()
        {
            var model = new CitiesModel();
            return PartialView("Partial/_CitiesPartial", model);
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _EditCategoryModalPartial(int categoryId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            category model = repo.GetCategory(categoryId);
            var categories = model.ForArticle ? repo.GetArticleCategories() : repo.GetMarkerCategories();
            ViewBag.Categories = categories.Where(c => c.Id != model.Id).ToList();
            return PartialView("Partial/_EditCategoryModalPartial", model);
        }

        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _MarkerCategoriesPartial()
        {
            var model = new CategoriesModel();
            return PartialView("Partial/_CategoriesPartial", model);
        }

        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _ArticleCategoriesPartial()
        {
            var model = new CategoriesModel(true);
            return PartialView("Partial/_CategoriesPartial", model);
        }

        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _NewMarkerCategoryPartial()
        {
            var model = new category {ForArticle = false};
            var repo = DependencyResolver.Current.GetService<IRepository>();
            ViewBag.Categories = repo.GetMarkerCategories();
            return PartialView("Partial/_NewCategoryPartial", model);
        }

        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _NewArticleCategoryPartial()
        {
            var model = new category{ForArticle = true};
            var repo = DependencyResolver.Current.GetService<IRepository>();
            ViewBag.Categories = repo.GetArticleCategories();
            return PartialView("Partial/_NewCategoryPartial", model);
        }
#endregion

#region actions
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool AddCountry(string name, string placeId, string code)
        {
            IRepository repo = DependencyResolver.Current.GetService<IRepository>();
            repo.AddCountry(name, placeId, code);
            return true;
        }
/*
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool AddRegion(string name, int countryId, string placeId)
        {
            IRepository repo = DependencyResolver.Current.GetService<IRepository>();
            repo.AddRegion(name, countryId,placeId);
            return true;
        }*/

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool AddCity(string name, int countryId, string placeId, string lat, string lng)
        {
            var latConverted = Convert.ToSingle(lat,
                new NumberFormatInfo { NumberDecimalSeparator = ".", NumberGroupSeparator = "," });
            var lngConverted = Convert.ToSingle(lng,
                new NumberFormatInfo { NumberDecimalSeparator = ".", NumberGroupSeparator = "," });
            IRepository repo = DependencyResolver.Current.GetService<IRepository>();
            repo.AddCity(name, countryId, placeId, latConverted, lngConverted);
            return true;
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool SaveCategoriesStructure(string structure)
        {
            var serializedStructure = JsonConvert.DeserializeObject<List<NestableElement>>(structure);
            IRepository repo = DependencyResolver.Current.GetService<IRepository>();
            repo.SaveCategoriesStructure(serializedStructure);
            return true;
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool AddNewCategory(category model, HttpPostedFileBase categoryIcon, HttpPostedFileBase categoryPin)
        {
            IRepository repo = DependencyResolver.Current.GetService<IRepository>();

            string filePath = FileProvider.SaveCategoryIcon(categoryIcon);
            model.Icon = filePath;

            filePath = FileProvider.SaveCategoryIcon(categoryPin);
            model.Pin = filePath;

            model.Color = model.Color.Replace("#", "");
            repo.AddNewCategory(model);
            return true;
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool EditCategory(category model, HttpPostedFileBase categoryIcon, HttpPostedFileBase categoryPin)
        {
            IRepository repo = DependencyResolver.Current.GetService<IRepository>();
            var prevCategory = repo.GetCategory(model.Id);
            var previousIcon = prevCategory.Icon;
            var previousPin = prevCategory.Pin;

            if (categoryIcon != null)
            {
                FileProvider.DeleteFile(previousIcon);
                string filePath = FileProvider.SaveCategoryIcon(categoryIcon);
                model.Icon = filePath;
            }
            else
                model.Icon = previousIcon;



            if (categoryPin != null)
            {
                FileProvider.DeleteFile(previousPin);
                string filePath = FileProvider.SaveCategoryIcon(categoryPin);
                model.Pin = filePath;
            }
            else
                model.Pin = previousPin;

            model.Color = model.Color.Replace("#", "");
            repo.EditCategory(model);
            return true;
        }
#endregion

        
    }

}