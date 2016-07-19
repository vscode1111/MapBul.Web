using System.Collections.Generic;
using MapBul.DBContext;
using MapBul.XIsland.Models;

namespace MapBul.XIsland.Repository
{
    public interface IRepository
    {
        List<article> GetEvents();
        List<article> GetArticles();
        article GetArticle(int id);
        List<article> GetEvents(EventsListModel eventsListModel);
    }
}