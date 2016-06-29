using System.Collections.Generic;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.Web.Repository;

namespace MapBul.Web.Models
{
    public class CitiesModel
    {
        public CitiesModel()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            Countries = repo.GetCountries();
//            Regions = repo.GetRegions();
            Cities = repo.GetCities();
        }
        public List<country> Countries { get; set; }
        public List<region> Regions { get; set; }
        public List<city> Cities { get; set; }
    }

    public class CategoriesModel
    {
        public CategoriesModel(bool forArticle=false)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            Categories = forArticle ? repo.GetArticleCategories() : repo.GetMarkerCategories();
            ForArticle = forArticle;
        }

        public List<category> Categories { get; set; }
        public bool ForArticle { get; set; }
 
    }
}