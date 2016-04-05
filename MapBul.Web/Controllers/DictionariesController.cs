using System;
using System.Collections.Generic;
using System.Linq;
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
    [Serializable]
    public class NestableElement
    {
        public int id { get; set; }
        public List<NestableElement> children { get; set; }
    }

    public class DictionariesController : Controller
    {
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
            ViewBag.Categories = repo.GetCategories().Where(c => c.Id != model.Id).ToList();
            return PartialView("Partial/_EditCategoryModalPartial", model);
        }

        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _CategoriesPartial()
        {
            var model = new CategoriesModel();
            return PartialView("Partial/_CategoriesPartial", model);
        }

        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _NewCategoryPartial()
        {
            var model = new category();
            var repo = DependencyResolver.Current.GetService<IRepository>();
            ViewBag.Categories = repo.GetCategories();
            return PartialView("Partial/_NewCategoryPartial", model);
        }
#endregion

#region actions
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool AddCountry(string name)
        {
            IRepository repo = DependencyResolver.Current.GetService<IRepository>();
            repo.AddCountry(name);
            return true;
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool AddRegion(string name, int countryId)
        {
            IRepository repo = DependencyResolver.Current.GetService<IRepository>();
            repo.AddRegion(name, countryId);
            return true;
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool AddCity(string name, int regionId)
        {
            IRepository repo = DependencyResolver.Current.GetService<IRepository>();
            repo.AddCity(name, regionId);
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
        public bool AddNewCategory(category model, HttpPostedFileBase categoryIcon)
        {
            IRepository repo = DependencyResolver.Current.GetService<IRepository>();
            string filePath = FileProvider.FileProvider.SaveCategoryIcon(categoryIcon);
            model.Icon = filePath;
            repo.AddNewCategory(model);
            return true;
        }

        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool EditCategory(category model, HttpPostedFileBase categoryIcon)
        {
            IRepository repo = DependencyResolver.Current.GetService<IRepository>();
            var previousIcon = repo.GetCategory(model.Id).Icon;
            if (categoryIcon != null)
            {
                FileProvider.FileProvider.DeleteFile(previousIcon);
                string filePath = FileProvider.FileProvider.SaveCategoryIcon(categoryIcon);
                model.Icon = filePath;
            }
            else
                model.Icon = previousIcon;
            
            repo.EditCategory(model);
            return true;
        }
#endregion

    }

}