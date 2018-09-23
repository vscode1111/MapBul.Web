using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
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
            Countries = repo.GetCountries().Select(c => new CountrieTranslate(c)).ToList();
//            Regions = repo.GetRegions();
            Cities = repo.GetCities().Select(c => new CityTranslate(c)).ToList();

        }
        public List<CountrieTranslate> Countries { get; set; }
        public List<region> Regions { get; set; }
        public List<CityTranslate> Cities { get; set; }
    }

    public class CountrieTranslate
    {
        public CountrieTranslate(country country)
        {
            Country = country;
            //CountryName = Translate(country.Name);
            CountryName = country.Name;
        }

        public country Country;
        public string CountryName;

        private string Translate(string name)
        {
            var request = HttpContext.Current.Request;
            string lang;
            if (request.UserLanguages?.Length > 0)
            {
                lang = request.UserLanguages[0].Substring(0, 2);
            }
            else
            {
                lang = "en";
            }
            //https://translate.google.com/translate_t/?client=j&ie=UTF8&text=apple&langpair=auto|ru
            try
            {
                var url =
                    $"https://translate.google.com/translate_a/t?client=j&ie=UTF8&text={name}&langpair=auto|{lang}";
                var webClient = new WebClient {Encoding = System.Text.Encoding.UTF8};
                var result = webClient.DownloadString(url);
                var temp = result.Split('\"')[1];
                return temp;
            }
            catch
            {
                return name;
            }
        }
    }

    public class CityTranslate
    {
        public CityTranslate(city city)
        {
            City = city;
            //CityName = Translate(city.Name);
            CityName = city.Name;
            //CountryName = Translate(city.country.Name);
            CountryName = city.country.Name;
        }

        public city City;
        public string CityName;
        public string CountryName;

        private string Translate(string name)
        {
            var request = HttpContext.Current.Request;
            string lang;
            if (request.UserLanguages?.Length > 0)
            {
                lang = request.UserLanguages[0].Substring(0, 2);
            }
            else
            {
                lang = "en";
            }
            try
            {
                var url =
                    $"https://translate.google.com/translate_a/t?client=j&ie=UTF8&text={name}&langpair=auto|{lang}";
                var webClient = new WebClient();
                webClient.Encoding = System.Text.Encoding.UTF8;
                var result = webClient.DownloadString(url);
                var temp = result.Split('\"')[1];
                return temp;
            }
            catch
            {
                return name;
            }
        }

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