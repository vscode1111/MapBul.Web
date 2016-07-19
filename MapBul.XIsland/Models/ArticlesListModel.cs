using System.Collections.Generic;
using System.Web.Mvc;
using MapBul.DBContext;
using MapBul.XIsland.Repository;

namespace MapBul.XIsland.Models
{
    public class ArticlesListModel
    {
        public ArticlesListModel()
        {
            var repo = DependencyResolver.Current.GetService<IRepository>();
            Articles = repo.GetArticles();
        }
        public List<article> Articles { get; set; } 
    }
}