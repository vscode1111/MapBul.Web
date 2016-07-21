using System;
using System.Collections.Generic;
using System.Linq;
using MapBul.DBContext;
using MapBul.SharedClasses.Constants;
using MapBul.XIsland.Models;

namespace MapBul.XIsland.Repository
{
    public class MySqlRepository : IRepository
    {
        private readonly Context _db = new Context();

        public List<article> GetEvents()
        {
            return
                _db.article.Where(a => a.status.Tag == MarkerStatuses.Published && a.StartDate != null&&a.StartDate>DateTime.Now)
                    .OrderBy(a => a.StartDate)
                    .Take(20)
                    .ToList();
        }

        public List<article> GetArticles()
        {
            return
                _db.article.Where(a => a.status.Tag == MarkerStatuses.Published && a.StartDate == null)
                    .OrderByDescending(a => a.PublishedDate)
                    .Take(20)
                    .ToList();
        }

        public article GetArticle(int id)
        {
            return _db.article.First(a => a.Id == id);
        }

        public List<article> GetEvents(EventsListModel eventsListModel)
        {
            return
                _db.article.Where(
                    a =>
                        a.status.Tag == MarkerStatuses.Published && a.StartDate != null &&
                        eventsListModel.StartDateTime <= a.StartDate &&
                        a.StartDate <= eventsListModel.EndDateTime)
                    .OrderByDescending(a => a.PublishedDate)
                    .Take(20)
                    .ToList();
        }
    }
}