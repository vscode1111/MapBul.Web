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
using MapBul.Web.Filters;
using MapBul.Web.Models;
using MapBul.Web.Repository;
using Newtonsoft.Json;

namespace MapBul.Web.Controllers
{
    /// <summary>
    /// Класс для десериализации древовидной структуры категорий
    /// </summary>
    [Serializable]
    public class NestableElement
    {
        public int id { get; set; }
        public List<NestableElement> children { get; set; }
    }


    [Culture]
    public class DictionariesController : Controller
    {
        /// <summary>
        /// Главная страница раздела словарей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult Index()
        {
            return View();
        }

#region partials

        /// <summary>
        /// частичное представление списка городов и стран
        /// </summary>
        /// <returns></returns>
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _CitiesPartial()
        {
            var model = new CitiesModel();
            return PartialView("Partial/_CitiesPartial", model);
        }

        /// <summary>
        /// частичное представление модального окна редактирования категории
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _EditCategoryModalPartial(int categoryId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var model = repo.GetCategory(categoryId);
            var categories = model.ForArticle ? repo.GetArticleCategories() : repo.GetMarkerCategories();
            ViewBag.Categories = categories.Where(c => c.Id != model.Id).ToList();
            return PartialView("Partial/_EditCategoryModalPartial", model);
        }

        /// <summary>
        /// частичное представление страницы категорий маркеров
        /// </summary>
        /// <returns></returns>
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _MarkerCategoriesPartial()
        {
            var model = new CategoriesModel();
            return PartialView("Partial/_CategoriesPartial", model);
        }

        /// <summary>
        /// частичное представление страницы категорий статей
        /// </summary>
        /// <returns></returns>
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _ArticleCategoriesPartial()
        {
            var model = new CategoriesModel(true);
            return PartialView("Partial/_CategoriesPartial", model);
        }

        /// <summary>
        /// частичное представление блока добавления новой категории маркеров
        /// </summary>
        /// <returns></returns>
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult _NewMarkerCategoryPartial()
        {
            var model = new category {ForArticle = false};
            var repo = DependencyResolver.Current.GetService<IRepository>();
            ViewBag.Categories = repo.GetMarkerCategories();
            return PartialView("Partial/_NewCategoryPartial", model);
        }

        /// <summary>
        /// частичное представление блока добавления новой категории статей
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Метод удаления категории
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult DeleteCategory(int categoryId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            repo.DeleteCategory(categoryId);
            return new JsonResult {JsonRequestBehavior = JsonRequestBehavior.DenyGet, Data = new {success = true}};
        }

        /// <summary>
        /// метод удаления страны
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult DeleteCountry(int countryId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            repo.DeleteCountry(countryId);
            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.DenyGet, Data = new { success = true } };
        }

        /// <summary>
        /// Метод удаления города
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public ActionResult DeleteCity(int cityId)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            repo.DeleteCity(cityId);
            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.DenyGet, Data = new { success = true } };
        }

        /// <summary>
        /// метод добавления новой страны
        /// </summary>
        /// <param name="name"></param>
        /// <param name="placeId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool AddCountry(string name, string placeId, string code)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
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

        /// <summary>
        /// метод добавления нового города
        /// </summary>
        /// <param name="name"></param>
        /// <param name="countryId"></param>
        /// <param name="placeId"></param>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool AddCity(string name, int countryId, string placeId, string lat, string lng)
        {
            var latConverted = Convert.ToSingle(lat,
                new NumberFormatInfo { NumberDecimalSeparator = ".", NumberGroupSeparator = "," });
            var lngConverted = Convert.ToSingle(lng,
                new NumberFormatInfo { NumberDecimalSeparator = ".", NumberGroupSeparator = "," });
            var repo = DependencyResolver.Current.GetService<IRepository>();
            repo.AddCity(name, countryId, placeId, latConverted, lngConverted);
            return true;
        }

        /// <summary>
        /// метод изменения древовидной структуры категорий
        /// </summary>
        /// <param name="structure">Новая структура категорий приходит в виде JSON строки</param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool SaveCategoriesStructure(string structure)
        {
            var serializedStructure = JsonConvert.DeserializeObject<List<NestableElement>>(structure);
            var repo = DependencyResolver.Current.GetService<IRepository>();
            repo.SaveCategoriesStructure(serializedStructure);
            return true;
        }

        /// <summary>
        /// Метод добавления новой категории
        /// </summary>
        /// <param name="model"></param>
        /// <param name="categoryIcon"></param>
        /// <param name="categoryPin"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool AddNewCategory(category model, HttpPostedFileBase categoryIcon, HttpPostedFileBase categoryPin)
        {
            try
            {
                var repo = DependencyResolver.Current.GetService<IRepository>();

                var filePath = FileProvider.SaveCategoryIcon(categoryIcon);
                model.Icon = filePath;

                filePath = FileProvider.SaveCategoryIcon(categoryPin);
                model.Pin = filePath;

                model.Color = model.Color.Replace("#", "");

                if (string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.EnName))
                {
                    model.Name = model.EnName;
                }

                repo.AddNewCategory(model);
            }
            catch (Exception e)
            {
                System.IO.File.AppendAllText(@"C:\temp\mapbul.txt", e.StackTrace + Environment.NewLine);
                throw;
            }

            return true;
        }

        /// <summary>
        /// метод сохранения изменений в категории
        /// </summary>
        /// <param name="model"></param>
        /// <param name="categoryIcon"></param>
        /// <param name="categoryPin"></param>
        /// <returns></returns>
        [HttpPost]
        [MyAuth(Roles = UserTypes.Admin)]
        public bool EditCategory(category model, HttpPostedFileBase categoryIcon, HttpPostedFileBase categoryPin)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var prevCategory = repo.GetCategory(model.Id);
            var previousIcon = prevCategory.Icon;
            var previousPin = prevCategory.Pin;

            if (categoryIcon != null)
            {
                FileProvider.DeleteFile(previousIcon);
                var filePath = FileProvider.SaveCategoryIcon(categoryIcon);
                model.Icon = filePath;
            }
            else
                model.Icon = previousIcon;



            if (categoryPin != null)
            {
                FileProvider.DeleteFile(previousPin);
                var filePath = FileProvider.SaveCategoryIcon(categoryPin);
                model.Pin = filePath;
            }
            else
                model.Pin = previousPin;

            model.Color = model.Color.Replace("#", "");


            if (string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.EnName))
            {
                model.Name = model.EnName;
            }

            repo.EditCategory(model);
            return true;
        }
#endregion

        
    }

}