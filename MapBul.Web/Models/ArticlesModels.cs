using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.Web.Repository;

namespace MapBul.Web.Models
{
    public class ArticlesListModel
    {
        public ArticlesListModel(string guid)
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            var user = repo.GetUserByGuid(guid);
            var avaliableArticles = repo.GetArticles(guid);
            MyArticles = avaliableArticles.Where(m => m.AuthorId == user.Id).ToList();
            OtherArticles = avaliableArticles.Where(m => m.AuthorId != user.Id).ToList();
        }
        public List<article> MyArticles { get; set; }
        public List<article> OtherArticles { get; set; } 
    }

    public class NewArticleModel:article
    {
        public NewArticleModel() { }

        public NewArticleModel(article article)
        {
            foreach (var propertyInfo in article.GetType().GetProperties())
            {
                if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name == "String")
                {
                    propertyInfo.SetValue(this, propertyInfo.GetValue(article));
                }
            }
            SubCategories = article.articlesubcategory.Select(sc => sc.CategoryId).ToList();
        }

        public void CopyTo(article article)
        {
            foreach (var propertyInfo in article.GetType().GetProperties())
            {
                if (!propertyInfo.Name.Contains("Id") && (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType.Name == "String"))
                {
                    propertyInfo.SetValue(article, propertyInfo.GetValue(this));
                }
            }
        }

        public List<int> SubCategories { get; set; }
 
    }
}