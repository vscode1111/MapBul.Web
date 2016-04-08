using System.Collections.Generic;
using System.Linq;
using MapBul.DBContext;

namespace MapBul.Web.Models
{
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